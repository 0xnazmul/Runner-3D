using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters.IK
{
    [Title("Lean with Momentum")]
    [Category("Lean with Momentum")]
    [Image(typeof(IconCharacterWalk), ColorTheme.Type.Green)]
    
    [Description(
        "Forces Characters to lean towards the acceleration direction and towards the opposite " +
        "direction when decelerating"
    )]
    
    [Serializable]
    public class RigLean : TRigAnimatorIK
    {
        // CONSTANTS: -----------------------------------------------------------------------------

        public const string RIG_NAME = "RigLeanMomentum";

        // MEMBERS: -------------------------------------------------------------------------------

        private Vector3 m_LastMoveDirection;
        private LeanSection[] m_LeanSections;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private float m_InclineSpine = 10f;
        [SerializeField] private float m_InclineLowerChest = 5f;
        [SerializeField] private float m_InclineUpperChest = 0f;
        
        [SerializeField] private float m_DeclineSpine = -15f;
        [SerializeField] private float m_DeclineLowerChest = -5f;
        [SerializeField] private float m_DeclineUpperChest = 10f;

        [SerializeField] private float m_RollSpine = 0f;
        [SerializeField] private float m_RollLowerChest = 5f;
        [SerializeField] private float m_RollUpperChest = 10f;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Lean with Momentum";
        
        public override string Name => RIG_NAME;
        
        public override bool RequiresHuman => true;

        // IMPLEMENT METHODS: ---------------------------------------------------------------------

        protected override bool DoStartup(Character character)
        {
            this.m_LeanSections = new[]
            {
                new LeanSection(
                    this, HumanBodyBones.Spine,
                    this.m_RollSpine, this.m_DeclineSpine, this.m_InclineSpine
                ),
                new LeanSection(
                    this, HumanBodyBones.UpperChest, 
                    this.m_RollLowerChest, this.m_DeclineLowerChest, this.m_InclineLowerChest
                ),
                new LeanSection(
                    this, HumanBodyBones.Chest, 
                    this.m_RollUpperChest, this.m_DeclineUpperChest, this.m_InclineUpperChest
                )
            };

            bool updateGraph = base.DoStartup(character);
            return updateGraph;
        }

        protected override bool DoEnable(Character character)
        {
            bool updateGraph = base.DoEnable(character);
            this.m_LastMoveDirection = character.Driver.LocalMoveDirection;
            
            return updateGraph;
        }

        protected override bool DoUpdate(Character character)
        {
            bool updateGraph = base.DoUpdate(character);
        
            float movement = character.Driver.LocalMoveDirection.z - this.m_LastMoveDirection.z;
            movement /= character.Time.DeltaTime;
        
            this.m_LastMoveDirection = character.Driver.LocalMoveDirection;
        
            float ratioZ = movement >= 0f
                ? Mathf.InverseLerp(0f, character.Motion.LinearSpeed, movement)
                : Mathf.InverseLerp(0f, -character.Motion.LinearSpeed, movement);
            
            float ratioX = Mathf.InverseLerp(
                0f, 
                character.Motion.LinearSpeed, 
                Math.Abs(character.Driver.LocalMoveDirection.x)
            );
            
            ratioZ *= Math.Sign(movement);
            ratioX *= Math.Sign(character.Driver.LocalMoveDirection.x);
        
            foreach (LeanSection section in this.m_LeanSections)
            {
                section.Update(ratioZ, ratioX);
            }
            
            return updateGraph;
        }
    }
}