using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Objects;
using GDMultiStash.Overlay.Controls;
using GDMultiStash.Overlay.Controls.Base;

using D3DHook.Overlay.Common;

namespace GDMultiStash.Overlay
{
    internal class InfoBox : Element
    {

        private readonly TextElement _titleElement;
        private readonly TextElement _lastChangeIntern;

        private readonly SmallButton _saveButton;
        private readonly SmallButton _loadButton;
        private readonly InfoBoxReloadButton _reloadButton;

        public override Color DebugColor => Color.FromArgb(128, 255, 255, 0);

        public InfoBox()
        {

            _titleElement = new TextElement(StaticResources.InfoBoxTitleFontHandler)
            {
                WidthToParent = true,
                Height = 25,
                Align = StringAlignment.Near,
                Color = Color.FromArgb(255, 235, 222, 195),
                AnchorPoint = Anchor.TopLeft,
                X = 10,
                Y = 10,
            };
            AddChild(_titleElement);

            _lastChangeIntern = new TextElement(StaticResources.InfoBoxTextFontHandler)
            {
                WidthToParent = true,
                Height = 20,
                Align = StringAlignment.Near,
                Color = Color.FromArgb(255, 235, 222, 195),
                AnchorPoint = Anchor.TopLeft,
                X = 10,
                Y = 10 + 25 + 5,
            };
            AddChild(_lastChangeIntern);

            _saveButton = new SmallButton()
            {
                X = 6,
                Y = -5,
                AnchorPoint = Anchor.BottomLeft,
            };
            AddChild(_saveButton);

            _loadButton = new SmallButton()
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

            G.Stashes.ActiveStashChanged += delegate {
                UpdateInfoText();
            };
            G.Ingame.StashReopenEnd += delegate {
                UpdateInfoText();
            };

            _saveButton.MouseClick += CheatSaveButton_Click;
            _loadButton.MouseClick += CheatLoadButton_Click;
            _reloadButton.MouseClick += ReloadButton_Click;

            UpdateButtonText();
            G.Configuration.LanguageChanged += delegate {
                UpdateButtonText();
            };
            G.Stashes.StashesInfoChanged += delegate {
                UpdateInfoText(); // maybe name changed
            };
        }

        private void UpdateButtonText()
        {
            _saveButton.Text = G.L.SaveButton();
            _loadButton.Text = G.L.LoadButton();
        }

        private void UpdateInfoText()
        {
            if (!G.Stashes.TryGetStash(G.Stashes.ActiveStashID, out StashObject stash))
                return;
            _titleElement.Text = stash.Name;
            _lastChangeIntern.Text = stash.LastWriteTime.ToString();
        }

        private void CheatSaveButton_Click(object sender, EventArgs e)
        {
            G.Ingame.SaveCurrentStash();
        }

        private void CheatLoadButton_Click(object sender, EventArgs e)
        {
            G.Ingame.LoadCurrentStash();
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            G.Ingame.ReloadCurrentStash();
        }


        

    }
}
