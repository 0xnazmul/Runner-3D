using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Game Object Global Name Variable")]
    [Category("Variables/Game Object Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Game Object value of a Global Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetLocationObjectGlobalName : PropertyTypeGetLocation
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueGameObject.TYPE_ID);

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