using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Input Euler")]
    [Category("Input/Input Euler")]
    
    [Image(typeof(IconGamepadCross), ColorTheme.Type.Green, typeof(OverlayDot))]
    [Description("Reads the Vector2 value of an input device as euler degrees")]

    [Serializable] [HideLabelsInEditor]
    public class GetRotationInputEuler : PropertyTypeGetRotation
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
            return Quaternion.Euler(value);
        }

        public GetRotationInputEuler()
        {
            InputValueVector2MotionPrimary source = new InputValueVector2MotionPrimary();
            this.m_Input = new InputPropertyValueVector2(source);
        }
        
        ~GetRotationInputEuler()
        {
            if (!this.m_Input.IsEnabled) return;
            
            this.m_Input.Disable();
            this.m_Input.OnDispose();
        }
        
        public static PropertyGetRotation Create() => new PropertyGetRotation(
            new GetRotationInputEuler()
        );

        public override string String => "Input Euler";
    }
}