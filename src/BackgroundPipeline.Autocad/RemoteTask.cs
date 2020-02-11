using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundPipeline.Autocad
{
    public class RemoteTask
    {
        public Guid Id { get; set; }
        public Runtime WorkerRuntime { get; set; }

        public List<File> InputFiles { get; set; }
        public List<File> OutputFiles { get; set; }

        public ResponseStatus? ResponseStatus { get; set; } 
    }

    public enum Runtime
    {
        Autocad,
        Civil3d
    }

    public struct File
    {
        public string Name { get; set; }
        public byte Data { get; set; }
    }

    public enum ResponseStatus
    {
        OK,
        UnknownTask,
        UnkownFailure
    }
}
