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
    public class InfoWindow : Element
    {

        public static Font _TitleFont = null;
        public static Font _TextFont = null;

        private readonly TextElement _titleElement;
        private readonly TextElement _lastChangeIntern;

        private readonly InfoButton _saveButton;
        private readonly InfoButton _loadButton;

        public InfoWindow()
        {
            X = 6;
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
                //Color = Color.FromArgb(255, 140, 131, 109),
                Color = Color.FromArgb(255, 235, 222, 195),
                AnchorPoint = Anchor.TopLeft,
                X = 10,
                Y = 10 + 25 + 5,
            };
            AddChild(_lastChangeIntern);

            _saveButton = new InfoButton()
            {
                X = 6,
                Y = -5,
                AnchorPoint = Anchor.BottomLeft,
            };
            AddChild(_saveButton);

            _loadButton = new InfoButton()
            {
                X = 6 + 6 + 120,
                Y = -5,
                AnchorPoint = Anchor.BottomLeft,
            };
            AddChild(_loadButton);

            Core.Runtime.ActiveStashChanged += delegate {
                UpdateInfo();
            };

            _saveButton.MouseClick += CheatSaveButton_Click;
            _loadButton.MouseClick += CheatLoadButton_Click;

            SetButtonText();
            Core.Config.LanguageChanged += delegate {
                SetButtonText();
            };
        }

        private void SetButtonText()
        {
            _saveButton.Text = Core.Localization.GetString("button_save");
            _loadButton.Text = Core.Localization.GetString("button_load");
        }






        private bool _buttonsDisabled = false;

        private void UpdateInfo()
        {
            Common.Stash stash = Core.Stashes.GetStash(Core.Runtime.ActiveStashID);
            if (stash == null) return; // something happend
            _titleElement.Text = stash.Name;
            _lastChangeIntern.Text = stash.LastWriteTime.ToString();
        }

        private void CheatSaveButton_Click(object sender, EventArgs e)
        {
            if (_buttonsDisabled) return;

            _buttonsDisabled = true;
            Alpha = 0.33f;
            Core.Runtime.SaveCurrentStash();
            _buttonsDisabled = false;
            Alpha = 1f;

        }

        private void CheatLoadButton_Click(object sender, EventArgs e)
        {
            if (_buttonsDisabled) return;

            _buttonsDisabled = true;
            Alpha = 0.33f;
            Core.Runtime.LoadCurrentStash();
            _buttonsDisabled = false;
            Alpha = 1f;

        }

    }
}
