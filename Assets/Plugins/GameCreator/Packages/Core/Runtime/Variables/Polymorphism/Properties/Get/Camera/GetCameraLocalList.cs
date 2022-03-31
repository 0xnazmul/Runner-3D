using System;
using GameCreator.Runtime.Cameras;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Game Creator Camera value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetCameraLocalList : PropertyTypeGetCamera
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueGameObject.TYPE_ID);

        public override TCamera Get(Args args)
        {
            GameObject camera = this.m_Variable.Get<GameObject>();
            return camera != null ? camera.Get<TCamera>() : null;
        }

        public override string String => this.m_Variable.ToString();
    }
}