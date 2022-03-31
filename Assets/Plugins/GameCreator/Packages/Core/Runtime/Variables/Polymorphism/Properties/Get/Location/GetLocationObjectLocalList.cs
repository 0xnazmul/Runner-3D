using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Game Object Local List Variable")]
    [Category("Variables/Game Object Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Game Object value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetLocationObjectLocalList : PropertyTypeGetLocation
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueGameObject.TYPE_ID);

        public override Location Get(Args args)
        {
            return new Location(this.m_Variable.Get<GameObject>(), Vector3.zero);
        }

        public override Location Get(GameObject gameObject)
        {
            return new Location(this.m_Variable.Get<GameObject>(), Vector3.zero);
        }

        public override string String => this.m_Variable.ToString();
    }
}