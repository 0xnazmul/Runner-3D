using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

namespace GameCreator.Editor.Overlays
{
    // [Overlay(typeof(SceneView), "Game Creator", true)]
    // public class Toolbar_NEW : ToolbarOverlay
    // {
    //     // private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;
    //     
    //     private Toolbar_NEW() : base()
    //     {
    //         // FieldInfo toolbar = typeof(Toolbar).BaseType?.GetField("m_Toolbar", BINDING_FLAGS);
    //         // MethodInfo addMethod = toolbar?.FieldType.GetMethod(
    //         //     "AddElement",
    //         //     BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic
    //         // );
    //         //
    //         // object instance = toolbar?.GetValue(this);
    //         //
    //         // Debug.Log($"fieldInfo is null ? {toolbar == null}");
    //         // Debug.Log($"instance is null ? {instance == null}");
    //         // addMethod?.Invoke(instance, new object[] {ToolbarButton.id});
    //     }
    // }
}