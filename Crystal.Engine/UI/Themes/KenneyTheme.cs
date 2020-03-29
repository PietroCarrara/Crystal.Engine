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
        private Dictionary<string, ButtonTheme> buttonThemes;

        public IFont SmallFont => this.fonts["Kenvector thin 12"];
        public IFont MediumFont => this.fonts["Kenvector thin 36"];
        public IFont BigFont => this.fonts["Kenvector thin 72"];

        public NinePatchImage PanelBackground => this.panelBackgrounds["blue"];

        public ButtonTheme ButtonTheme => this.ButtonThemes["blue"];

        public Dictionary<string, IFont> Fonts => fonts;
        public Dictionary<string, NinePatchImage> PanelBackgrounds => panelBackgrounds;
        public Dictionary<string, ButtonTheme> ButtonThemes => buttonThemes;

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
                    (92, 92)
                )
            );

            this.buttonThemes = new Dictionary<string, ButtonTheme>();
            this.buttonThemes.Add(
                "blue",
                new ButtonTheme(
                    new ThreePatchImage(
                        cm.Load<IDrawable>("engine://Themes/kenney/Blue/blue_button"),
                        6,
                        42
                    ),
                    new ThreePatchImage(
                        cm.Load<IDrawable>("engine://Themes/kenney/Blue/blue_button_hover"),
                        6,
                        42
                    )
                )
            );
        }
    }
}