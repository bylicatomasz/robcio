﻿using System;
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
        PortSet<ActionTask, ActionTaskHighPriority, ActionTaskUpdateStatus> portSetTaskRobcio;


        public BasicFormTest(PortSet<ActionTask, ActionTaskHighPriority, ActionTaskUpdateStatus> _portSetTaskRobcio)
        {

            this.portSetTaskRobcio = _portSetTaskRobcio;

            InitializeComponent();
        }

        public BasicFormTest()
        {
            InitializeComponent();
        }

        private void ClawOpenButton_Click(object sender, EventArgs e)
        {
            ActionTask actionOpenClaw = new ActionTask();
            actionOpenClaw.State = LogicalState.OpenClaw;
            portSetTaskRobcio.Post(actionOpenClaw);
        }

        private void ClawCloseButton_Click(object sender, EventArgs e)
        {
            ActionTask actionCloseClaw = new ActionTask();
            actionCloseClaw.State = LogicalState.CloseClaw;
            portSetTaskRobcio.Post(actionCloseClaw);
        }

        private void ClawBrawoButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                ActionTask actionOpenClaw = new ActionTask();
                actionOpenClaw.State = LogicalState.OpenClaw;
                portSetTaskRobcio.Post(actionOpenClaw);
                ActionTask actionCloseClaw = new ActionTask();
                actionCloseClaw.State = LogicalState.CloseClaw;
                portSetTaskRobcio.Post(actionCloseClaw);
                

            }

        }

        private void ClearTaskButton_Click(object sender, EventArgs e)
        {

            ActionTaskHighPriority actionClear = new ActionTaskHighPriority();
            actionClear.State = LogicalState.ClearAllTask;
            portSetTaskRobcio.Post(actionClear);

        }
    }
}
