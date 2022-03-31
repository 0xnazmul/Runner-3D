using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace GameCreator.Editor.Common
{
    public static class CopyPasteUtils
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public static object SourceObject { get; private set; }
        public static Type SourceType { get; private set; }

        // EVENTS: --------------------------------------------------------------------------------

        public static event Action EventSoftCopy;
        public static event Action EventSoftPaste;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static void CopyToClipboard(string text)
        {
            TextEditor textEditor = new TextEditor { text = text };

            textEditor.SelectAll();
            textEditor.Copy();
        }

        public static void SoftCopy(object source, Type baseType)
        {
            string jsonSource = EditorJsonUtility.ToJson(source);
            
            SourceType = source.GetType();
            SourceObject = JsonUtility.FromJson(jsonSource, SourceType);

            EventSoftCopy?.Invoke();
        }

        public static bool CanSoftPaste(Type baseType)
        {
            if (SourceObject == null) return false;
            return SourceType != null && baseType.IsAssignableFrom(SourceType);
        }

        public static void SoftPaste(SerializedProperty target, Type baseType)
        {
            if (!CanSoftPaste(baseType)) return;

            target.SetManaged(SourceObject);
            EventSoftPaste?.Invoke();
        }
        
        public static void Duplicate(SerializedProperty target, object source)
        {
            if (source == null) return;
            if (target.propertyType != SerializedPropertyType.ManagedReference) return;
            
            string jsonSource = EditorJsonUtility.ToJson(source);
            
            object newInstance = Activator.CreateInstance(source.GetType());
            EditorJsonUtility.FromJsonOverwrite(jsonSource, newInstance);

            target.SetManaged(newInstance);
        }
    }
}
