namespace Jpp.BackgroundPipeline.Stages
{
    public class FileUploadStage : ConfirmationStage
    {
        public FileUploadStage(Pipeline parent, string name) : base(parent, name)
        { }

        public void AddFile(string filename, byte[] data)
        {
            Pipeline.Artifacts.AddFile(filename, data);
        }
    }
}
