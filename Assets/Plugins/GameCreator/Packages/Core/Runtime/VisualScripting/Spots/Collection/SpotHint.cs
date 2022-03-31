using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Hint Object")]
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue)]
    
    [Category("Hint Object")]
    [Description(
        "Creates or Activates a prefab game object when the Hotspot is enabled and " +
        "deactivates it when the Hotspot is disabled"
    )]

    [Serializable]
    public class SpotHint : Spot
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] protected GameObject m_Prefab;
        [SerializeField] protected Vector3 m_Offset = Vector3.zero;

        // MEMBERS: -------------------------------------------------------------------------------
        
        private GameObject m_Hint;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "Show {0} when near",
            this.m_Prefab != null ? this.m_Prefab.name : "(none)"
        );

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override void OnUpdate(Hotspot hotspot)
        {
            base.OnUpdate(hotspot);

            GameObject instance = this.RequireInstance(hotspot);
            if (instance == null) return;

            instance.transform.SetPositionAndRotation(
                hotspot.Position + hotspot.transform.TransformDirection(this.m_Offset),
                hotspot.Rotation
            );
            
            instance.SetActive(hotspot.IsActive);
        }

        public override void OnDisable(Hotspot hotspot)
        {
            base.OnDisable(hotspot);
            if (this.m_Hint != null) this.m_Hint.SetActive(false);
        }

        public override void OnDestroy(Hotspot hotspot)
        {
            base.OnDestroy(hotspot);
            
            if (this.m_Hint != null)
            {
                UnityEngine.Object.Destroy(this.m_Hint);
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private GameObject RequireInstance(Hotspot hotspot)
        {
            if (this.m_Hint == null)
            {
                if (this.m_Prefab == null) return null;
                this.m_Hint = UnityEngine.Object.Instantiate(
                    this.m_Prefab,
                    hotspot.Position + hotspot.transform.TransformDirection(this.m_Offset),
                    hotspot.Rotation
                );

                this.m_Hint.hideFlags = HideFlags.HideAndDontSave;
            }

            return this.m_Hint;
        }
    }
}