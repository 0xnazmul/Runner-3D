using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Cursor")]
    [Image(typeof(IconCursor), ColorTheme.Type.Yellow)]
    
    [Category("Cursor")]
    [Description("Changes the cursor image when hovering the Hotspot")]

    [Serializable]
    public class SpotCursor : Spot
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] protected Texture2D m_Texture;
        [SerializeField] protected Vector2 m_Origin = Vector2.zero;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "Change cursor to {0} when hovering",
            this.m_Texture != null ? this.m_Texture.name : "(none)"
        );
        
        private bool IsPointerHovering { get; set; }

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override void OnUpdate(Hotspot hotspot)
        {
            base.OnUpdate(hotspot);
            this.RefreshCursor(hotspot.IsActive && this.IsPointerHovering);
        }

        public override void OnPointerEnter(Hotspot hotspot)
        {
            base.OnPointerEnter(hotspot);
            this.IsPointerHovering = true;
        }

        public override void OnPointerExit(Hotspot hotspot)
        {
            base.OnPointerExit(hotspot);
            this.IsPointerHovering = false;
        }

        public override void OnDisable(Hotspot hotspot)
        {
            base.OnDisable(hotspot);
            this.IsPointerHovering = false;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshCursor(bool customCursor)
        {
            switch (customCursor)
            {
                case true:
                    Cursor.SetCursor(this.m_Texture, this.m_Origin, CursorMode.Auto);
                    break;
                
                case false:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    break;
            }
        }
    }
}