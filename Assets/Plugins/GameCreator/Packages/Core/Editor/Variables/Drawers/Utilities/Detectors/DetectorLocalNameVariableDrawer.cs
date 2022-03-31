using System;
using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;

namespace GameCreator.Editor.Variables
{
    [CustomPropertyDrawer(typeof(DetectorLocalNameVariable))]
    public class DetectorLocalNameVariableDrawer : TDetectorNameVariableDrawer
    {
        protected override Type AssetType => typeof(LocalNameVariables);
        protected override bool AllowSceneReferences => true;

        protected override TNamePickTool Tool(ObjectField field, SerializedProperty property)
        {
            return new LocalNamePickTool(field, property);
        }
    }
}