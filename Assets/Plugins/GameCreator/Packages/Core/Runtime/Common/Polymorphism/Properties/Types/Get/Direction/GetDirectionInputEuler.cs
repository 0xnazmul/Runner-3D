using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Input Euler")]
    [Category("Input/Input Euler")]
    
    [Image(typeof(IconGamepadCross), ColorTheme.Type.Green, typeof(OverlayDot))]
    [Description("Reads the Vector2 value of an input device as euler degrees")]

    [Serializable] [HideLabelsInEditor]
    public class GetDirectionInputEuler : PropertyTypeGetDirection
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
            Vector2 value = this.m_Input.Read();
            return Quaternion.Euler(value) * Vector3.forward;
        }

        public GetDirectionInputEuler()
        {
            InputValueVector2MotionPrimary source = new InputValueVector2MotionPrimary();
            this.m_Input = new InputPropertyValueVector2(source);
        }
        
        ~GetDirectionInputEuler()
        {
            if (!this.m_Input.IsEnabled) return;
            
            this.m_Input.Disable();
            this.m_Input.OnDispose();
        }
        
        public static PropertyGetDirection Create() => new PropertyGetDirection(
            new GetDirectionInputEuler()
        );

        public override string String => "Input Euler";
    }
}