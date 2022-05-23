using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace GDMultiStash.Forms.Controls
{
    [ToolboxItem(true)]
    public class ExTrackBar : TrackBar
    {

        /*

        public new event EventHandler Scroll;

        public ExTrackBar() : base()
        {
            base.Scroll += new EventHandler(Base_Scroll);
        }

        private bool _blockRecursion = false;

        private void Base_Scroll(object sender, EventArgs e)
        {
            if (_blockRecursion) return;

            int trackValue = Value;
            int smallChangeValue = TickFrequency;
            if (trackValue % smallChangeValue != 0)
            {
                trackValue = (trackValue / smallChangeValue) * smallChangeValue;

                _blockRecursion = true;
                Value = trackValue;
                _blockRecursion = false;
            }

            Scroll?.Invoke(this, e);
        }
        */
    }
}
