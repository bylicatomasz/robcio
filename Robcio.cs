using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

using Microsoft.Dss.Core;

using bumper = Microsoft.Robotics.Services.ContactSensor.Proxy;
using drive = Microsoft.Robotics.Services.Drive.Proxy;
using motor = Microsoft.Robotics.Services.Motor.Proxy;


namespace com.bylica.robcio
{
	[Contract(Contract.Identifier)]
	[DisplayName("Robcio")]
	[Description("Robcio service (no description provided)")]
	class RobcioService : DsspServiceBase
	{
		[ServiceState]
		RobcioState _state = new RobcioState();
		
		[ServicePort("/Robcio", AllowMultipleInstances = true)]
		RobcioOperations _mainPort = new RobcioOperations();

        #region DriveRegion

        [Partner("Drive",
        Contract = drive.Contract.Identifier,
        CreationPolicy = PartnerCreationPolicy.UseExisting)]
        drive.DriveOperations _drivePort = new drive.DriveOperations();

        #endregion

        #region BumperRegion


        [Partner("bumper", Contract = bumper.Contract.Identifier,
        CreationPolicy = PartnerCreationPolicy.UseExisting)]
        private bumper.ContactSensorArrayOperations _bumperPort = new bumper.ContactSensorArrayOperations();


        #endregion




        public RobcioService(DsspServiceCreationPort creationPort)
            : base(creationPort)
        {
        }

      


        public void writeToLogInfo(String msg) {
            LogInfo(LogGroups.Console, msg);
        }
        public void writeToLogError(String msg)
        {
            LogError(LogGroups.Console, msg);
        }

        public Port<DateTime> getTimeoutPort(int time) {

            
            return TimeoutPort(time);
        }



        public void getSpawnIterator<T0>(T0 t0, IteratorHandler<T0> handler) {



            SpawnIterator<T0>(t0, handler);

        }

        public void getSpawnIterator<T0, T1>(T0 t0, T1 t1, IteratorHandler<T0, T1> handler) {

            SpawnIterator<T0,T1>(t0,t1, handler);
        }


        protected override void Start()
		{						
			base.Start();
            new BumperSensor().initBumper(this,_bumperPort);
            new MotoDrives(this, _drivePort).test();
		}
	}
}


