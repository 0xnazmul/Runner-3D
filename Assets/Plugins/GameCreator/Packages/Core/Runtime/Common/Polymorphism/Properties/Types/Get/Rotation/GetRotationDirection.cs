using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Direction")]
    [Category("Direction")]
    
    [Image(typeof(IconVector3), ColorTheme.Type.Yellow)]
    [Description("Rotation from a world space direction vector")]

    [Serializable] [HideLabelsInEditor]
    public class GetRotationDirection : PropertyTypeGetRotation
    {
        [SerializeField] protected Vector3 m_Direction;
        
        public GetRotationDirection()
        {
            this.m_Direction = Vector3.zero;
        }
        
        public GetRotationDirection(Vector3 direction) : this()
        {
            this.m_Direction = direction;
        }

        public override Quaternion Get(Args args) => Quaternion.LookRotation(this.m_Direction);
        public override Quaternion Get(GameObject gameObject) => Quaternion.LookRotation(this.m_Direction);

        public static PropertyGetRotation Create => new PropertyGetRotation(
            new GetRotationDirection()
        );
        
        public static PropertyGetRotation CreateForward => new PropertyGetRotation(
            new GetRotationDirection(Vector3.forward)
        );
        
        public static PropertyGetRotation CreateBackward => new PropertyGetRotation(
            new GetRotationDirection(Vector3.back)
        );
        
        public static PropertyGetRotation CreateLeft => new PropertyGetRotation(
            new GetRotationDirection(Vector3.left)
        );
        
        public static PropertyGetRotation CreateRight => new PropertyGetRotation(
            new GetRotationDirection(Vector3.right)
        );

        public override string String => $"Direction {this.m_Direction.ToString()}";
    }
}