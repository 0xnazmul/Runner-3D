using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Euler Local Name Variable")]
    [Category("Variables/Euler Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the euler rotation value of a Local Name Variable")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetRotationEulerLocalName : PropertyTypeGetRotation
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueVector3.TYPE_ID);

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