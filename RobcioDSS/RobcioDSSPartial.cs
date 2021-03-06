using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Ccr.Adapters.WinForms;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using submgr = Microsoft.Dss.Services.SubscriptionManager;

using drive = Microsoft.Robotics.Services.Drive.Proxy;
using motor = Microsoft.Robotics.Services.Motor.Proxy;
using analog = Microsoft.Robotics.Services.AnalogSensor.Proxy;
using contact = Microsoft.Robotics.Services.ContactSensor.Proxy;
using RobcioDSS.Action;
using Microsoft.Dss.Core;
using System.Text;
using RobcioDSS.Network;

namespace RobcioDSS
{

    public partial class RobcioDSSService : DsspServiceBase
    {



        #region Robcio_VaribleTaskQuota

        public List<ActionTask> actionToDo = new List<ActionTask>();


        private PortSet<ActionTask, ActionTaskHighPriority, ActionTaskCheckStatus> portSetTaskRobcio = new PortSet<ActionTask, ActionTaskHighPriority, ActionTaskCheckStatus>();

        public Dispatcher dispatcherPort;

        public DispatcherQueue taskQueue;

        private List<ITask> list = new List<ITask>();

        private Boolean startRecording = false;

        #endregion


        #region Robcio_MindInitialize

        /// <summary>
        /// Gets initial MindRobcio informations
        /// </summary>
        private IEnumerator<ITask> InitializeMindRobcio()
        {

            yield return Arbiter.FromIteratorHandler(InitializePortTask);

            yield return Arbiter.FromIteratorHandler(CreateForm);
            yield break;
        }


        private IEnumerator<ITask> InitializePortTask()
        {

            dispatcherPort = new Dispatcher(
                  0, // zero means use one thread per CPU, or 2 if only one CPU present
                  "RobcioDispatcher" // friendly name assgined to OS threads
                  );

            taskQueue = new DispatcherQueue(
                   "RobcioQueue", // friendly name
                   dispatcherPort // dispatcher instance
               );


            Arbiter.Activate(taskQueue,


                   Arbiter.Interleave(
                       new TeardownReceiverGroup(
                            Arbiter.ReceiveWithIteratorFromPortSet<ActionTaskHighPriority>(false, portSetTaskRobcio, ExecuteActionHighPriority)
                           ),
                       new ExclusiveReceiverGroup
                       (
                        Arbiter.ReceiveWithIteratorFromPortSet<ActionTask>(true, portSetTaskRobcio, ExecuteAction)
                       ),
                       new ConcurrentReceiverGroup(
                        Arbiter.ReceiveWithIteratorFromPortSet<ActionTaskCheckStatus>(true, portSetTaskRobcio, ExecuteActionTaskCheckStatus)
                           )));




            yield break;
        }

        private IEnumerator<ITask> CreateForm()
        {
            WinFormsServicePort.Post(new RunForm(StartForm));
            yield break;

        }
        private System.Windows.Forms.Form StartForm()
        {
            BasicFormTest form = new BasicFormTest(portSetTaskRobcio);
            return form;
        }


        #endregion

        #region Robcio_MainMethod


        #region Robcio_ExecuteActio



        public IEnumerator<ITask> ExecuteActionHighPriority(ActionTaskHighPriority action)
        {

            if (action.State.Equals(LogicalState.ClearAllTask))
            {

                portSetTaskRobcio.P0.Clear();
                portSetTaskRobcio.P2.Clear();

                taskQueue.Suspend();
                taskQueue.Dispose();
                return InitializePortTask();
            }

            return null;

        }


        public IEnumerator<ITask> ExecuteAction(ActionTask action)
        {
            if (!action.State.Equals(LogicalState.StartRecording) || !action.State.Equals(LogicalState.StopRecording))
            {
                PutNewLogicalState(action.State);
            }
            
            if (action.State.Equals(LogicalState.OpenClaw))
            {
                LogInfo(LogGroups.Console, "Int is: Open");
                return OpenClaw();

            }
            else if (action.State.Equals(LogicalState.CloseClaw))
            {
                LogInfo(LogGroups.Console, "Int is: Close");
                return CloseClaw();
            }
            else if (action.State.Equals(LogicalState.Forward))
            {
                LogInfo(LogGroups.Console, "Int is: Forward");
                return Forward();
            }
            else if (action.State.Equals(LogicalState.Back))
            {
                LogInfo(LogGroups.Console, "Int is: Back");
                return Back();
            }
            else if (action.State.Equals(LogicalState.Left))
            {
                LogInfo(LogGroups.Console, "Int is: Left");
                return Left();
            }
            else if (action.State.Equals(LogicalState.Right))
            {
                LogInfo(LogGroups.Console, "Int is: Right");
                return Right();
            }
            else if (action.State.Equals(LogicalState.Stop))
            {
                LogInfo(LogGroups.Console, "Int is: Right");
                return Halt();
            }
            else if (action.State.Equals(LogicalState.StopRecording))
            {
                LogInfo(LogGroups.Console, "Int is: StopRecord");
                startRecording = false;                
            }
            else if (action.State.Equals(LogicalState.StartRecording))
            {
                LogInfo(LogGroups.Console, "Int is: StartRecord");
                startRecording = true;
            }
            return null;

        }
        public IEnumerator<ITask> ExecuteActionTaskCheckStatus(ActionTaskCheckStatus check)
        {
            addToLogString();
            if (_state.State.Equals(LogicalState.Forward) && _state.SonarState.DistanceMeasurement < 0.30)
            {
                LogInfo(LogGroups.Console, "To close obstruction. We must stop.");
                ActionTask actionStop = new ActionTask();
                actionStop.State = LogicalState.Stop;
                portSetTaskRobcio.Post(actionStop);
            }
            yield break;

        }

        void addToLogString()
        {

            if (startRecording==true && _state != null && _state.CompassState != null && _state.CompassState.NormalizedMeasurement !=0.0
                && _state.SonarState != null && _state.SonarState.DistanceMeasurements != null
                && _state.SonarUltrasonicState != null && _state.SonarUltrasonicState.RawMeasurement != 0.0
                )
            {
                
                StringBuilder sb = new StringBuilder();

                Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                Int32 unixTimestampSonar = (Int32)(_state.SonarState.TimeStamp.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                Int32 unixTimestampCompass = (Int32)(_state.CompassState.TimeStamp.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                

                sb.Append(unixTimestamp);
                sb.Append(";");
                sb.Append(_state.Dystance);
                sb.Append(";");
                sb.Append(_state.CompassState.NormalizedMeasurement);
                sb.Append(";");
                sb.Append(_state.SonarUltrasonicState.RawMeasurement);
                sb.Append(";");
                sb.Append(_state.State.ToString());
                sb.Append(";");

                sb.Append(unixTimestampSonar);
                sb.Append(";");
                sb.Append(unixTimestampCompass);
                sb.Append(";");

                sb.Append(_state.CompassState.RawMeasurement);
                sb.Append(";");

                sb.Append(_state.SonarState.DistanceMeasurement);
                sb.Append(";");


                foreach (double v in _state.SonarState.DistanceMeasurements)
                {

                    sb.Append(v);
                    sb.Append(";");
                }

                new CallWebService().SendData(sb.ToString());
                System.IO.StreamWriter file = new System.IO.StreamWriter(csvFileName, true);
                file.WriteLine(sb.ToString());
                file.Close();

            }      
        }

        #endregion


        #region Robcio_MainStateMethod


        private void PutNewLogicalState(LogicalState state)
        {
            _state.State = state;
        }
        private void CheckStateRobocio()
        {
            ActionTaskCheckStatus check = new ActionTaskCheckStatus();
            check.DateCheck = new DateTime();
            portSetTaskRobcio.Post(check);
        }



        #endregion

        #endregion




    }
}


