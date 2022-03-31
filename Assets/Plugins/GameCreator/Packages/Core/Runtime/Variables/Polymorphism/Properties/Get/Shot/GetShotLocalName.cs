using System;
using GameCreator.Runtime.Cameras;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Camera Shot value of a Local Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetShotLocalName : PropertyTypeGetShot
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueGameObject.TYPE_ID);

        public override ShotCamera Get(Args args)
        {
            GameObject camera = this.m_Variable.Get<GameObject>();
            return camera != null ? camera.Get<ShotCamera>() : null;
        }

        public override string String => this.m_Variable.ToString();
    }
}