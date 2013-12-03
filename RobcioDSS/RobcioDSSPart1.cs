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

namespace RobcioDSS
{

    public partial class RobcioDSSService : DsspServiceBase
    {



        #region Robcio_VaribleTaskQuota

        public List<ActionTask> actionToDo = new List<ActionTask>();


        private PortSet<ActionTask, ActionTaskHighPriority, ActionTaskUpdateStatus> portSetTaskRobcio = new PortSet<ActionTask, ActionTaskHighPriority, ActionTaskUpdateStatus>();

        public Dispatcher dispatcherPort;

        public DispatcherQueue taskQueue;

        private List<ITask> list = new List<ITask>();

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
                        Arbiter.ReceiveWithIteratorFromPortSet<ActionTaskUpdateStatus>(true, portSetTaskRobcio, ExecuteActionUpdateStatus)
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
            return null;

        }
        public IEnumerator<ITask> ExecuteActionUpdateStatus(ActionTaskUpdateStatus action)
        {
            return StateChange(action.State);
        }


        #endregion


        #region Robcio_MainStateMethod

        /// <summary>
        /// This function is called on each state change
        /// </summary>
        private IEnumerator<ITask> StateChange(LogicalState newState)
        {

            _state.State = newState;


            yield break;
        }


        #endregion

        #endregion




    }
}


