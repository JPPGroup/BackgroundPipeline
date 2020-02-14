using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.BackgroundPipeline
{
    public interface IArtifactPersistence
    {
        void Persist(Guid pipeId, string path, byte[] data);

        IEnumerable<string> GetPersistedPaths(Guid pipeId);

        byte[] GetPersistedData(Guid pipeId, string path);
    }
}
