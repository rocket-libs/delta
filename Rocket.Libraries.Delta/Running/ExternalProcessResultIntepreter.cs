using System;
using System.Linq;
using delta.ProcessRunning;

namespace Rocket.Libraries.Delta.Running
{
    public interface IExternalProcessResultIntepreter
    {
        bool IsEqualTo(object value, ProcessRunningResults processRunningResults);
    }

    public class ExternalProcessResultIntepreter : IExternalProcessResultIntepreter
    {
        public bool IsEqualTo(object value, ProcessRunningResults processRunningResults)
        {
            var stringValue = value == null ? string.Empty : value.ToString();
            if (processRunningResults == null || processRunningResults.Output == null || processRunningResults.Output.Length == 0)
            {
                return stringValue == string.Empty;
            }
            else
            {
                return processRunningResults.Output.FirstOrDefault().Equals(stringValue, StringComparison.InvariantCulture);
            }
        }
    }
}