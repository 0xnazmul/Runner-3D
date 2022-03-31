using System;
using GameCreator.Runtime.Cameras;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Camera Shot value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetShotLocalList : PropertyTypeGetShot
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueGameObject.TYPE_ID);

        public override ShotCamera Get(Args args)
        {
            GameObject camera = this.m_Variable.Get<GameObject>();
            return camera != null ? camera.Get<ShotCamera>() : null;
        }

        public override string String => this.m_Variable.ToString();
    }
}