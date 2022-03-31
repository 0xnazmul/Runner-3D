using System;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Overlays
{
    // OLD: 
    
    // internal class ToolbarButton : Button
    // {
    //     private Image m_Image;
    //     
    //     // PROPERTIES: ----------------------------------------------------------------------------
    //     
    //     public Image Image
    //     {
    //         set => this.m_Image = value;
    //     }
    //     
    //     // CONSTRUCTOR: ---------------------------------------------------------------------------
    //     
    //     public ToolbarButton(Texture texture, string tooltip, Action onClick)
    //     {
    //         this.m_Image = new Image { image = texture };
    //
    //         this.Add(this.m_Image);
    //         
    //         this.tooltip = tooltip;
    //         this.clicked += onClick;
    //     }
    // }

    // NEW Below:
    
    [EditorToolbarElement(id, typeof(SceneView))]
    internal class ToolbarButton : EditorToolbarButton
    {
        public const string id = "ToolbarButton";
        
        private Image m_Image;
    
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public Image Image
        {
            set => this.m_Image = value;
        }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ToolbarButton() : this(null, "hello world", () => Debug.Log("Hello"))
        { }
        
        public ToolbarButton(Texture texture, string tooltip, Action onClick)
        {
            this.m_Image = new Image { image = texture };
        
            this.Add(this.m_Image);
            
            this.tooltip = tooltip;
            this.clicked += onClick;
        }
    }
}