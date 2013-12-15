using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using submgr = Microsoft.Dss.Services.SubscriptionManager;

using drive = Microsoft.Robotics.Services.Drive.Proxy;
using motor = Microsoft.Robotics.Services.Motor.Proxy;
using analog = Microsoft.Robotics.Services.AnalogSensor.Proxy;
using sonar = Microsoft.Robotics.Services.Sonar.Proxy;
using contact = Microsoft.Robotics.Services.ContactSensor.Proxy;
using Microsoft.Dss.Core;

namespace RobcioDSS
{
    [Contract(Contract.Identifier)]
    [DisplayName("RobcioDSS")]
    [Description("RobcioDSS service (no description provided)")]
    public partial class RobcioDSSService : DsspServiceBase
    {
        /// <summary>
        /// Service state
        /// </summary>
        [ServiceState]
        RobcioDSSState _state = new RobcioDSSState();

        /// <summary>
        /// Main service port
        /// </summary>
        [ServicePort("/RobcioDSS", AllowMultipleInstances = true)]
        RobcioDSSOperations _mainPort = new RobcioDSSOperations();

        [SubscriptionManagerPartner]
        submgr.SubscriptionManagerPort _submgrPort = new submgr.SubscriptionManagerPort();


      


        #region Constants

        /// <summary>
        /// Power used for drive
        /// </summary>
        public const double DrivePower = 0.25;

        /// <summary>
        /// Power used for claw
        /// </summary>
        public const double ClawPower = 0.10;

        /// <summary>
        /// Claw movement duration (in ms)
        /// </summary>
        public const int ClawAction = 2000;

        /// <summary>
        /// Light sensor limit (between black line and other color)
        /// </summary>
        public const double LightLimit = 45;

        /// <summary>
        /// Base for searching in one direction period
        /// </summary>
        public const int BaseSeekPeriod = 1500;

        /// <summary>
        /// Base direction for seek
        /// </summary>
        public const bool BaseSeekDirection = true;

        #endregion


        #region Variables

      

        private DateTime _seekLastTime = DateTime.MinValue;
   


        #endregion


        #region Partner

        /// <summary>
        /// Drive partner
        /// </summary>
        [Partner("Drive", Contract = drive.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
        private drive.DriveOperations _drivePort = new drive.DriveOperations();

        /// <summary>
        /// Claw motor partner
        /// </summary>
        [Partner("Claw", Contract = motor.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
        motor.MotorOperations _clawPort = new motor.MotorOperations();

        /// <summary>
        /// Light Sensor partner
        /// </summary>
        [Partner("Light", Contract = analog.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
        analog.AnalogSensorOperations _analogLightPort = new analog.AnalogSensorOperations();
        analog.AnalogSensorOperations _analogLightNotify = new analog.AnalogSensorOperations();

        /// <summary>
        /// Ultrasonic Sensor partner
        /// </summary>
        [Partner("SonarUltrasonic", Contract = analog.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
        analog.AnalogSensorOperations _analogSonarUltrasonicPort = new analog.AnalogSensorOperations();
        analog.AnalogSensorOperations _analogSonarUltrasonicNotify = new analog.AnalogSensorOperations();

        /// <summary>
        /// Ultrasonic Sensor partner
        /// </summary>
        [Partner("Sonar", Contract = sonar.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
        sonar.SonarOperations _sonarSonarPort = new sonar.SonarOperations();
        sonar.SonarOperations _sonarSonarNotify = new sonar.SonarOperations();


        /// <summary>
        /// Compass partner
        /// </summary>
        [Partner("Compass", Contract = analog.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
        analog.AnalogSensorOperations _analogCompassPort = new analog.AnalogSensorOperations();
        analog.AnalogSensorOperations _analogCompassNotify = new analog.AnalogSensorOperations();

        /// <summary>
        /// Bumper sensor partner
        /// </summary>
        [Partner("Bumper", Contract = contact.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
        contact.ContactSensorArrayOperations _contactTouchPort = new contact.ContactSensorArrayOperations();
        contact.ContactSensorArrayOperations _contactTouchNotify = new contact.ContactSensorArrayOperations();

        #endregion

        /// <summary>
        /// Service constructor
        /// </summary>
        public RobcioDSSService(DsspServiceCreationPort creationPort)
            : base(creationPort)
        {
        }



        /// <summary>
        /// Service Start
        /// </summary>
        protected override void Start()
        {
            base.Start();
            
            // We span an iterator to initialize the robot
            SpawnIterator(Initialize);
            SpawnIterator(InitializeMindRobcio);
            
        }

     


        /// <summary>
        /// Gets initial sensor informations, subscribes to sensors notifications and launch the main loop
        /// </summary>

        private IEnumerator<ITask> Initialize()
        {
            // Retreives sonar state
            yield return Arbiter.Choice(
                _analogSonarUltrasonicPort.Get(),
                (success) => _state.SonarUltrasonicState = success,
                (fault) => LogError(fault));
            // Retreives sonar state
            yield return Arbiter.Choice(
                _sonarSonarPort.Get(),
                (success) => _state.SonarState = success,
                (fault) => LogError(fault));

            // Retreives sonar state
            yield return Arbiter.Choice(
                _analogCompassPort.Get(),
                (success) => _state.CompassState = success,
                (fault) => LogError(fault));

            // Retreive light state
            yield return Arbiter.Choice(
                _analogLightPort.Get(),
                (success) => _state.LightState = success,
                (fault) => LogError(fault));

            // Retreive bumper state
            yield return Arbiter.Choice(
                _contactTouchPort.Get(),
                (success) => _state.TouchState = success.Sensors[0],
                (fault) => LogError(fault));

            // Registers receivers for sensor notifications
            // As they modify the service state, they are bound to exclusive group
            MainPortInterleave.CombineWith(
               Arbiter.Interleave(
                   new TeardownReceiverGroup(),
                   new ExclusiveReceiverGroup
                   (
                       Arbiter.Receive<analog.Replace>(true, _analogCompassNotify, CompassReplaceHandler),
                       Arbiter.Receive<analog.Replace>(true, _analogSonarUltrasonicNotify, SonarUltrasonicReplaceHandler),
                       Arbiter.Receive<sonar.Replace>(true, _sonarSonarNotify, SonarReplaceHandler),
                       Arbiter.Receive<analog.Replace>(true, _analogLightNotify, LightReplaceHandler),
                       Arbiter.Receive<contact.Update>(true, _contactTouchNotify, TouchUpdateHandler)
                   ),
                   new ConcurrentReceiverGroup()));

            // Subscribes to sensor notification
            _sonarSonarPort.Subscribe(_sonarSonarNotify);
            _analogCompassPort.Subscribe(_analogCompassNotify);
            _analogSonarUltrasonicPort.Subscribe(_analogSonarUltrasonicNotify);
            _analogLightPort.Subscribe(_analogLightNotify);
            _contactTouchPort.Subscribe(_contactTouchNotify);

            // Launch the main loop
            //OnStateChange();
            

            


            yield break;
        }




        #region Action

        /// <summary>
        /// Robot moves forward
        /// </summary>
        private IEnumerator<ITask> Forward()
        {
            _drivePort.SetDrivePower(DrivePower, DrivePower);
            yield break;
        }
        /// <summary>
        /// Robot moves back
        /// </summary>
        private IEnumerator<ITask> Back()
        {
            _drivePort.SetDrivePower(-DrivePower, -DrivePower);
            yield break;
        }

        /// <summary>
        /// Robot moves forward at maximum speed
        /// </summary>
        private IEnumerator<ITask> FastForward()
        {
            _drivePort.SetDrivePower(1, 1);
            yield break;
        }

        /// <summary>
        /// Robot stops
        /// </summary>
        private IEnumerator<ITask> Halt()
        {
            _drivePort.SetDrivePower(0.0d, 0.0d);
            yield break;
        }

        /// <summary>
        /// Robot turns left
        /// </summary>
        private IEnumerator<ITask> Left()
        {
            _drivePort.SetDrivePower(-DrivePower / 4.0d, DrivePower / 4.0d);
            yield break;
        }

        /// <summary>
        /// Robot turns right
        /// </summary>
        private IEnumerator<ITask> Right()
        {
            _drivePort.SetDrivePower(DrivePower / 4.0d, -DrivePower / 4.0d);
            yield break;
        }

        /// <summary>
        /// Robot closes claw
        /// </summary>
        private IEnumerator<ITask> CloseClaw()
        {
            _clawPort.SetMotorPower(-ClawPower);
            yield return Arbiter.Receive<DateTime>(false, TimeoutPort(ClawAction), (time) => { });
            _clawPort.SetMotorPower(0.0d);
            yield break;
        }

        /// <summary>
        /// Robot opens claw
        /// </summary>
        private IEnumerator<ITask> OpenClaw()
        {
            _clawPort.SetMotorPower(ClawPower);
            yield return Arbiter.Receive<DateTime>(false, TimeoutPort(ClawAction), (time) => { });
            _clawPort.SetMotorPower(0.0d);
            yield break;
        }

        #endregion



     

        #region Sensors Events

        /// <summary>
        /// Sonar notification handler
        /// </summary>
        private void SonarUltrasonicReplaceHandler(analog.Replace replace)
        {
            
            if (Math.Abs(replace.Body.RawMeasurement - _state.SonarUltrasonicState.RawMeasurement) > 5)
            {
        
                _state.SonarUltrasonicState = replace.Body;
                CheckStateRobocio();       
     
            }
        }
        /// <summary>
        /// Sonar notification handler
        /// </summary>
        private void SonarReplaceHandler(sonar.Replace replace)
        {
            //replace.Body.DistanceMeasurements
            if (Math.Abs(replace.Body.DistanceMeasurement - _state.SonarState.DistanceMeasurement) > 5)
            {
                _state.SonarState = replace.Body;

            }
        }

        /// <summary>
        /// Compass notification handler
        /// </summary>
        private void CompassReplaceHandler(analog.Replace replace)
        {
            if (Math.Abs(replace.Body.RawMeasurement - _state.CompassState.RawMeasurement) > 5)
            {
                _state.CompassState = replace.Body;
      
            }
        }

        /// <summary>
        /// Light notification handler
        /// </summary>
        private void LightReplaceHandler(analog.Replace replace)
        {
            if (Math.Abs(replace.Body.RawMeasurement - _state.LightState.RawMeasurement) > 1)
            {
                _state.LightState = replace.Body;
             
            }
        }

        /// <summary>
        /// Touch notification handler
        /// </summary>
        private void TouchUpdateHandler(contact.Update update)
        {
            if (_state.TouchState.Pressed != update.Body.Pressed)
            {
                _state.TouchState = update.Body;
             
            }
        }

        #endregion


        /// <summary>
        /// Handles Subscribe messages
        /// </summary>
        /// <param name="subscribe">the subscribe request</param>
        [ServiceHandler]
        public void SubscribeHandler(Subscribe subscribe)
        {
            SubscribeHelper(_submgrPort, subscribe.Body, subscribe.ResponsePort);
        }
    }
}


