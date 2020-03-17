using System.Collections.Generic;
using Crystal.Framework.UI;
using Crystal.Framework.Content;
using Crystal.Engine.Backends.MonoGame.Wrappers;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.UI.Themes
{
    public class KenneyTheme : ITheme
    {
        private Dictionary<string, IFont> fonts;

        public IFont SmallFont => this.fonts["Kenvector thin 12"];
        public IFont MediumFont => this.fonts["Kenvector thin 36"];
        public IFont BigFont => this.fonts["Kenvector thin 72"];
        
        public Dictionary<string, IFont> Fonts => fonts;
        
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
        }
    }
}