using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;


namespace RobcioDSS.Network
{
    public class CallWebService
    {
        public void SendData(String postData)
        {
            WebRequest request = WebRequest.Create("http://127.0.0.1:5151/ ");
            request.Method = "POST";
            
            byte[] byteArray = Encoding.UTF8.GetBytes("DataRob=" + postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            
            WebResponse response = request.GetResponse();            
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
        }
        
        
    }
}
