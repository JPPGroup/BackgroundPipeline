using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Jpp.BackgroundPipeline;
using Jpp.Ironstone.Draughter.TaskPayloads;
using Newtonsoft.Json;

namespace BackgroundPipeline.Autocad
{
    public class RemoteTask : IRemoteTask
    {
        public Guid Id { get; set; }
        public Runtime WorkerRuntime { get; set; }

        public bool AutoDeserialize { get; set; } = true;

        [JsonIgnore]
        public List<ITaskPayload> TaskPayload
        {
            get { return _taskPayload;}
            set
            {
                _taskPayload = value;
                _serializedTaskPayload = JsonConvert.SerializeObject(value, _settings);
            }
        }

        public string SerializedTaskPayload {
            get
            {
                //Need to serialize every time in case collection has modified members. Not too intensive as this field is rarely used
                if(_taskPayload != null)
                  _serializedTaskPayload = JsonConvert.SerializeObject(_taskPayload, _settings);
                
                return _serializedTaskPayload;
            }
            set
            {
                _serializedTaskPayload = value;
                if(AutoDeserialize)
                    Deserialize();
            }
        }

        private List<ITaskPayload> _taskPayload;
        private string _serializedTaskPayload;

        JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

        public List<File> WorkingDirectory { get; set; }

        public ResponseStatus? ResponseStatus { get; set; }

        public RemoteTask()
        {
            Id = Guid.NewGuid();
        }

        public void Deserialize()
        {
            _taskPayload = JsonConvert.DeserializeObject<List<ITaskPayload>>(_serializedTaskPayload, _settings);
        }
    }
}
