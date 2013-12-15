using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using analog = Microsoft.Robotics.Services.AnalogSensor.Proxy;
using contact = Microsoft.Robotics.Services.ContactSensor.Proxy;
using sonar = Microsoft.Robotics.Services.Sonar.Proxy;
namespace RobcioDSS
{
    /// <summary>
    /// RobcioDSS contract class
    /// </summary>
    public sealed class Contract
    {
        /// <summary>
        /// DSS contract identifer for RobcioDSS
        /// </summary>
        [DataMember]
        public const string Identifier = "http://robcio.dss.bylica.com/2013/11/robciodss.html";
    }

    /// <summary>
    /// RobcioDSS state
    /// </summary>
    [DataContract]
    public class RobcioDSSState
    {
        private LogicalState _state;

        private analog.AnalogSensorState _sonarUltrasonicState;

        private analog.AnalogSensorState _copassState;

        private analog.AnalogSensorState _lightState;

        private contact.ContactSensor _touchState;

        private sonar.SonarState _sonarState;



        public RobcioDSSState()
        {
            _state = LogicalState.Start;
        }

        /// <summary>
        /// Robot logical state
        /// </summary>
        [DataMember]
        public LogicalState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// Last retreived Sonar state
        /// </summary>
        [DataMember]
        public sonar.SonarState SonarState
        {
            get { return _sonarState; }
            set { _sonarState = value; }
        }


        /// <summary>
        /// Last retreived SonarUltrasonic state
        /// </summary>
        [DataMember]
        public analog.AnalogSensorState SonarUltrasonicState
        {
            get { return _sonarUltrasonicState; }
            set { _sonarUltrasonicState = value; }
        }

        /// <summary>
        /// Last retreived light state
        /// </summary>
        [DataMember]
        public analog.AnalogSensorState LightState
        {
            get { return _lightState; }
            set { _lightState = value; }
        }

        /// <summary>
        /// Compass state
        /// </summary>
        [DataMember]
        public analog.AnalogSensorState CompassState
        {
            get { return _copassState; }
            set { _copassState = value; }
        }

        /// <summary>
        /// Last retreived touch state
        /// </summary>
        [DataMember]
        public contact.ContactSensor TouchState
        {
            get { return _touchState; }
            set { _touchState = value; }
        }
    }
    /// <summary>
    /// Logical state for the robot
    /// </summary>
    [DataContract]
    public enum LogicalState
    {
        /// <summary>
        /// Initial state
        /// </summary>
        Start,
        /// <summary>
        /// The robot moves forwards
        /// </summary>
        Forward,
        /// <summary>
        /// The robot moves Back
        /// </summary>
        Back,
        /// <summary>
        /// The robot moves Stop
        /// </summary>
        Stop,
        /// <summary>
        /// The robot moves Stop
        /// </summary>
        Left,
        /// <summary>
        /// The robot moves Stop
        /// </summary>
        Right,
        /// <summary>
        /// The robot moves OpenClaw
        /// </summary>
        OpenClaw,
        /// <summary>
        /// The robot moves CloseClaw
        /// </summary>
        CloseClaw,
        /// <summary>
        /// The robot searches the black line
        /// </summary>
        Seek,
        /// <summary>
        /// The robot launches the ball
        /// </summary>
        Launch,
        /// <summary>
        /// The robot Clear
        /// </summary>        
        ClearAllTask,

        /// <summary>
        /// The robot stops
        /// </summary>
        FinalStop,
        /// <summary>
        /// The StateRobotChange stops
        /// </summary>
        StateRobotChange
    }

    /// <summary>
    /// RobcioDSS main operations port
    /// </summary>
    [ServicePort]
    public class RobcioDSSOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get, Subscribe>
    {
    }

    /// <summary>
    /// RobcioDSS get operation
    /// </summary>
    public class Get : Get<GetRequestType, PortSet<RobcioDSSState, Fault>>
    {
        /// <summary>
        /// Creates a new instance of Get
        /// </summary>
        public Get()
        {
        }

        /// <summary>
        /// Creates a new instance of Get
        /// </summary>
        /// <param name="body">the request message body</param>
        public Get(GetRequestType body)
            : base(body)
        {
        }

        /// <summary>
        /// Creates a new instance of Get
        /// </summary>
        /// <param name="body">the request message body</param>
        /// <param name="responsePort">the response port for the request</param>
        public Get(GetRequestType body, PortSet<RobcioDSSState, Fault> responsePort)
            : base(body, responsePort)
        {
        }
    }


    /// <summary>
    /// RobcioDSS subscribe operation
    /// </summary>
    public class Subscribe : Subscribe<SubscribeRequestType, PortSet<SubscribeResponseType, Fault>>
    {
        /// <summary>
        /// Creates a new instance of Subscribe
        /// </summary>
        public Subscribe()
        {
        }

        /// <summary>
        /// Creates a new instance of Subscribe
        /// </summary>
        /// <param name="body">the request message body</param>
        public Subscribe(SubscribeRequestType body)
            : base(body)
        {
        }

        /// <summary>
        /// Creates a new instance of Subscribe
        /// </summary>
        /// <param name="body">the request message body</param>
        /// <param name="responsePort">the response port for the request</param>
        public Subscribe(SubscribeRequestType body, PortSet<SubscribeResponseType, Fault> responsePort)
            : base(body, responsePort)
        {
        }
    }
}


