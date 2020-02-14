using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Jpp.BackgroundPipeline
{
    public class Artifacts : IEnumerable<string>
    {
        private Dictionary<string, byte[]> _artifacts;
        private IArtifactPersistence _persistence;
        private Guid _pipeId;

        public IEnumerator<string> GetEnumerator()
        {
            return _artifacts.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _artifacts.Keys.GetEnumerator();
        }

        public byte[] this[string s]
        {
            get { return _artifacts[s]; }
            set { _artifacts[s] = value; }
        }

        public Artifacts(Guid pipeId, IArtifactPersistence persistence)
        {
            _persistence = persistence;
            _pipeId = pipeId;

            _artifacts = new Dictionary<string, byte[]>();

            foreach (string persistedPath in _persistence.GetPersistedPaths(_pipeId))
            {
                _artifacts.Add(persistedPath, _persistence.GetPersistedData(_pipeId, persistedPath));
            }
        }

        public List<File> GetFiles()
        {
            List<File> result = new List<File>();

            foreach (string artifactsKey in _artifacts.Keys)
            {
                File f = new File()
                {
                    Name = artifactsKey,
                    Data = _artifacts[artifactsKey]
                };
            }

            return result;
        }

        public void SetFiles(List<File> files)
        {
            foreach (File file in files)
            {
                if (_artifacts.ContainsKey(file.Name))
                {
                    _artifacts[file.Name] = file.Data;
                }
                else
                {
                    _artifacts.Add(file.Name, file.Data);
                }
            }
        }

        public void AddFile(string filename, byte[] data)
        {
            _artifacts.Add(filename, data);
        }
    }
}
