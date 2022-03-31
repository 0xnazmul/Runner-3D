using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Input Direction")]
    [Category("Input/Input Direction")]
    
    [Image(typeof(IconGamepadCross), ColorTheme.Type.Green, typeof(OverlayArrowRight))]
    [Description("Reads the Vector2 value of an input device as a direction vector")]

    [Serializable] [HideLabelsInEditor]
    public class GetDirectionInputDirection : PropertyTypeGetDirection
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

        public GetDirectionInputDirection()
        {
            InputValueVector2MotionPrimary source = new InputValueVector2MotionPrimary();
            this.m_Input = new InputPropertyValueVector2(source);
        }
        
        ~GetDirectionInputDirection()
        {
            if (!this.m_Input.IsEnabled) return;
            
            this.m_Input.Disable();
            this.m_Input.OnDispose();
        }
        
        public static PropertyGetDirection Create() => new PropertyGetDirection(
            new GetDirectionInputDirection()
        );

        public override string String => "Input Direction";
    }
}