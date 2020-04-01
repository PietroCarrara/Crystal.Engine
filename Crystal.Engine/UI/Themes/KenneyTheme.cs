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
        private Dictionary<string, SliderTheme> sliderThemes;
        private Dictionary<string, CheckboxTheme> checkboxThemes;

        public IFont SmallFont => fonts["Kenvector thin 12"];
        public IFont MediumFont => fonts["Kenvector thin 36"];
        public IFont BigFont => fonts["Kenvector thin 72"];

        public NinePatchImage PanelBackground => panelBackgrounds["blue"];
        public ButtonTheme ButtonTheme => ButtonThemes["blue"];
        public SliderTheme SliderTheme => sliderThemes["blue"];
        public CheckboxTheme CheckboxTheme => checkboxThemes["blue"];

        public Dictionary<string, IFont> Fonts => fonts;
        public Dictionary<string, NinePatchImage> PanelBackgrounds => panelBackgrounds;
        public Dictionary<string, ButtonTheme> ButtonThemes => buttonThemes;
        public Dictionary<string, SliderTheme> SliderThemes => sliderThemes;
        public Dictionary<string, CheckboxTheme> CheckboxThemes => checkboxThemes;


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
                    25
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

            this.sliderThemes = new Dictionary<string, SliderTheme>();
            this.sliderThemes.Add(
                "blue",
                new SliderTheme(
                    cm.Load<IDrawable>("engine://Themes/kenney/Blue/blue_slider_down"),
                    cm.Load<IDrawable>("engine://Themes/kenney/Grey/slider")
                )
            );

            this.checkboxThemes = new Dictionary<string, CheckboxTheme>();
            this.checkboxThemes.Add(
                "blue",
                new CheckboxTheme(
                    cm.Load<IDrawable>("engine://Themes/kenney/Blue/blue_checkmark"),
                    cm.Load<IDrawable>("engine://Themes/kenney/Grey/grey_box")
                )
            );
        }
    }
}