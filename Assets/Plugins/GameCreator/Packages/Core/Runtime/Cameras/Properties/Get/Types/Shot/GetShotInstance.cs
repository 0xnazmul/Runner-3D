using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Cameras
{
    [Title("Shot")]
    [Category("Shot")]
    
    [Image(typeof(IconCameraShot), ColorTheme.Type.Yellow)]
    [Description("Reference to a Camera Shot game object")]

    [Serializable] [HideLabelsInEditor]
    public class GetShotInstance : PropertyTypeGetShot
    {
        [SerializeField] protected ShotCamera m_CameraShot;

        public override ShotCamera Get(Args args) => this.m_CameraShot;
        public override ShotCamera Get(GameObject gameObject) => this.m_CameraShot;

        public static PropertyGetShot Create => new PropertyGetShot(
            new GetShotInstance()
        );

        public override string String => this.m_CameraShot != null
            ? this.m_CameraShot.gameObject.name
            : "(none)";
    }
}