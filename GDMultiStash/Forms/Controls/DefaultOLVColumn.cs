
using BrightIdeasSoftware;
using System.ComponentModel;

namespace GDMultiStash.Forms.Controls
{
    [DesignerCategory("code")]
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
