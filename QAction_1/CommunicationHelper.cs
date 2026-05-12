namespace QAction_1
{
    using System;
    using Skyline.DataMiner.Scripting;

    public static class CommunicationHelper
    {
        public static bool IsSuccessStatusCode(string statusCode)
        {
            return statusCode.Contains("HTTP/1.1 200");
        }

        public static bool ValidateStatusCode(SLProtocol protocol, int statusCodePid, int communicationStatusPid, out string statusCode)
        {
            statusCode = Convert.ToString(protocol.GetParameter(statusCodePid));
            bool isSuccess = statusCode.Contains("HTTP/1.1 200");
            protocol.SetParameter(communicationStatusPid, isSuccess ? 0 : 1);
            return isSuccess;
        }
    }
}
