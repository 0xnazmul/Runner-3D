using GameCreator.Runtime.Common;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Common
{
    public sealed class ErrorMessage : VisualElement
    {
        private const string USS_PATH = EditorPaths.COMMON + "UI/StyleSheets/ErrorMessage";
        
        private static readonly IIcon ICON = new IconErrorOutline(ColorTheme.Type.TextLight);

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ErrorMessage(string message)
        {
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets)
            {
                this.styleSheets.Add(sheet);
            }

            Image image = new Image { image = ICON.Texture };
            Label label = new Label { text = message };
            
            this.Add(image);
            this.Add(label);
        }
    }
}
