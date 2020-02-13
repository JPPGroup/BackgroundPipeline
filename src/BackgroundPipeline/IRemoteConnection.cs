using System;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    public interface IRemoteConnection
    {
        void SendMessage(IRemoteTask remoteTask);

        Task<IRemoteTask> GetResponseAsync(Guid responseId);
    }
}
