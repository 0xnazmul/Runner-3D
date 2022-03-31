using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Instantiate")]
    [Description("Creates a new instance of a referenced game object")]

    [Category("Game Objects/Instantiate")]
    
    [Parameter("Game Object", "Game Object reference that is instantiated")]
    [Parameter("Position", "The position where the new game object is instantiated")]
    [Parameter("Rotation", "The rotation that the new game object has")]
    [Parameter("Save", "Optional value where the newly instantiated game object is stored")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    
    [Keywords("Create", "New", "Game Object")]
    [Serializable]
    public class InstructionGameObjectInstantiate : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] 
        private PropertyGetInstantiate m_GameObject = new PropertyGetInstantiate();

        [SerializeField] 
        private PropertyGetLocation m_Location = GetLocationCharactersPlayer.Create;

        [SerializeField] 
        private PropertyGetGameObject m_Parent = GetGameObjectNone.Create();

        [SerializeField]
        private PropertySetGameObject m_Save = SetGameObjectNone.Create;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Instantiate {this.m_GameObject}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Location location = this.m_Location.Get(args);

            Vector3 position = location.HasPosition
                ? location.Position
                : args.Self != null ? args.Self.transform.position : Vector3.zero;
            
            Quaternion rotation = location.HasRotation
                ? location.Rotation
                : args.Self != null ? args.Self.transform.rotation : Quaternion.identity;
            
            GameObject instance = this.m_GameObject.Get(args, position, rotation);

            if (instance != null)
            {
                Transform parent = this.m_Parent.Get<Transform>(args);
                if (parent != null) instance.transform.SetParent(parent);
                
                this.m_Save.Set(instance, args);
            }

            return DefaultResult;
        }
    }
}