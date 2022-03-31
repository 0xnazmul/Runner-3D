using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Input Direction")]
    [Category("Input/Input Direction")]
    
    [Image(typeof(IconGamepadCross), ColorTheme.Type.Green, typeof(OverlayArrowRight))]
    [Description("Reads the Vector2 value of an input device as a direction vector")]

    [Serializable] [HideLabelsInEditor]
    public class GetRotationInputDirection : PropertyTypeGetRotation
    {
        [SerializeField] private InputPropertyValueVector2 m_Input;

        public override Quaternion Get(Args args)
        {
            if (!this.m_Input.IsEnabled)
            {
                this.m_Input.OnStartup();
                this.m_Input.Enable();
            }
            
            this.m_Input.OnUpdate();
            
            Vector2 value = this.m_Input.Read();
            return value != Vector2.zero
                ? Quaternion.LookRotation(value)
                : Quaternion.identity;
        }

        public GetRotationInputDirection()
        {
            InputValueVector2MotionPrimary source = new InputValueVector2MotionPrimary();
            this.m_Input = new InputPropertyValueVector2(source);
        }
        
        ~GetRotationInputDirection()
        {
            if (!this.m_Input.IsEnabled) return;
            
            this.m_Input.Disable();
            this.m_Input.OnDispose();
        }
        
        public static PropertyGetRotation Create() => new PropertyGetRotation(
            new GetRotationInputDirection()
        );

        public override string String => "Input Direction";
    }
}