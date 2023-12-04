using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMultiStash.Global
{
    using Configuration;
    namespace Configuration
    {

    }

    partial class ConfigurationManager
    {
        public EventHandler<EventArgs> LanguageChanged;
        public EventHandler<EventArgs> GamePathChanged;
        public EventHandler<EventArgs> OverlayDesignChanged;

        public void InvokeLanguageChanged()
            => SaveInvoke(() => LanguageChanged?.Invoke(this, EventArgs.Empty));

        public void InvokeGamePathChanged()
            => SaveInvoke(() => GamePathChanged?.Invoke(this, EventArgs.Empty));

        public void InvokeOverlayDesignChanged()
            => SaveInvoke(() => OverlayDesignChanged?.Invoke(this, EventArgs.Empty));

    }
}
