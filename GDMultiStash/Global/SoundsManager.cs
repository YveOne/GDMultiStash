using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace GDMultiStash.Global
{
    using Sounds;
    namespace Sounds
    {

    }

    internal partial class SoundsManager : Base.Manager
    {

        // this sound class is just experimental and not implemented yet

        private readonly SoundPlayer ButtonOverTick;

        public SoundsManager() : base()
        {
            ButtonOverTick = new SoundPlayer(Properties.Resources.soundButtonOverTick);
        }

        public void PlayButtonOverTick()
        {
            ButtonOverTick.Play();
        }


    }
}
