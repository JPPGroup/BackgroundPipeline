using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline
{
    public interface IModal
    {
        Task<bool> ShowAsync(string title, Type contentType, params KeyValuePair<string, object>[] attributes);
    }
}
