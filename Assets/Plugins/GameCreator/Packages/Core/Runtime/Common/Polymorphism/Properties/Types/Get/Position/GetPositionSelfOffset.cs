using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Self")]
    [Category("Self")]
    
    [Image(typeof(IconSelf), ColorTheme.Type.Yellow)]
    [Description("Returns the position of the caller plus an offset in local space")]

    [Serializable] [HideLabelsInEditor]
    public class GetPositionSelfOffset : PropertyTypeGetPosition
    {
        [SerializeField] protected Vector3 m_LocalOffset;

        public override Vector3 Get(Args args)
        {
            if (args.Self == null) return default;
            return args.Self.transform.position +
                   args.Self.transform.TransformDirection(this.m_LocalOffset);
        }

        public GetPositionSelfOffset()
        {
            this.m_LocalOffset = Vector3.zero;
        }
        
        public GetPositionSelfOffset(Vector3 offset)
        {
            this.m_LocalOffset = offset;
        }
        
        public static PropertyGetPosition Create(Vector3 position) => new PropertyGetPosition(
            new GetPositionSelfOffset(position)
        );

        public override string String => "Self";
    }
}