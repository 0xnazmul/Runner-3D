using System;
using GameCreator.Runtime.Cameras;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Game Creator Camera value of a Global Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetCameraGlobalName : PropertyTypeGetCamera
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueGameObject.TYPE_ID);

        public override TCamera Get(Args args)
        {
            GameObject camera = this.m_Variable.Get<GameObject>();
            return camera != null ? camera.Get<TCamera>() : null;
        }

        public override string String => this.m_Variable.ToString();
    }
}