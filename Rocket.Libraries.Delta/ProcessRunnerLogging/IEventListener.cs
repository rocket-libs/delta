using System.Threading.Tasks;

namespace Rocket.Libraries.Delta.ProcessRunnerLogging
{
    public interface IEventListener
    {
        Task OnEventAsync(string message);
    }
}