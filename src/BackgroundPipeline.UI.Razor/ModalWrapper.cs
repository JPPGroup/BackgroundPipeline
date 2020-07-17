using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jpp.Common.Razor.Services;

namespace Jpp.BackgroundPipeline.UI.Razor
{
    public class ModalWrapper : ModalService, IModal
    {
        //TODO: Check this
        public async Task<bool> ShowAsync(string title, Type contentType, params KeyValuePair<string, object>[] attributes)
        {
            ModalResult result = await base.ShowAsync(title, contentType, attributes);
            return result.Success;
        }
    }
}
