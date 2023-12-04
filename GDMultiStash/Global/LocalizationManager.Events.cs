using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Global
{
    using Localization;
    namespace Localization
    {

    }

    partial class LocalizationManager
    {
        
        public EventHandler<EventArgs> LanguageLoaded;

        public void InvokeLanguageLoaded()
            => SaveInvoke(() => LanguageLoaded?.Invoke(null, EventArgs.Empty));

    }
}
