using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Serializable]
    public class Footsteps : TPolymorphicList<Bone>
    {
        private const float MIN_STEP_INTERVAL = 0.2f;
        
        private const float RAYCAST_DISTANCE_PERCENTAGE = 0.25f;
        private const int RAYCAST_BUFFER_SIZE = 5;

        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeReference] private Bone[] m_Feet =
        {
            new Bone(HumanBodyBones.LeftFoot),
            new Bone(HumanBodyBones.RightFoot),
        };

        [SerializeField] private float m_GroundThreshold = 0.05f;
        [SerializeField] private MaterialSounds m_FootstepSounds = new MaterialSounds();
        
        // MEMBERS: -------------------------------------------------------------------------------

        private Character m_Character;
        
        private Dictionary<Transform, Footstep> m_Footsteps = new Dictionary<Transform, Footstep>();
        
        private readonly RaycastHit[] m_HitsBuffer = new RaycastHit[RAYCAST_BUFFER_SIZE];

        // PROPERTIES: ----------------------------------------------------------------------------

        public override int Length => this.m_Feet.Length;

        public GameObject LastFootstep { get; private set; }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<Transform> EventStep;

        // INITIALIZE METHODS: --------------------------------------------------------------------
        
        internal void OnStartup(Character character)
        {
            this.m_Character = character;
            this.m_FootstepSounds.OnStartup();
        }
        
        internal void AfterStartup(Character character)
        { }

        internal void OnDispose(Character character)
        {
            this.m_Character = character;
        }

        internal void OnEnable()
        { }

        internal void OnDisable()
        { }

        // UPDATE METHODS: ------------------------------------------------------------------------
        
        internal void OnUpdate()
        {
            Animator animator = this.m_Character.Animim.Animator;
            if (animator == null) return;

            float feetThreshold = this.m_Character.Feet.y + this.m_GroundThreshold;
            
            foreach (IBone foot in m_Feet)
            {
                Transform bone = foot.GetTransform(animator);
                if (bone == null) continue;

                bool isGrounded = bone.position.y <= feetThreshold;

                if (this.m_Character.Driver.IsGrounded &&
                    this.m_Footsteps.TryGetValue(bone, out Footstep footstep))
                {
                    if (isGrounded && !footstep.wasGrounded &&
                        this.m_Character.Time.Time > footstep.stepTime + MIN_STEP_INTERVAL)
                    {
                        this.OnStep(bone);
                        footstep.stepTime = this.m_Character.Time.Time;
                    }

                    footstep.wasGrounded = isGrounded;
                }
                else
                {
                    this.m_Footsteps[bone] = new Footstep
                    {
                        wasGrounded = true,
                        stepTime = this.m_Character.Time.Time
                    };
                }
            }
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void ChangeFootstepSounds(MaterialSoundsAsset materialSoundsAsset)
        {
            this.m_FootstepSounds.ChangeSoundsAsset(materialSoundsAsset);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnStep(Transform bone)
        {
            this.LastFootstep = bone.gameObject;
            this.EventStep?.Invoke(bone);
            
            int numHits = Physics.RaycastNonAlloc(
                bone.position, -this.m_Character.transform.up,
                this.m_HitsBuffer,
                this.m_Character.Motion.Height * RAYCAST_DISTANCE_PERCENTAGE,
                this.m_FootstepSounds.LayerMask,
                QueryTriggerInteraction.Ignore
            );

            RaycastHit hit = new RaycastHit();
            float minDistance = Mathf.Infinity;

            for (int i = 0; i < numHits; ++i)
            {
                float distance = Vector3.Distance(
                    this.m_HitsBuffer[i].transform.position,
                    bone.position
                );

                if (distance > minDistance) continue;
                
                hit = this.m_HitsBuffer[i];
                minDistance = distance;
            }

            if (hit.collider == null) return;
            
            float speed = Mathf.Clamp01(
                this.m_Character.Driver.WorldMoveDirection.magnitude /
                this.m_Character.Motion.LinearSpeed
            );
            Args args = new Args(this.m_Character.gameObject, hit.collider.gameObject);
            this.m_FootstepSounds.Play(bone, hit, speed, args);
        }

        // GIZMOS: --------------------------------------------------------------------------------
        
        internal void OnDrawGizmos(Character character)
        {
            Gizmos.color = Color.blue;
            GizmosExtension.Cylinder(
                character.Feet,
                this.m_GroundThreshold,
                character.Motion.Radius + 0.01f
            );
        }
    }
}
