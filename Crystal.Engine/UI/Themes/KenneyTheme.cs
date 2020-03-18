using System.Collections.Generic;
using Crystal.Framework;
using Crystal.Framework.UI;
using Crystal.Framework.Content;
using Crystal.Framework.Graphics;

namespace Crystal.Engine.UI.Themes
{
    public class KenneyTheme : ITheme
    {
        private Dictionary<string, IFont> fonts;
        private Dictionary<string, NinePatchImage> panelBackgrounds;

        public IFont SmallFont => this.fonts["Kenvector thin 12"];
        public IFont MediumFont => this.fonts["Kenvector thin 36"];
        public IFont BigFont => this.fonts["Kenvector thin 72"];

        public NinePatchImage PanelBackground => this.panelBackgrounds["blue"];
        
        public Dictionary<string, IFont> Fonts => fonts;
        public Dictionary<string, NinePatchImage> PanelBackgrounds => panelBackgrounds;
        
        public void Load(IContentManager cm)
        {
            this.fonts = new Dictionary<string, IFont>();
            this.fonts.Add(
                "Kenvector thin 12",
                cm.Load<IFont>("engine://Themes/kenney/Font/kenvector_future_thin_12")
            );
            this.fonts.Add(
                "Kenvector thin 36",
                cm.Load<IFont>("engine://Themes/kenney/Font/kenvector_future_thin_36")
            );
            this.fonts.Add(
                "Kenvector thin 72",
                cm.Load<IFont>("engine://Themes/kenney/Font/kenvector_future_thin_72")
            );

            this.panelBackgrounds = new Dictionary<string, NinePatchImage>();
            this.panelBackgrounds.Add(
                "blue",
                new NinePatchImage(
                    cm.Load<IDrawable>("engine://Themes/kenney/Blue/blue_panel"),
                    (7, 7),
                    (92, 7),
                    (7, 92),
                    (92, 92),
                    (25, 25)
                )
            );
        }
    }
}