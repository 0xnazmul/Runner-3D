using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Vector3")]
    [Category("Vector3")]
    
    [Image(typeof(IconVector3), ColorTheme.Type.Green)]
    [Description("A Vector3 that defines a direction")]

    [Serializable] [HideLabelsInEditor]
    public class GetDirectionVector3 : PropertyTypeGetDirection
    {
        [SerializeField] protected Vector3 m_Direction = Vector3.forward;

        public GetDirectionVector3()
        { }
        
        public GetDirectionVector3(Vector3 direction)
        {
            this.m_Direction = direction;
        }

        public override Vector3 Get(Args args) => this.m_Direction;
        public override Vector3 Get(GameObject gameObject) => this.m_Direction;

        public static PropertyGetDirection Create(Vector3 direction) => new PropertyGetDirection(
            new GetDirectionVector3(direction)
        );

        public override string String => this.m_Direction.ToString();
    }
}