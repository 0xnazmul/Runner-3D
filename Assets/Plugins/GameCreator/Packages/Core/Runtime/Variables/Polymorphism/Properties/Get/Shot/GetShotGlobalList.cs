using System;
using GameCreator.Runtime.Cameras;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Camera Shot value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetShotGlobalList : PropertyTypeGetShot
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueGameObject.TYPE_ID);

        public override ShotCamera Get(Args args)
        {
            GameObject gameObject = this.m_Variable.Get<GameObject>();
            return gameObject != null ? gameObject.Get<ShotCamera>() : null;
        }

        public override string String => this.m_Variable.ToString();
    }
}