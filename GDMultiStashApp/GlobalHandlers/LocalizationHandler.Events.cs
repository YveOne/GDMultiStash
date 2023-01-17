using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.GlobalHandlers
{
    partial class LocalizationHandler
    {
        
        public EventHandler<EventArgs> LanguageLoaded;

        public void InvokeLanguageLoaded()
            => SaveInvoke(() => LanguageLoaded?.Invoke(null, EventArgs.Empty));

    }
}
