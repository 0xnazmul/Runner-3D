using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Local Self")]
    [Category("Local Self")]
    
    [Image(typeof(IconSelf), ColorTheme.Type.Yellow)]
    [Description("A Vector3 offset in local space from the targeted game object's local space")]

    [Serializable] [HideLabelsInEditor]
    public class GetOffsetLocalSelf : PropertyTypeGetOffset
    {
        [SerializeField] protected Vector3 m_Offset = Vector3.zero;
        
        public GetOffsetLocalSelf()
        { }

        public GetOffsetLocalSelf(Vector3 offset)
        {
            this.m_Offset = offset;
        }

        public override Vector3 Get(Args args) => args.Self != null
            ? args.Self.transform.TransformDirection(this.m_Offset)
            : Vector3.zero;

        public override Vector3 Get(GameObject gameObject) => gameObject != null
            ? gameObject.transform.TransformDirection(this.m_Offset)
            : Vector3.zero;

        public static PropertyGetOffset Create(Vector3 offset) => new PropertyGetOffset(
            new GetOffsetLocalSelf(offset)
        );
        
        public static PropertyGetOffset Create() => new PropertyGetOffset(
            new GetOffsetLocalSelf()
        );

        public override string String => this.m_Offset.ToString();
    }
}