using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Transform Offset")]
    [Category("Transforms/Transform Offset")]
    
    [Image(typeof(IconVector3), ColorTheme.Type.Green)]
    [Description("Transforms the local space point to world space and returns the value")]

    [Serializable] [HideLabelsInEditor]
    public class GePositionTransformOffset : PropertyTypeGetPosition
    {
        [SerializeField] protected PropertyGetGameObject m_From = GetGameObjectPlayer.Create();
        [SerializeField] protected Vector3 m_Point;

        public override Vector3 Get(Args args)
        {
            GameObject from = this.m_From.Get(args);
            if (from == null) return Vector3.zero;
            
            return from.transform.TransformPoint(this.m_Point);
        }

        public GePositionTransformOffset()
        {
            this.m_Point = Vector3.zero;
        }
        
        public GePositionTransformOffset(Vector3 point)
        {
            this.m_Point = point;
        }
        
        public static PropertyGetPosition Create(Vector3 point) => new PropertyGetPosition(
            new GePositionTransformOffset(point)
        );

        public override string String => $"{this.m_From} {this.m_Point.ToString()}";
    }
}