// using System;
// using System.Collections.Generic;
// using GameCreator.Editor.Common;
// using GameCreator.Runtime.Common;
// using UnityEditor;
// using UnityEditor.Overlays;
// using UnityEngine.UIElements;
//
// namespace GameCreator.Editor.Overlays
// {
//     [Overlay(typeof(SceneView), "Game Creator", true)]
//     internal class Toolbar_OLD : Overlay
//     {
//         private const string USS_PATH = EditorPaths.TOOLBAR + "StyleSheets/Toolbar";
//         
//         private const string NAME_ROOT = "GC-Overlays-Toolbar";
//
//         private const string CLASS_ROOT_PANEL = "gc-overlays-toolbar-panel";
//         private const string CLASS_ROOT_HORIZONTAL = "gc-overlays-toolbar-horizontal";
//         private const string CLASS_ROOT_VERTICAL = "gc-overlays-toolbar-vertical";
//         
//         // MEMBERS: -------------------------------------------------------------------------------
//
//         private VisualElement m_Root;
//         
//         // PROPERTIES: ----------------------------------------------------------------------------
//         
//         protected override Layout supportedLayouts => Layout.All;
//
//         // CREATOR METHODS: -----------------------------------------------------------------------
//         
//         public override VisualElement CreatePanelContent()
//         {
//             this.CreateContent(CLASS_ROOT_PANEL);
//             return this.m_Root;
//         }
//
//         public VisualElement CreateHorizontalToolbarContent()
//         {
//             this.CreateContent(CLASS_ROOT_HORIZONTAL);
//             return this.m_Root;
//         }
//
//         public VisualElement CreateVerticalToolbarContent()
//         {
//             this.CreateContent(CLASS_ROOT_VERTICAL);
//             return this.m_Root;
//         }
//         
//         // PRIVATE METHODS: -----------------------------------------------------------------------
//
//         private void CreateContent(string className)
//         {
//             this.m_Root = new VisualElement { name = NAME_ROOT };
//             this.m_Root.AddToClassList(className);
//             
//             StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
//             foreach (StyleSheet sheet in sheets) this.m_Root.styleSheets.Add(sheet);
//             
//             TToolbar[] elements = this.GetList();
//             foreach (TToolbar element in elements)
//             {
//                 ToolbarButton button = new ToolbarButton(
//                     element.Icon.Texture,
//                     element.Tooltip,
//                     element.Run
//                 );
//                 
//                 this.m_Root.Add(button);
//             }
//         }
//         
//         private TToolbar[] GetList()
//         {
//             TypeCache.TypeCollection collection = TypeCache.GetTypesDerivedFrom<TToolbar>();
//             List<TToolbar> attributes = new List<TToolbar>();
//             
//             foreach (Type type in collection)
//             {
//                 if (type.IsAbstract) continue;
//                 if (Activator.CreateInstance(type) is TToolbar instance)
//                 {
//                     attributes.Add(instance);
//                 }
//             }
//
//             attributes.Sort((a, b) => a.Priority.CompareTo(b.Priority));
//             return attributes.ToArray();
//         }
//     }
// }