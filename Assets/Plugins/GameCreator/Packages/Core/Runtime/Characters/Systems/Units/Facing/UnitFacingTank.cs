using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Tank")]
    [Image(typeof(IconTank), ColorTheme.Type.Green)]
    
    [Category("Tank")]
    [Description("Rotates the Character around itself based on the input")]

    [Serializable]
    public class UnitFacingTank : TUnitFacing
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private InputPropertyValueVector2 m_InputMove;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override Type ForcePlayer => typeof(UnitPlayerTank);

        // INITIALIZERS: --------------------------------------------------------------------------

        public UnitFacingTank()
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
        }

        protected override Vector3 GetDefaultDirection()
        {
            this.m_InputMove.OnUpdate();

            if (!this.Character.IsPlayer || !this.Character.Player.IsControllable)
            {
                return this.DecideDirection(Vector3.zero);
            }

            Vector3 inputMovement = this.m_InputMove.Read();
            Vector3 direction = Vector3.Scale(
                this.Transform.TransformDirection(inputMovement),
                Vector3Plane.NormalUp
            );

            return this.DecideDirection(direction);
        }
        
        protected virtual Vector3 GetMoveDirection(Vector3 input)
        {
            Vector3 direction = new Vector3(input.x, 0f, 0f);
            Vector3 moveDirection = this.Transform.TransformDirection(direction);
        
            moveDirection.Scale(Vector3Plane.NormalUp);
            moveDirection.Normalize();
        
            return moveDirection * direction.magnitude;
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => "Tank";
    }
}