using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline.Stages
{
    public class FileUploadStage : ConfirmationStage
    {
        public const string FILE_OUTPUT = "UploadedFile";
        public const string FILENAME = "UploadedFilename";

        [NotMapped]
        public byte[] Data
        {
            get { return Output[FILE_OUTPUT] as byte[]; }
            set 
            {
                if (Output.ContainsKey(FILE_OUTPUT))
                {
                    Output[FILE_OUTPUT] = value;
                }
                else
                {
                    Output.Add(FILE_OUTPUT, value);
                }
                
            }
        }

        [NotMapped]
        public string Filename
        {
            get { return Output[FILENAME] as string; }
            set 
            {
                if (Output.ContainsKey(FILENAME))
                {
                    Output[FILENAME] = value;
                }
                else
                {
                    Output.Add(FILENAME, value);
                }
            }
        }
    }
}
