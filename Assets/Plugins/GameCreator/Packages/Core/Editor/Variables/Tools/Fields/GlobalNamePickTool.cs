using System.Collections.Generic;
using System.Reflection;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Variables
{
    public class GlobalNamePickTool : TNamePickTool
    {
        private PopupField<string> m_Popup;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public GlobalNamePickTool(ObjectField asset, SerializedProperty property)
            : base(asset, property, false, ValueNull.TYPE_ID)
        { }
        
        public GlobalNamePickTool(ObjectField asset, SerializedProperty property, IdString typeID)
            : base(asset, property, true, typeID)
        { }

        protected override void RefreshPickList(Object asset)
        {
            base.RefreshPickList(asset);
            List<string> listNames = this.GetVariablesList(asset);

            int index = listNames.Count > 1 
                ? listNames.IndexOf(this.m_PropertyName.stringValue)
                : 0;
            
            this.m_Popup = new PopupField<string>(
                " ",
                listNames, Mathf.Max(0, index),
                selection => selection,
                selection => selection
            );
            
            this.m_Popup.RegisterValueChangedCallback(changeEvent =>
            {
                this.m_PropertyName.serializedObject.Update();
                this.m_PropertyName.stringValue = changeEvent.newValue;

                this.m_PropertyName.serializedObject.ApplyModifiedProperties();
                this.m_PropertyName.serializedObject.Update();
            });

            this.Add(this.m_Popup);
        }

        protected override List<string> GetVariablesList(Object asset)
        {
            List<string> list = new List<string> { string.Empty };
            GlobalNameVariables variable = asset as GlobalNameVariables;
            
            if (variable == null) return list;

            NameList names = variable.GetType()
                .GetField("m_NameList", BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(variable) as NameList;

            for (int i = 0; i < names?.Length; ++i)
            {
                NameVariable nameVariable = names.Get(i);
                if (this.HasType && nameVariable.TypeID.Hash != this.TypeID.Hash) continue;

                list.Add(nameVariable.Name);
            }

            return list;
        }
    }
}