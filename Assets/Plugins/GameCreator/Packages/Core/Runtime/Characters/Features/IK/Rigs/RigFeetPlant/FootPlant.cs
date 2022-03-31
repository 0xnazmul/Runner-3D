using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters.IK
{
    internal class FootPlant
    {
        private const int RAYCAST_FIXED_SIZE = 10;
        
        private const float RANGE_FEET_UP = 0.2f;
        private const float RANGE_FEET_DOWN = 0.2f;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private readonly RaycastHit[] m_RaycastHitsBuffer = new RaycastHit[RAYCAST_FIXED_SIZE];
        
        private Transform m_BoneTransform;
        
        private readonly AnimFloat m_WeightPosition = new AnimFloat(0f, 0.25f);
        private readonly AnimFloat m_WeightRotation = new AnimFloat(0f, 0.25f);

        // PROPERTIES: ----------------------------------------------------------------------------

        private HumanBodyBones Bone { get; }
        private AvatarIKGoal AvatarIK { get; }
        
        private RigFeetPlant RigFeetPlant { get; }

        private IUnitDriver Driver => this.RigFeetPlant.Character.Driver;
        private IUnitMotion Motion => this.RigFeetPlant.Character.Motion;

        public float OverReach { get; private set; }

        private Transform BoneTransform
        {
            get
            {
                if (this.m_BoneTransform == null)
                {
                    Animator animator = this.RigFeetPlant.Animator;
                    this.m_BoneTransform = animator.GetBoneTransform(this.Bone);
                }

                return this.m_BoneTransform;
            }
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public FootPlant(HumanBodyBones bone, AvatarIKGoal avatarIK, RigFeetPlant rigFeetPlant)
        {
            this.Bone = bone;
            this.AvatarIK = avatarIK;
            this.RigFeetPlant = rigFeetPlant;

            this.RigFeetPlant.Character.EventAfterChangeModel += this.RegisterAnimatorIK;
            this.RegisterAnimatorIK();
        }

        private void RegisterAnimatorIK()
        {
            this.RigFeetPlant.Character.Animim.EventOnAnimatorIK -= this.OnAnimatorIK;
            this.RigFeetPlant.Character.Animim.EventOnAnimatorIK += this.OnAnimatorIK;
        }
        
        // CALLBACKS: -----------------------------------------------------------------------------

        private void OnAnimatorIK(int layerIndex)
        {
            Animator animator = this.RigFeetPlant.Animator;
            if (animator == null) return;

            Vector3 bonePreprocessPosition = animator.GetIKPosition(this.AvatarIK);
            Quaternion bonePreprocessRotation = animator.GetIKRotation(this.AvatarIK);

            float feetRangeUp = this.RigFeetPlant.Character.Motion.Height * RANGE_FEET_UP;
            float feetRangeDown = this.RigFeetPlant.Character.Motion.Height * RANGE_FEET_DOWN;
            
            int hitCount = Physics.RaycastNonAlloc(
                new Vector3(
                    bonePreprocessPosition.x,
                    this.RigFeetPlant.Character.Feet.y + feetRangeUp,
                    bonePreprocessPosition.z
                ),
                Vector3.down,
                this.m_RaycastHitsBuffer,
                feetRangeUp + feetRangeDown,
                this.RigFeetPlant.FootMask,
                QueryTriggerInteraction.Ignore
            );

            Vector3 targetPosition = bonePreprocessPosition;
            Quaternion targetRotation = bonePreprocessRotation;
            
            float minDistance = Mathf.Infinity;
            
            for (int i = 0; i < hitCount; ++i)
            {
                RaycastHit hit = this.m_RaycastHitsBuffer[i];
                if (hit.distance > minDistance) continue;

                float offset = this.RigFeetPlant.FootOffset + this.Driver.SkinWidth;
                targetPosition = hit.point + Vector3.up * offset;

                Quaternion forward = this.RigFeetPlant.Character.transform.rotation;
                targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * forward;

                minDistance = hit.distance;
            }

            if (hitCount == 0)
            {
                targetPosition = bonePreprocessPosition;
                targetRotation = bonePreprocessRotation;
                
                this.m_WeightPosition.Target = 0f;
                this.m_WeightRotation.Target = 0f;
            }
            else
            {
                Character character = this.RigFeetPlant.Character;
                bool gestureWithRM = character.Gestures.IsPlaying &&
                                     character.Gestures.RootMotion > 0.5f;
                
                this.m_WeightPosition.Target = this.RigFeetPlant.IsActive && !gestureWithRM ? 1f : 0f;
                this.m_WeightRotation.Target = this.RigFeetPlant.IsActive && !gestureWithRM ? 1f : 0f;
                
                float speedThreshold = this.Motion.LinearSpeed * RigFeetPlant.IDLE_SPEED_THRESHOLD;
                bool isIdle = this.Driver.WorldMoveDirection.magnitude < speedThreshold;

                if (!isIdle && targetPosition.y < this.BoneTransform.position.y)
                {
                    targetPosition = bonePreprocessPosition;
                    targetRotation = bonePreprocessRotation;
                    
                    this.m_WeightPosition.Target = 0f;
                    this.m_WeightRotation.Target = 0f;
                }
            }

            animator.SetIKPosition(this.AvatarIK, targetPosition);
            animator.SetIKPositionWeight(this.AvatarIK, this.m_WeightPosition.Current);

            animator.SetIKRotation(this.AvatarIK, targetRotation);
            animator.SetIKRotationWeight(this.AvatarIK, this.m_WeightRotation.Current);

            float overReach = this.RigFeetPlant.Character.Feet.y - targetPosition.y;
            this.OverReach = Mathf.Max(0f, overReach);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Update()
        {
            float deltaTime = this.RigFeetPlant.Character.Time.DeltaTime;

            this.m_WeightPosition.UpdateWithDelta(deltaTime);
            this.m_WeightRotation.UpdateWithDelta(deltaTime);
        }
    }
}