using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Euler Angle")]
    [Category("Euler Angle")]
    
    [Image(typeof(IconRotation), ColorTheme.Type.Green)]
    [Description("A direction defined by the rotation of each axis")]

    [Serializable] [HideLabelsInEditor]
    public class GetDirectionEulerAngle : PropertyTypeGetDirection
    {
        [SerializeField] protected Vector3 m_AngleAxis = new Vector3(0f, 180f, 0f);

        public GetDirectionEulerAngle()
        { }
        
        public GetDirectionEulerAngle(Vector3 angleAxis)
        {
            this.m_AngleAxis = angleAxis;
        }

        public override Vector3 Get(Args args) => 
            Quaternion.Euler(this.m_AngleAxis) * Vector3.forward;

        public static PropertyGetDirection Create(Vector3 angleAxis) => new PropertyGetDirection(
            new GetDirectionEulerAngle(angleAxis)
        );

        public override string String => $"{this.m_AngleAxis}º";
    }
}