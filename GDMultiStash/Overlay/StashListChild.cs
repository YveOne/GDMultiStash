using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GDMultiStash.Common.Objects;
using GDMultiStash.Common.Overlay;
using GDMultiStash.Overlay.Controls;
using GDMultiStash.Overlay.Controls.Base;

namespace GDMultiStash.Overlay
{
    internal class StashListChild : SelectableListChild<StashObject>
    {
        public override Color DebugColor => Color.FromArgb(128, 0, 255, 255);

        private readonly ImageElement _lockSign;
        private readonly ImageElement _usageIndicator;
        private bool _locked = false;

        public StashListChild() : base()
        {
            TextWidth -= 60;
            _lockSign = new ImageElement()
            {
                Resource = StaticResources.LockSignResource,
                AnchorPoint = Anchor.Right,
                X = -55,
                Scale = 0.8f,
                Visible = false,
                Alpha = 0.5f,
                AutoSize = true,
            };
            _usageIndicator = new ImageElement()
            {
                X = -10,
                AnchorPoint = Anchor.Right,
                Height = 10,
                Width = 40,
            };

            AddChild(_lockSign);
            AddChild(_usageIndicator);

            MouseClick += delegate { Global.Runtime.SwitchToStash(Model.ID); };
        }

        public bool Locked
        {
            get { return _locked; }
            set
            {
                _locked = value;
                _lockSign.Visible = value;
            }
        }

        public bool ShowWorkload
        {
            get { return _usageIndicator.Visible; }
            set
            {
                _usageIndicator.Visible = value;
                _lockSign.X = value ? -55 : -10 ;
            }
        }

        public void UpdateUsageIndicator()
        {
            if (ParentViewport == null) return;
            ParentViewport.OverlayResources.DeleteResource(_usageIndicator.Resource);
            ParentViewport.OverlayResources.AsyncCreateImageResource(Model.UsageIndicator, System.Drawing.Imaging.ImageFormat.Png)
                .ResourceCreated += delegate(object sender, ResourceHandler.ResourceCreatedEventArgs args) {
                    _usageIndicator.Resource = args.Resource;
                };
        }

        public override void ViewportConnected(Viewport parentViewport)
        {
            base.ViewportConnected(parentViewport);
            UpdateUsageIndicator();
        }

    }
}
