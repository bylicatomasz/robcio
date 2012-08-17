using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Ccr.Core;

using motor = Microsoft.Robotics.Services.Motor.Proxy;
using drive = Microsoft.Robotics.Services.Drive.Proxy;
using Microsoft.Dss.ServiceModel.Dssp;


namespace com.bylica.robcio
{
    class MotoDrives
    {


        private RobcioService robcioService;
        private drive.DriveOperations _drivePort;
          
        private Random _randomGen = new Random();

        public MotoDrives(RobcioService rs,drive.DriveOperations dp)
        {
    

            this.robcioService=rs;
            this._drivePort=dp;
    
        }

      

        /// <summary>
        /// Implements a simple sequential state machine that makes the robot wander
        /// </summary>
        /// <param name="polarity">Use 1 for going forward, -1 for going backwards</param>
        /// <returns></returns>
        IEnumerator<ITask> BackUpTurnAndMove(double polarity)
        {
            // First backup a little.
            yield return Arbiter.Receive(false,
                StartMove(0.4 * polarity),
                delegate(bool result) { });

            // wait
            yield return Arbiter.Receive(false, robcioService.getTimeoutPort(1500), delegate(DateTime t) { });

            // now Turn
            yield return Arbiter.Receive(false,
                StartTurn(),
                delegate(bool result) { });

            // wait
            yield return Arbiter.Receive(false, robcioService.getTimeoutPort(1500), delegate(DateTime t) { });

            // now reverse direction and keep moving straight
            yield return Arbiter.Receive(false,
                StartMove(_randomGen.NextDouble() * polarity),
                delegate(bool result) { });


            // wait
            yield return Arbiter.Receive(false, robcioService.getTimeoutPort(1500), delegate(DateTime t) { });


            // now reverse direction and keep moving straight
            yield return Arbiter.Receive(false,
                StopMove(),
                delegate(bool result) { });



            // done
            yield break;
        }

        Port<bool> StartTurn()
        {
            Port<bool> result = new Port<bool>();
            // start a turn
            robcioService.getSpawnIterator<Port<bool>>(result, RandomTurn);
            
            return result;
        }

        Port<bool> StartMove(double powerLevel)
        {
            Port<bool> result = new Port<bool>();
            // start movement
            robcioService.getSpawnIterator<Port<bool>, double>(result, powerLevel, MoveStraight);
            return result;
        }

        Port<bool> StopMove()
        {
            Port<bool> result = new Port<bool>();
            // start movement
            robcioService.getSpawnIterator<Port<bool>>(result,  StopStraight);
            return result;
        }

        IEnumerator<ITask> RandomTurn(Port<bool> done)
        {
            // we turn by issuing motor commands, using reverse polarity for left and right
            // We could just issue a Rotate command but since its a higher level function
            // we cant assume (yet) all our implementations of differential drives support it
            double randomPower = _randomGen.NextDouble();
            drive.SetDrivePowerRequest setPower = new drive.SetDrivePowerRequest(randomPower, -randomPower);

            bool success = false;
            yield return
                Arbiter.Choice(
                    _drivePort.SetDrivePower(setPower),
                    delegate(DefaultUpdateResponseType rsp) { success = true; },
                    delegate(W3C.Soap.Fault failure)
                    {
                        // report error but report done anyway. we will attempt
                        // to do the next step in wander behavior even if turn failed
                        robcioService.writeToLogError("Failed setting drive power");
                    });

            done.Post(success);
            yield break;
        }

        IEnumerator<ITask> MoveStraight(Port<bool> done, double powerLevel)
        {
            drive.SetDrivePowerRequest setPower = new drive.SetDrivePowerRequest(powerLevel, powerLevel);

            yield return
                Arbiter.Choice(
                _drivePort.SetDrivePower(setPower),
                delegate(DefaultUpdateResponseType success) { done.Post(true); },
                delegate(W3C.Soap.Fault failure)
                {
                    // report error but report done anyway. we will attempt
                    // to do the next step in wander behavior even if turn failed
                    robcioService.writeToLogError("Failed setting drive power");
                    done.Post(false);
                });
        }

        IEnumerator<ITask> StopStraight(Port<bool> done)
        {
            
            drive.SetDrivePowerRequest setPower = new drive.SetDrivePowerRequest(0,0);

            yield return
                Arbiter.Choice(
                _drivePort.SetDrivePower(setPower),
                delegate(DefaultUpdateResponseType success) { done.Post(true); },
                delegate(W3C.Soap.Fault failure)
                {
                    // report error but report done anyway. we will attempt
                    // to do the next step in wander behavior even if turn failed
                    robcioService.writeToLogError("Failed setting drive power");
                    done.Post(false);
                });
        }


        public void test() {

            robcioService.getSpawnIterator<double>(-1, BackUpTurnAndMove);
            return;

        
        }



    }
}
