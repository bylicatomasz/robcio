using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using RobcioDSS.Action;
using Microsoft.Ccr.Core;

namespace RobcioDSS
{
    public partial class BasicFormTest : Form
    {
        private Port<ActionToExecute> portTaskRobcio;
        private Port<ActionToExecute> portTaskRobcioHighPriority;

        public BasicFormTest(Port<ActionToExecute> _portTaskRobcio, Port<ActionToExecute> _portTaskRobcioHighPriority)
        {
   
            this.portTaskRobcio = _portTaskRobcio;
            this.portTaskRobcioHighPriority = _portTaskRobcioHighPriority;
            InitializeComponent();
        }

        public BasicFormTest()
        {
            InitializeComponent();
        }

        private void ClawOpenButton_Click(object sender, EventArgs e)
        {
            ActionToExecute actionOpenClaw = new ActionToExecute();
            actionOpenClaw.State = LogicalState.OpenClaw;
            portTaskRobcio.Post(actionOpenClaw);
        }

        private void ClawCloseButton_Click(object sender, EventArgs e)
        {
            ActionToExecute actionCloseClaw = new ActionToExecute();
            actionCloseClaw.State = LogicalState.CloseClaw;
            portTaskRobcio.Post(actionCloseClaw);
        }

        private void ClawBrawoButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                ActionToExecute actionOpenClaw = new ActionToExecute();
                actionOpenClaw.State = LogicalState.OpenClaw;
                portTaskRobcio.Post(actionOpenClaw);
                ActionToExecute actionCloseClaw = new ActionToExecute();
                actionCloseClaw.State = LogicalState.CloseClaw;
                portTaskRobcio.Post(actionCloseClaw);
                

            }

        }

        private void ClearTaskButton_Click(object sender, EventArgs e)
        {
            
            ActionToExecute actionClear = new ActionToExecute();
            actionClear.State = LogicalState.ClearAllTask;
            portTaskRobcioHighPriority.Post(actionClear);

        }
    }
}
