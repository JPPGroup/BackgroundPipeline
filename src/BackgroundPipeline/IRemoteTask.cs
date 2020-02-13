using System;
using System.Collections.Generic;

namespace Jpp.BackgroundPipeline
{
    public interface IRemoteTask
    {
        Guid Id { get; set; }

        Runtime WorkerRuntime { get; set; }

        ResponseStatus? ResponseStatus { get; set; }

        List<File> WorkingDirectory { get; set; }
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
