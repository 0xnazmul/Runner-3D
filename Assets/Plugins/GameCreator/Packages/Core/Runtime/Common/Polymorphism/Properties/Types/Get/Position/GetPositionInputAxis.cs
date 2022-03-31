using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Input Axis")]
    [Category("Input/Input Axis")]
    
    [Image(typeof(IconGamepadCross), ColorTheme.Type.Green)]
    [Description("Reads the Vector2 value of an input device")]

    [Serializable] [HideLabelsInEditor]
    public class GetPositionInputAxis : PropertyTypeGetPosition
    {
        [SerializeField] private InputPropertyValueVector2 m_Input;

        public override Vector3 Get(Args args)
        {
            if (!this.m_Input.IsEnabled)
            {
                this.m_Input.OnStartup();
                this.m_Input.Enable();
            }
            
            this.m_Input.OnUpdate();
            return this.m_Input.Read();
        }

        public GetPositionInputAxis()
        {
            InputValueVector2MotionPrimary source = new InputValueVector2MotionPrimary();
            this.m_Input = new InputPropertyValueVector2(source);
        }
        
        ~GetPositionInputAxis()
        {
            if (!this.m_Input.IsEnabled) return;
            
            this.m_Input.Disable();
            this.m_Input.OnDispose();
        }
        
        public static PropertyGetPosition Create() => new PropertyGetPosition(
            new GetPositionInputAxis()
        );

        public override string String => "Input Axis";
    }
}