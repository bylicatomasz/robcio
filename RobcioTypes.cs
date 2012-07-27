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
	public sealed class Contract
	{
		[DataMember]
		public const string Identifier = "http://robcio.bylica.com/2012/07/robcio.html";
	}
	
	[DataContract]
	public class RobcioState
	{
	}
	
	[ServicePort]
	public class RobcioOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get>
	{
	}
	
	public class Get : Get<GetRequestType, PortSet<RobcioState, Fault>>
	{
		public Get()
		{
		}
		
		public Get(GetRequestType body)
			: base(body)
		{
		}
		
		public Get(GetRequestType body, PortSet<RobcioState, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}
}


