using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Editor.Common.ID;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Variables
{
    public abstract class TNamePickTool : VisualElement
    {
        private const string USS_PATH = EditorPaths.VARIABLES + "StyleSheets/NamePickTool";

        // MEMBERS: -------------------------------------------------------------------------------

        protected readonly SerializedProperty m_Property;
        
        private readonly SerializedProperty m_PropertyVariable;
        protected readonly SerializedProperty m_PropertyName;

        private readonly ObjectField m_Asset;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected readonly bool HasType = false;
        protected IdString TypeID { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TNamePickTool(ObjectField asset, SerializedProperty property, 
            bool hasType, IdString typeID)
        {
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet styleSheet in sheets)
            {
                this.styleSheets.Add(styleSheet);
            }
            
            this.m_Property = property;
            this.m_Property.serializedObject.Update();
            
            this.m_PropertyVariable = property.FindPropertyRelative("m_Variable");
            this.m_PropertyName = property.FindPropertyRelative($"m_Name.{IdStringDrawer.NAME_STRING}");

            this.HasType = hasType;
            this.TypeID = typeID;
            this.m_Asset = asset;
            
            asset.UnregisterValueChangedCallback(this.OnChangeAsset);
            asset.RegisterValueChangedCallback(this.OnChangeAsset);
            
            this.RefreshPickList(this.m_PropertyVariable.objectReferenceValue);
        }

        private void OnChangeAsset(ChangeEvent<Object> changeEvent)
        {
            this.RefreshPickList(changeEvent.newValue);
        }

        protected virtual void RefreshPickList(Object asset)
        {
            this.Clear();
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        protected abstract List<string> GetVariablesList(Object asset);
    }
}