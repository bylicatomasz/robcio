using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

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
		
		public RobcioService(DsspServiceCreationPort creationPort)
			: base(creationPort)
		{
		}
		
		protected override void Start()
		{
			
			// 
			// Add service specific initialization here
			// 
			
			base.Start();
		}
	}
}


