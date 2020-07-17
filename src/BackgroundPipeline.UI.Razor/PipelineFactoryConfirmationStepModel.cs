using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jpp.BackgroundPipeline.UI.Razor
{
    public class PipelineFactoryConfirmationStepModel : IEnumerable<string>
    {
        private List<object> _models;
        private List<string> _settings;

        public PipelineFactoryConfirmationStepModel()
        {
            _models = new List<object>();
            _settings = new List<string>();
        }

        public void Add(object model)
        {
            _models.Add(model);
        }

        public void Parse()
        {
            _settings.Clear();
            foreach (object o in _models)
            {
                var props = o.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DisplayAttribute)));
                foreach (PropertyInfo propertyInfo in props)
                {
                    string propertyValue = propertyInfo.GetValue(o).ToString();
                    object[] attrs = propertyInfo.GetCustomAttributes(true);
                    foreach (object attr in attrs)
                    {
                        if (attr is DisplayAttribute)
                        {
                            string propertyName = (attr as DisplayAttribute).Name;
                            _settings.Add($"{propertyName}: {propertyValue}");
                        }
                    }
                }
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _settings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _settings.GetEnumerator();
        }
    }
}
