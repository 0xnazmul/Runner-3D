using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Target")]
    [Category("Target")]
    
    [Image(typeof(IconTarget), ColorTheme.Type.Yellow)]
    [Description("Returns the position of the targeted object plus an offset in local space")]

    [Serializable] [HideLabelsInEditor]
    public class GetPositionTargetOffset : PropertyTypeGetPosition
    {
        [SerializeField] protected Vector3 m_LocalOffset;

        public override Vector3 Get(Args args)
        {
            if (args.Target == null) return default;
            return args.Target.transform.position +
                   args.Target.transform.TransformDirection(this.m_LocalOffset);
        }

        public GetPositionTargetOffset()
        {
            this.m_LocalOffset = Vector3.zero;
        }
        
        public GetPositionTargetOffset(Vector3 offset)
        {
            this.m_LocalOffset = offset;
        }
        
        public static PropertyGetPosition Create(Vector3 position) => new PropertyGetPosition(
            new GetPositionTargetOffset(position)
        );

        public override string String => "Target";
    }
}