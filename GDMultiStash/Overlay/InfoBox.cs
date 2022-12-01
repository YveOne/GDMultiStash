using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common;
using GDMultiStash.Common.Overlay;
using GrimDawnLib;

namespace GDMultiStash.Overlay.Elements
{
    internal class InfoBox : Element
    {

        public static Font _TitleFont = null;
        public static Font _TextFont = null;

        private readonly TextElement _titleElement;
        private readonly TextElement _lastChangeIntern;

        private readonly InfoBoxButton _saveButton;
        private readonly InfoBoxButton _loadButton;
        private readonly InfoBoxReloadButton _reloadButton;

        public InfoBox()
        {
            X = 2;
            Y = 605;
            WidthToParent = true;
            Width = -10;
            Height = 127;

            _titleElement = new TextElement()
            {
                WidthToParent = true,
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
                WidthToParent = true,
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
                X = _saveButton.X + _saveButton.Width + 6,
                Y = -5,
                AnchorPoint = Anchor.BottomLeft,
            };
            AddChild(_loadButton);

            _reloadButton = new InfoBoxReloadButton()
            {
                X = -5,
                Y = -5,
                AnchorPoint = Anchor.BottomRight,
            };
            AddChild(_reloadButton);

            Global.Runtime.ActiveStashChanged += delegate {
                UpdateInfoText();
            };

            _saveButton.MouseClick += CheatSaveButton_Click;
            _loadButton.MouseClick += CheatLoadButton_Click;
            _reloadButton.MouseClick += ReloadButton_Click;

            UpdateButtonText();
            Global.Configuration.LanguageChanged += delegate {
                UpdateButtonText();
            };

            Global.Runtime.StashReopenStart += delegate {
                Alpha = 0.33f;
            };

            Global.Runtime.StashReopenEnd += delegate {
                Alpha = 1.0f;
            };

        }

        private void UpdateButtonText()
        {
            _saveButton.Text = Global.L["overlayWindow_saveButton"];
            _loadButton.Text = Global.L["overlayWindow_loadButton"];
        }

        private void UpdateInfoText()
        {
            StashObject stash = Global.Stashes.GetStash(Global.Runtime.ActiveStashID);
            if (stash == null) return; // something happend
            _titleElement.Text = stash.Name;
            _lastChangeIntern.Text = stash.LastWriteTime.ToString();
        }

        private void CheatSaveButton_Click(object sender, EventArgs e)
        {
            if (Global.Runtime.StashIsReopening) return;
            Global.Runtime.SaveCurrentStash();
        }

        private void CheatLoadButton_Click(object sender, EventArgs e)
        {
            if (Global.Runtime.StashIsReopening) return;
            Global.Runtime.LoadCurrentStash();
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            if (Global.Runtime.StashIsReopening) return;
            Global.Runtime.ReloadCurrentStash();
        }


        

    }
}
