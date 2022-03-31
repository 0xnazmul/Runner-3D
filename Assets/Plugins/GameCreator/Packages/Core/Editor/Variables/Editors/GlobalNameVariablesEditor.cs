using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Variables
{
    [CustomEditor(typeof(GlobalNameVariables))]
    public class GlobalNameVariablesEditor : UnityEditor.Editor
    {
        private const string USS_PATH = EditorPaths.VARIABLES + "StyleSheets/RuntimeGlobalList";
        
        private const string NAME_LIST = "GC-RuntimeGlobal-List-Head";
        private const string CLASS_LIST_ELEMENT = "gc-runtime-global-list-element";
        
        // MEMBERS: -------------------------------------------------------------------------------

        private VisualElement m_Root;
        private VisualElement m_Head;
        private VisualElement m_Body;

        // PAINT METHOD: --------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            this.m_Head = new VisualElement();
            this.m_Body = new VisualElement();
            
            this.m_Root.Add(this.m_Head);
            this.m_Root.Add(this.m_Body);

            this.m_Root.style.marginTop = new StyleLength(5);
            
            switch (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                case true: this.PaintRuntime(); break;
                case false: this.PaintEditor(); break;
            }

            return this.m_Root;
        }

        // PAINT EDITOR: --------------------------------------------------------------------------

        private void PaintEditor()
        {
            SerializedProperty nameList = this.serializedObject.FindProperty("m_NameList");
            SerializedProperty saveUniqueID = this.serializedObject.FindProperty("m_SaveUniqueID");

            PropertyField fieldNameList = new PropertyField(nameList);
            PropertyField fieldSaveUniqueID = new PropertyField(saveUniqueID);

            this.m_Body.Add(fieldNameList);
            this.m_Body.Add(fieldSaveUniqueID);
        }
        
        // PAINT RUNTIME: -------------------------------------------------------------------------

        private void PaintRuntime()
        {
            GlobalNameVariables variables = this.target as GlobalNameVariables;
            if (variables == null) return;
            
            variables.Unregister(this.RuntimeOnChange);
            variables.Register(this.RuntimeOnChange);
            
            this.RuntimeOnChange(string.Empty);
        }

        private void RuntimeOnChange(string variableName)
        {
            this.m_Body.Clear();
            this.m_Body.styleSheets.Clear();
            
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet styleSheet in sheets) this.m_Body.styleSheets.Add(styleSheet);

            VisualElement content = new VisualElement
            {
                name = NAME_LIST
            };

            GlobalNameVariables variables = this.target as GlobalNameVariables;
            if (variables == null) return;

            string[] names = variables.Names;
            foreach (string id in names)
            {
                Image image = new Image
                {
                    image = variables.Icon(id)
                };
            
                Label title = new Label(variables.Title(id));
                title.style.color = ColorTheme.Get(ColorTheme.Type.TextNormal);

                VisualElement element = new VisualElement();
                element.AddToClassList(CLASS_LIST_ELEMENT);

                element.Add(image);
                element.Add(title);
            
                content.Add(element);
            }
            
            this.m_Body.Add(content);
        }
    }
}
