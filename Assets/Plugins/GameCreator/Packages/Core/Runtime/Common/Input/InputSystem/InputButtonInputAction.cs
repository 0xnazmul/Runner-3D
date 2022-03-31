using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCreator.Runtime.Common
{
    [Title("Input Action (Button)")]
    [Category("Input System/Input Action (Button)")]
    
    [Description("When an Input Action asset with a Button behavior is executed")]
    [Image(typeof(IconBoltOutline), ColorTheme.Type.Blue)]
    
    [Keywords("Unity", "Asset", "Map")]
    
    [Serializable]
    public class InputButtonInputAction : TInputButtonInputAction
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private InputActionFromAsset m_Input = new InputActionFromAsset();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override InputAction InputAction => this.m_Input.InputAction;
    }
}