using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Variables
{
    [CustomEditor(typeof(GlobalListVariables))]
    public class GlobalListVariablesEditor : UnityEditor.Editor
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
            SerializedProperty indexList = this.serializedObject.FindProperty("m_IndexList");
            SerializedProperty saveUniqueID = this.serializedObject.FindProperty("m_SaveUniqueID");

            PropertyField indexListRuntime = new PropertyField(indexList);
            PropertyField fieldSaveUniqueID = new PropertyField(saveUniqueID);

            this.m_Body.Add(indexListRuntime);
            this.m_Body.Add(fieldSaveUniqueID);
        }
        
        // PAINT RUNTIME: -------------------------------------------------------------------------

        private void PaintRuntime()
        {
            GlobalListVariables variables = this.target as GlobalListVariables;
            if (variables == null) return;
            
            variables.Unregister(this.RuntimeOnChange);
            variables.Register(this.RuntimeOnChange);
            
            this.RuntimeOnChange(ListVariableRuntime.Change.Set, 0);
        }

        private void RuntimeOnChange(ListVariableRuntime.Change changeType, int index)
        {
            this.m_Body.styleSheets.Clear();
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet styleSheet in sheets) this.m_Body.styleSheets.Add(styleSheet);

            VisualElement content = new VisualElement
            {
                name = NAME_LIST
            };

            GlobalListVariables variables = this.target as GlobalListVariables;
            if (variables == null) return;

            int variablesCount = variables.Count;
            for (int i = 0; i < variablesCount; ++i)
            {
                Image image = new Image
                {
                    image = variables.Icon(i)
                };
            
                Label title = new Label(variables.Title(i));
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
