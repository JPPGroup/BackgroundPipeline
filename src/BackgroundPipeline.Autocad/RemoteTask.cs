using System;
using System.Collections.Generic;
using System.Text;
using Jpp.Ironstone.Draughter.TaskPayloads;

namespace BackgroundPipeline.Autocad
{
    public class RemoteTask
    {
        public Guid Id { get; set; }
        public Runtime WorkerRuntime { get; set; }

        public List<ITaskPayload> TaskPayload { get; set; }

        public List<File> WorkingDirectory { get; set; }

        public ResponseStatus? ResponseStatus { get; set; } 
    }

    public enum Runtime
    {
        Autocad,
        Civil3d
    }

    public enum ResponseStatus
    {
        OK,
        UnknownTask,
        UnkownFailure
    }
}
