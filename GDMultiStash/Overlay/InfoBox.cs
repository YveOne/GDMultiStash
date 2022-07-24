using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Overlay;
using GrimDawnLib;

namespace GDMultiStash.Overlay.Elements
{
    public class InfoBox : Element
    {

        public static Font _TitleFont = null;
        public static Font _TextFont = null;

        private readonly TextElement _titleElement;
        private readonly TextElement _lastChangeIntern;

        private readonly InfoBoxButton _saveButton;
        private readonly InfoBoxButton _loadButton;

        public InfoBox()
        {
            X = 2;
            Y = 605;
            Width = 265;
            Height = 127;

            _titleElement = new TextElement()
            {
                Width = Width,
                Height = 25,
                Font = _TitleFont,
                Align = StringAlignment.Near,
                Color = Color.FromArgb(255, 235, 222, 195),
                AnchorPoint = Anchor.TopLeft,
                X = 10,
                Y = 10,
            };
            AddChild(_titleElement);

            _lastChangeIntern = new TextElement()
            {
                Width = Width,
                Height = 20,
                Font = _TextFont,
                Align = StringAlignment.Near,
                Color = Color.FromArgb(255, 235, 222, 195),
                AnchorPoint = Anchor.TopLeft,
                X = 10,
                Y = 10 + 25 + 5,
            };
            AddChild(_lastChangeIntern);

            _saveButton = new InfoBoxButton()
            {
                X = 6,
                Y = -5,
                AnchorPoint = Anchor.BottomLeft,
            };
            AddChild(_saveButton);

            _loadButton = new InfoBoxButton()
            {
                X = 6 + 6 + 120,
                Y = -5,
                AnchorPoint = Anchor.BottomLeft,
            };
            AddChild(_loadButton);

            Core.Runtime.ActiveStashChanged += delegate {
                UpdateInfoText();
            };

            _saveButton.MouseClick += CheatSaveButton_Click;
            _loadButton.MouseClick += CheatLoadButton_Click;

            UpdateButtonText();
            Core.Config.LanguageChanged += delegate {
                UpdateButtonText();
            };

            Core.Runtime.StashReopenStart += delegate {
                Alpha = 0.33f;
            };

            Core.Runtime.StashReopenEnd += delegate {
                Alpha = 1.0f;
            };

        }

        private void UpdateButtonText()
        {
            _saveButton.Text = Core.Localization.GetString("button_save");
            _loadButton.Text = Core.Localization.GetString("button_load");
        }

        private void UpdateInfoText()
        {
            Common.Stash stash = Core.Stashes.GetStash(Core.Runtime.ActiveStashID);
            if (stash == null) return; // something happend
            _titleElement.Text = stash.Name;
            _lastChangeIntern.Text = stash.LastWriteTime.ToString();
        }

        private void CheatSaveButton_Click(object sender, EventArgs e)
        {
            if (Core.Runtime.StashIsReopening) return;
            Core.Runtime.SaveCurrentStash();
        }

        private void CheatLoadButton_Click(object sender, EventArgs e)
        {
            if (Core.Runtime.StashIsReopening) return;
            Core.Runtime.LoadCurrentStash();
        }

    }
}
