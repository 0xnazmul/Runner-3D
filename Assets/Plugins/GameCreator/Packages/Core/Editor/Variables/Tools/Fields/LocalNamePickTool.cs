using System.Collections.Generic;
using System.Reflection;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Variables
{
    public class LocalNamePickTool : TNamePickTool
    {
        private const string NAME_ROOT_NAME = "GC-NamePickTool-Name";
        private const string NAME_DROPDOWN = "GC-NamePickTool-Dropdown";

        private static readonly IIcon ICON_DROPDOWN = new IconArrowDropDown(ColorTheme.Type.TextLight);
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private TextField m_NameField;
        private VisualElement m_NameDropdown;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public LocalNamePickTool(ObjectField asset, SerializedProperty property)
            : base(asset, property, false, ValueNull.TYPE_ID)
        { }
        
        public LocalNamePickTool(ObjectField asset, SerializedProperty property, IdString typeID)
            : base(asset, property, true, typeID)
        { }

        protected override void RefreshPickList(Object asset)
        {
            base.RefreshPickList(asset);
            
            this.m_NameField = new TextField(string.Empty)
            {
                bindingPath = this.m_PropertyName.propertyPath
            };
            
            this.m_NameField.Bind(this.m_Property.serializedObject);

            this.m_NameDropdown = new Image
            {
                image = ICON_DROPDOWN.Texture,
                name = NAME_DROPDOWN,
                focusable = true
            };
            
            this.m_NameDropdown.SetEnabled(asset != null);
            this.m_NameDropdown.AddManipulator(new MouseDropdownManipulator(context =>
            {
                List<string> listNames = this.GetVariablesList(asset);

                foreach (string listName in listNames)
                {
                    context.menu.AppendAction(
                        listName,
                        menuAction =>
                        {
                            this.m_PropertyName.stringValue = menuAction.name;
                            SerializationUtils.ApplyUnregisteredSerialization(
                                this.m_Property.serializedObject
                            );
                        },
                        menuAction => menuAction.name == this.m_PropertyName.stringValue
                            ? DropdownMenuAction.Status.Checked
                            : DropdownMenuAction.Status.Normal
                    );
                }
            }));

            VisualElement nameContainer = new VisualElement { name = NAME_ROOT_NAME };
            
            nameContainer.Add(new Label(" "));
            nameContainer.Add(this.m_NameField);
            nameContainer.Add(this.m_NameDropdown);

            this.Add(nameContainer);
        }

        protected override List<string> GetVariablesList(Object asset)
        {
            List<string> list = new List<string> { string.Empty };
            LocalNameVariables variable = asset as LocalNameVariables;
            
            if (variable == null) return list;

            NameVariableRuntime runtime = variable.GetType()
                .GetField("m_Runtime", BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(variable) as NameVariableRuntime;
            
            NameList names = runtime?.GetType()
                .GetField("m_List", BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(runtime) as NameList;

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