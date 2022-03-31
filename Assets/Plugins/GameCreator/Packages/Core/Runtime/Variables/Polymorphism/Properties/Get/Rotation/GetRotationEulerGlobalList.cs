using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Euler Global List Variable")]
    [Category("Variables/Euler Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the euler rotation value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetRotationEulerGlobalList : PropertyTypeGetRotation
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueVector3.TYPE_ID);

        public override Quaternion Get(Args args)
        {
            return Quaternion.Euler(this.m_Variable.Get<Vector3>());
        }

        public override Quaternion Get(GameObject gameObject)
        {
            return Quaternion.Euler(this.m_Variable.Get<Vector3>());
        }

        public override string String => this.m_Variable.ToString();
    }
}