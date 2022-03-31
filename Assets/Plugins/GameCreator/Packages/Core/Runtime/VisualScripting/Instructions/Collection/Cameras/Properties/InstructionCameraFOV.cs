using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Change Field of View")]
    [Description("Changes the camera field of view")]

    [Category("Cameras/Properties/Change Field of View")]

    [Parameter("Camera", "The camera component whose property changes")]
    [Parameter("FoV", "The field of view of the camera, measured in degrees")]

    [Keywords("Cameras", "Perspective", "FOV", "3D")]
    [Image(typeof(IconCamera), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class InstructionCameraFOV : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetCamera m_Camera = GetCameraMain.Create;

        [Space] 
        [SerializeField] private PropertyGetDecimal m_FoV = new PropertyGetDecimal(60f);

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Title => $"Change FoV to {this.m_FoV}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            TCamera camera = this.m_Camera.Get(args);
            if (camera == null) return DefaultResult;

            float value = (float) this.m_FoV.Get(args);
            camera.Get<Camera>().fieldOfView = value;
            
            return DefaultResult;
        }
    }
}