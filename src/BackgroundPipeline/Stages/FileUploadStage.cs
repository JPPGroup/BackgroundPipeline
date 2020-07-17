namespace Jpp.BackgroundPipeline.Stages
{
    public class FileUploadStage : ConfirmationStage
    {
        public string RenamedFilename { get; set; }

        public FileUploadStage(Pipeline parent, string name) : base(parent, name)
        { }

        public void AddFile(string filename, byte[] data)
        {
            if (!string.IsNullOrEmpty(RenamedFilename))
            {
                filename = RenamedFilename;
            }
            Pipeline.Artifacts.AddFile(filename, data);
        }
    }
}
