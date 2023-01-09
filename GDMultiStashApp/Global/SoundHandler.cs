using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace GDMultiStash.GlobalHandlers
{
    internal class SoundHandler
    {

        // this sound class is just experimental and not implemented yet

        private readonly SoundPlayer ButtonOverTick;

        public SoundHandler()
        {
            ButtonOverTick = new SoundPlayer(Properties.Resources.soundButtonOverTick);
        }

        public void PlayButtonOverTick()
        {
            ButtonOverTick.Play();
        }


    }
}
