using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Euler Local List Variable")]
    [Category("Variables/Euler Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the euler rotation value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetRotationEulerLocalList : PropertyTypeGetRotation
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueVector3.TYPE_ID);

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