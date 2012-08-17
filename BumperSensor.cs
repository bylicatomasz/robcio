using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dss.Core.Attributes;


using bumper = Microsoft.Robotics.Services.ContactSensor.Proxy;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core;
namespace com.bylica.robcio
{
    class BumperSensor
    {



        private RobcioService robcioService;


        public void initBumper(RobcioService rs, bumper.ContactSensorArrayOperations _bumperPort)
        {
            this.robcioService = rs;
            // Create the bumper notification port.
            bumper.ContactSensorArrayOperations bumperNotificationPort = new bumper.ContactSensorArrayOperations();

            // Subscribe to the bumper service, receive notifications on the bumperNotificationPort.
            _bumperPort.Subscribe(bumperNotificationPort);

            // Start listening for updates from the bumper service.
            robcioService.Activate(
                Arbiter.Receive<bumper.Update>
                    (true, bumperNotificationPort, BumperHandler));


        }


        private void BumperHandler(bumper.Update notification)
        {


            if (notification.Body.Pressed)
            {
                robcioService.writeToLogInfo("------------------------------");
                robcioService.writeToLogInfo("Ouch - the bumper was pressed.");
                robcioService.writeToLogInfo("------------------------------");
            }
        }




    }
}
