using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Vector3")]
    [Category("Vector3")]
    
    [Image(typeof(IconVector3), ColorTheme.Type.Yellow)]
    [Description("An offset in world-space")]

    [Serializable] [HideLabelsInEditor]
    public class GetOffsetVector3 : PropertyTypeGetOffset
    {
        [SerializeField] protected Vector3 m_Offset = Vector3.zero;

        public GetOffsetVector3()
        { }

        public GetOffsetVector3(Vector3 offset)
        {
            this.m_Offset = offset;
        }

        public override Vector3 Get(Args args) => this.m_Offset;
        public override Vector3 Get(GameObject transform) => this.m_Offset;

        public static PropertyGetOffset Create() => new PropertyGetOffset(
            new GetOffsetVector3()
        );
        
        public static PropertyGetOffset Create(Vector3 offset) => new PropertyGetOffset(
            new GetOffsetVector3(offset)
        );

        public override string String => this.m_Offset.ToString();
    }
}