using System;
using System.Collections.Generic;
using System.IO;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Common
{
    public abstract class TPolymorphicListTool : VisualElement, IPolymorphicListTool
    {
        public const string CLASS_HEAD = "gc-list-head";
        public const string CLASS_BODY = "gc-list-body";
        public const string CLASS_FOOT = "gc-list-foot";
        
        // MEMBERS: -------------------------------------------------------------------------------

        protected abstract string ElementNameHead { get; }
        protected abstract string ElementNameBody { get; }
        protected abstract string ElementNameFoot { get; }
        
        protected readonly SerializedProperty m_Property;

        protected readonly VisualElement m_Head;
        protected readonly VisualElement m_Body;
        protected readonly VisualElement m_Foot;

        // PROPERTIES: ----------------------------------------------------------------------------

        public SerializedProperty PropertyList { get; }
        
        public SerializedObject SerializedObject => this.m_Property?.serializedObject;
        
        public abstract bool AllowReordering    { get; }
        public abstract bool AllowDuplicating   { get; }
        public abstract bool AllowDeleting      { get; }
        public abstract bool AllowContextMenu   { get; }
        public abstract bool AllowCopyPaste     { get; }
        public abstract bool AllowInsertion     { get; }
        public abstract bool AllowBreakpoint    { get; }
        public abstract bool AllowDisable       { get; }
        public abstract bool AllowDocumentation { get; }

        protected virtual List<string> CustomStyleSheetPaths => new List<string>();

        public override VisualElement contentContainer => this.m_Body;
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action<int> EventChangeSize;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        protected TPolymorphicListTool(SerializedProperty property, string listName)
        {
            this.m_Head = new VisualElement { name = this.ElementNameHead };
            this.m_Body = new VisualElement { name = this.ElementNameBody };
            this.m_Foot = new VisualElement { name = this.ElementNameFoot };
            
            this.m_Head.AddToClassList(CLASS_HEAD);
            this.m_Body.AddToClassList(CLASS_BODY);
            this.m_Foot.AddToClassList(CLASS_FOOT);
            
            this.m_Property = property;
            this.PropertyList = property.FindPropertyRelative(listName);

            this.SetupHead();
            this.SetupBody();
            this.SetupFoot();

            this.SetStyles();
            this.Refresh();
        }

        // PROTECTED VIRTUAL METHODS: -------------------------------------------------------------

        protected virtual void SetupHead()
        {
            this.hierarchy.Add(this.m_Head);
        }

        protected virtual void SetupBody()
        {
            this.hierarchy.Add(this.m_Body);
        }

        protected virtual void SetupFoot()
        {
            this.hierarchy.Add(this.m_Foot);
        }

        protected abstract VisualElement MakeItemTool(int index);

        // PRIVATE METHODS: ----------------------------------------------------------------------- 

        private void SetStyles()
        {
            List<string> styleSheetsPaths = this.CustomStyleSheetPaths;
            styleSheetsPaths.Add(PathUtils.Combine(
                EditorPaths.COMMON, 
                "Polymorphism/Lists/StyleSheets/Polymorphic-List"
            ));

            StyleSheet[] sheets = StyleSheetUtils.Load(styleSheetsPaths.ToArray());
            foreach (StyleSheet sheet in sheets) this.styleSheets.Add(sheet);
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected void CollapseItems()
        {
            this.SerializedObject.Update();
            
            int arraySize = this.PropertyList.arraySize;
            for (int i = 0; i < arraySize; ++i)
            {
                SerializedProperty item = this.PropertyList.GetArrayElementAtIndex(i);
                item.FindPropertyRelative("m_IsExpanded").boolValue = false;
            }

            SerializationUtils.ApplyUnregisteredSerialization(this.SerializedObject);
            this.Refresh();
        }

        protected void ExecuteEventChangeSize(int size)
        {
            this.EventChangeSize?.Invoke(size);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Refresh()
        {
            this.SerializedObject.Update();
            this.Clear();
            
            for (int i = 0; i < this.PropertyList.arraySize; ++i)
            {
                this.Add(this.MakeItemTool(i));
            }
        }

        public virtual void InsertItem(int index, object value)
        {
            this.SerializedObject.Update();
            
            this.PropertyList.InsertArrayElementAtIndex(index);
            this.PropertyList.GetArrayElementAtIndex(index).SetValue(value);

            SerializationUtils.ApplyUnregisteredSerialization(this.SerializedObject);

            int size = this.PropertyList.arraySize;
            this.EventChangeSize?.Invoke(size);
            
            using ChangeEvent<int> changeEvent = ChangeEvent<int>.GetPooled(size, size);
            changeEvent.target = this;
            this.SendEvent(changeEvent);
            
            this.Refresh();
        }

        public virtual void DeleteItem(int index)
        {
            this.SerializedObject.Update();
            if (index < 0) return;

            this.PropertyList.DeleteArrayElementAtIndex(index);
            SerializationUtils.ApplyUnregisteredSerialization(this.SerializedObject);

            int size = this.PropertyList.arraySize;
            this.EventChangeSize?.Invoke(size);
            
            using ChangeEvent<int> changeEvent = ChangeEvent<int>.GetPooled(size, size);
            changeEvent.target = this;
            this.SendEvent(changeEvent);
            
            this.Refresh();
        }
        
        public virtual void DuplicateItem(int index)
        {
            this.SerializedObject.Update();
            if (index < 0) return;

            object source = this.PropertyList.GetArrayElementAtIndex(index).GetManagedValue();
            if (source == null) return;
            
            this.PropertyList.InsertArrayElementAtIndex(index);
            SerializedProperty newElement = this.PropertyList.GetArrayElementAtIndex(index + 1);

            CopyPasteUtils.Duplicate(newElement, source);
            SerializationUtils.ApplyUnregisteredSerialization(this.SerializedObject);

            int size = this.PropertyList.arraySize;
            this.EventChangeSize?.Invoke(size);
            
            using ChangeEvent<int> changeEvent = ChangeEvent<int>.GetPooled(size, size);
            changeEvent.target = this;
            this.SendEvent(changeEvent);
            
            this.Refresh();
        }

        public virtual void MoveItems(int sourceIndex, int destinationIndex)
        {
            this.SerializedObject.Update();

            destinationIndex = Math.Max(destinationIndex, 0);
            destinationIndex = Math.Min(destinationIndex, this.PropertyList.arraySize - 1);

            this.PropertyList.MoveArrayElement(
                sourceIndex,
                destinationIndex
            );

            SerializationUtils.ApplyUnregisteredSerialization(this.SerializedObject);
            this.Refresh();
        }
    }
}
