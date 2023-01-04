
using BrightIdeasSoftware;

namespace GDMultiStash.Forms.Controls
{
    internal class DefaultOLVColumn : OLVColumn
    {
        public DefaultOLVColumn() : base()
        {
            Searchable = false;
            Groupable = false;
            Sortable = false;
            IsEditable = false;
            CheckBoxes = false;
        }
        public new int Width
        {
            get => base.Width;
            set
            {
                base.Width = value;
                base.MaximumWidth = value;
                base.MinimumWidth = value;
            }
        }
    }
}
