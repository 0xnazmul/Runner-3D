using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters.IK
{
    [Title("Align Feet with Ground")]
    [Category("Align Feet with Ground")]
    [Image(typeof(IconFootprint), ColorTheme.Type.Green)]
    
    [Description(
        "IK system that allows the Character to correctly align their feet to uneven terrain. " +
        "It also avoids character's feet from penetrating the floor. Requires a humanoid character"
    )]
    
    [Serializable]
    public class RigFeetPlant : TRigAnimatorIK
    {
        // CONSTANTS: -----------------------------------------------------------------------------

        public const string RIG_NAME = "RigFeetPlant";

        internal const float IDLE_SPEED_THRESHOLD = 0.25f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private float m_FootOffset = 0f;
        [SerializeField] private LayerMask m_FootMask = -1;
        
        [SerializeField] private bool m_AlignHips = true;

        // MEMBERS: -------------------------------------------------------------------------------

        private FootPlant m_LimbFootL;
        private FootPlant m_LimbFootR;
        
        private AnimFloat m_AlignHipsWeight = new AnimFloat(1f, 0.5f);
        private AnimFloat m_PenetrationWeight = new AnimFloat(1f, 0.25f);

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Align Feet with Ground";
        
        public override string Name => RIG_NAME;
        
        public override bool RequiresHuman => true;

        internal float FootOffset => m_FootOffset;
        internal LayerMask FootMask => this.m_FootMask;

        // IMPLEMENT METHODS: ---------------------------------------------------------------------

        protected override bool DoStartup(Character character)
        {
            this.m_LimbFootL = new FootPlant(HumanBodyBones.LeftFoot,  AvatarIKGoal.LeftFoot,  this);
            this.m_LimbFootR = new FootPlant(HumanBodyBones.RightFoot, AvatarIKGoal.RightFoot, this);

            bool updateGraph = base.DoStartup(character);
            return updateGraph;
        }

        protected override bool DoUpdate(Character character)
        {
            bool updateGraph = base.DoUpdate(character);

            float speedThreshold = this.Character.Motion.LinearSpeed * IDLE_SPEED_THRESHOLD;
            
            bool isIdle = this.Character.Driver.WorldMoveDirection.magnitude < speedThreshold;
            bool isGrounded = this.Character.Driver.IsGrounded;

            float deltaTime = this.Character.Time.DeltaTime;
            float tAlignHips = isIdle && isGrounded && this.IsActive ? 1f : 0f;
            float tPenetrate = !isIdle && isGrounded && this.IsActive ? 1f : 0f;
            
            this.m_AlignHipsWeight.UpdateWithDelta(tAlignHips, deltaTime);
            this.m_PenetrationWeight.UpdateWithDelta(tPenetrate, deltaTime);

            this.m_LimbFootL.Update();
            this.m_LimbFootR.Update();

            if (this.m_AlignHips)
            {
                float overReach = Mathf.Max(
                    this.m_LimbFootL.OverReach,
                    this.m_LimbFootR.OverReach
                );
                
                this.Character.Animim.ModelOffset = -overReach;
            }
            
            return updateGraph;
        }
    }
}