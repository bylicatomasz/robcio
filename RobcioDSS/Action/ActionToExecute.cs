using System;
using System.Collections.Generic;
using System.Text;

namespace RobcioDSS.Action
{
    public class ActionToExecute
    {
        private LogicalState state;

        public LogicalState State
        {
            get { return state; }
            set { state = value; }
        }

    }
}
