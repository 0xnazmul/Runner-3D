using System;
using UnityEditor;
using UnityEngine.UIElements;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Editor.Variables
{
    public sealed class PickFieldElement : TypeSelectorValueElement
    {
        private const string USS_PATH = EditorPaths.VARIABLES + "StyleSheets/PickFieldElement";
        
        // MEMBERS: -------------------------------------------------------------------------------

        private Button m_Button;
        private Label m_Label;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string ElementNameRoot => "GC-PickField-Root";
        protected override string ElementNameHead => "GC-PickField-Head";
        protected override string ElementNameBody => "GC-PickField-Body";

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public PickFieldElement(SerializedProperty property, string label) : base(property, true)
        {
            this.TypeSelector = new TypeSelectorFancyProperty(this.m_Property, this.m_Button);
            this.TypeSelector.EventChange += this.OnChange;

            this.m_Label.text = label;
            this.RefreshButton();
        }
        
        protected override void CreateHead()
        {
            base.CreateHead();
            this.m_Head.AddToClassList("unity-base-field");
            
            this.m_Button = new Button();
            this.m_Button.AddToClassList("unity-base-field__input");

            this.m_Label = new Label();
            this.m_Label.AddToClassList("unity-base-field__label");
            this.m_Label.AddToClassList("unity-label");
            this.m_Label.AddToClassList("unity-inspector-element");
            this.m_Label.AddToClassList("unity-property-field__label");
            
            this.m_Head.Add(this.m_Label);
            this.m_Head.Add(this.m_Button);

            this.LoadHeadStyleSheet(this.m_Head);
        }

        protected override void CreateBody()
        {
            if (this.m_Property == null) return;
            SerializationUtils.CreateChildProperties(
                this.m_Body, 
                this.m_Property, 
                this.HideLabels
            );
        }

        protected override void OnChange(Type prevType, Type newType)
        {
            if (this.m_Property == null) return;

            base.OnChange(prevType, newType);
            this.RefreshButton();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void LoadHeadStyleSheet(VisualElement element)
        {
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets)
            {
                element.styleSheets.Add(sheet);
            }
        }
        
        private void RefreshButton()
        {
            if (this.m_Property == null) return;
            
            Type fieldType = TypeUtils.GetTypeFromProperty(this.m_Property, true);
            this.m_Button.text = TypeUtils.GetTitleFromType(fieldType);
        }
    }
}
