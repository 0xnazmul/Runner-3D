using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Local Target")]
    [Category("Local Target")]
    
    [Image(typeof(IconTarget), ColorTheme.Type.Yellow)]
    [Description("A Vector3 offset in local space from the caller's local space")]

    [Serializable] [HideLabelsInEditor]
    public class GetOffsetLocalTarget : PropertyTypeGetOffset
    {
        [SerializeField] protected Vector3 m_Offset = Vector3.zero;

        public GetOffsetLocalTarget()
        { }

        public GetOffsetLocalTarget(Vector3 offset)
        {
            this.m_Offset = offset;
        }

        public override Vector3 Get(Args args) =>
            args.Target != null ? args.Target.transform.TransformDirection(this.m_Offset)
            : Vector3.zero;

        public override Vector3 Get(GameObject gameObject) => gameObject != null
            ? gameObject.transform.TransformDirection(this.m_Offset)
            : Vector3.zero;

        public static PropertyGetOffset Create() => new PropertyGetOffset(
            new GetOffsetLocalTarget()
        );
        
        public static PropertyGetOffset Create(Vector3 offset) => new PropertyGetOffset(
            new GetOffsetLocalTarget(offset)
        );

        public override string String => this.m_Offset.ToString();
    }
}