using System;
using GameCreator.Runtime.Cameras;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Game Creator Camera value of a Local Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetCameraLocalName : PropertyTypeGetCamera
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueGameObject.TYPE_ID);

        public override TCamera Get(Args args)
        {
            GameObject camera = this.m_Variable.Get<GameObject>();
            return camera != null ? camera.Get<TCamera>() : null;
        }

        public override string String => this.m_Variable.ToString();
    }
}