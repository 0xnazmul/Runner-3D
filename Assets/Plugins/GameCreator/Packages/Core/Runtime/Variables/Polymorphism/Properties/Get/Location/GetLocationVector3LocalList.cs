using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Vector3 Local List Variable")]
    [Category("Variables/Vector3 Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Vector3 as the position value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetLocationVector3LocalList : PropertyTypeGetLocation
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueVector3.TYPE_ID);

        public override Location Get(Args args) => new Location(this.m_Variable.Get<Vector3>());
        public override Location Get(GameObject gameObject) => new Location(this.m_Variable.Get<Vector3>());

        public override string String => this.m_Variable.ToString();
    }
}