using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using UnityEngine.InputSystem;

namespace GameCreator.Runtime.Characters
{
    [Title("Directional")]
    [Image(typeof(IconGamepadCross), ColorTheme.Type.Green)]
    
    [Category("Directional")]
    [Description(
        "Moves the Player using a directional input from the Main Camera's perspective"
    )]

    [Serializable]
    public class UnitPlayerDirectional : TUnitPlayer
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private InputPropertyValueVector2 m_InputMove;

        // INITIALIZERS: --------------------------------------------------------------------------

        public UnitPlayerDirectional()
        {
            this.m_InputMove = InputValueVector2MotionPrimary.Create();
        }
        
        // OVERRIDERS: ----------------------------------------------------------------------------

        public override void OnStartup(Character character)
        {
            base.OnStartup(character);
            this.m_InputMove.OnStartup();
        }
        
        public override void OnDispose(Character character)
        {
            base.OnDispose(character);
            this.m_InputMove.OnDispose();
        }
        
        public override void OnEnable()
        {
            base.OnEnable();
            this.m_InputMove.Enable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            this.m_InputMove.Disable();
            
            this.Character.Motion?.MoveToDirection(Vector3.zero, Space.World, 0);
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        public override void OnUpdate()
        {
            base.OnUpdate();
            this.m_InputMove.OnUpdate();

            this.InputDirection = Vector3.zero;
            
            if (!this.Character.IsPlayer || !this.m_IsControllable) return;
            Vector3 inputMovement = this.m_InputMove.Read();
            
            this.InputDirection = this.GetMoveDirection(inputMovement);
            float speed = this.Character.Motion?.LinearSpeed ?? 0f;
            
            this.Character.Motion?.MoveToDirection(this.InputDirection * speed, Space.World, 0);
        }

        protected virtual Vector3 GetMoveDirection(Vector3 input)
        {
            Vector3 direction = new Vector3(input.x, 0f, input.y);

            Vector3 moveDirection = this.Camera != null
                ? this.Camera.TransformDirection(direction)
                : Vector3.zero;

            moveDirection.Scale(Vector3Plane.NormalUp);
            moveDirection.Normalize();

            return moveDirection * direction.magnitude;
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => "Directional";
    }
}