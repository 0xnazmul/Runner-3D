using System;
using System.Collections.Generic;
using System.Xml;
using GameCreator.Runtime.Characters.Animim;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [DisallowMultipleComponent]
    [HelpURL("https://docs.gamecreator.io/gamecreator/characters")]
    
    [AddComponentMenu("Game Creator/Characters/Character")]
    [Icon(RuntimePaths.GIZMOS + "GizmoCharacter.png")]
    public class Character : MonoBehaviour, ISpatialHash
    {
        public enum MovementType
        {
            None,
            MoveToDirection,
            MoveToPosition,
        }

        // CONSTANTS: -----------------------------------------------------------------------------

        public const float BIG_EPSILON = 0.01f;
        
        // STATIC: --------------------------------------------------------------------------------

        private static Dictionary<IdString, Character> Characters;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemsInit()
        {
            Characters = new Dictionary<IdString, Character>();
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] protected bool m_IsPlayer;
        [SerializeField] protected TimeMode m_Time;
        
        [SerializeField] protected Busy m_Busy = new Busy();

        [SerializeField] protected CharacterKernel m_Kernel = new CharacterKernel();
        
        [SerializeField] protected AnimimGraph m_AnimimGraph = new AnimimGraph();
        [SerializeField] protected InverseKinematics m_InverseKinematics = new InverseKinematics();
        
        [SerializeField] protected Footsteps m_Footsteps = new Footsteps();
        [SerializeField] protected Ragdoll m_Ragdoll = new Ragdoll();
        [SerializeField] protected Props m_Props = new Props();
        [SerializeField] protected Jump m_Jump = new Jump();

        [SerializeField] protected UniqueID m_UniqueID = new UniqueID();
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private bool m_IsDead;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Busy Busy => this.m_Busy;
        public TimeMode Time => this.m_Time;
        public IdString ID => this.m_UniqueID.Get;

        public bool IsPlayer
        {
            get => this.m_IsPlayer;
            set
            {
                ShortcutPlayer.Change(value ? this : null);
                this.m_IsPlayer = value;
            }
        }

        public bool IsDead
        {
            get => this.m_IsDead;
            set
            {
                if (this.m_IsDead == value) return;
                this.m_IsDead = value;

                switch (this.m_IsDead)
                {
                    case true:  this.EventDie?.Invoke(); break;
                    case false: this.EventRevive?.Invoke(); break;
                }
            }
        }

        public CharacterKernel Kernel => this.m_Kernel;
        public InverseKinematics IK => this.m_InverseKinematics;

        public Footsteps Footsteps => m_Footsteps;
        public Ragdoll Ragdoll => this.m_Ragdoll;
        public Props Props => this.m_Props;
        public Jump Jump => this.m_Jump;

        public StatesOutput States => this.m_AnimimGraph.States;
        public GesturesOutput Gestures => this.m_AnimimGraph.Gestures;
        public float RootMotion => this.m_AnimimGraph.RootMotion;

        public IUnitPlayer Player => this.m_Kernel?.Player;
        public IUnitMotion Motion => this.m_Kernel?.Motion;
        public IUnitDriver Driver => this.m_Kernel?.Driver;
        public IUnitFacing Facing => this.m_Kernel?.Facing;
        public IUnitAnimim Animim => this.m_Kernel?.Animim;
        
        public Vector3 Eyes => this.Animim.Animator != null && this.Animim.Animator.isHuman
            ? this.Animim.Animator.GetBoneTransform(HumanBodyBones.Head).position
            : this.transform.position + Vector3.up * this.Motion.Height / 2f;
        
        public Vector3 Feet => this.transform.position - Vector3.up * this.Motion.Height / 2f;
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventEnable;
        public event Action EventDisable;

        public event Action EventBeforeUpdate;
        public event Action EventAfterUpdate;
        
        public event Action EventBeforeLateUpdate;
        public event Action EventAfterLateUpdate;
        
        public event Action EventBeforeFixedUpdate;
        public event Action EventAfterFixedUpdate;

        public event Action EventDie;
        public event Action EventRevive;

        public event Action<float> EventLand;
        public event Action<float> EventJump;

        public event Action EventBeforeChangeModel;
        public event Action EventAfterChangeModel;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        protected virtual void Awake()
        {
            if (this.IsPlayer) ShortcutPlayer.Change(this);
            
            this.m_Busy?.OnStartup(this);
            this.m_Kernel?.OnStartup(this);
            this.m_AnimimGraph?.OnStartup(this);
            this.m_InverseKinematics?.OnStartup(this);
            this.m_Footsteps?.OnStartup(this);
            this.m_Ragdoll?.OnStartup(this);
            this.m_Props?.OnStartup(this);
            this.m_Jump?.OnStartup(this);
            
            SpatialHashCharacters.Get.Insert(this);
            Characters[this.m_UniqueID.Get] = this;
        }

        protected void Start()
        {
            this.m_Busy?.AfterStartup(this);
            this.m_Kernel?.AfterStartup(this);
            this.m_AnimimGraph?.AfterStartup(this);
            this.m_InverseKinematics?.AfterStartup(this);
            this.m_Footsteps?.AfterStartup(this);
            this.m_Ragdoll?.AfterStartup(this);
            this.m_Props?.AfterStartup(this);
            this.m_Jump?.AfterStartup(this);
        }

        protected virtual void OnDestroy()
        {
            this.m_Kernel?.OnDispose(this);
            this.m_AnimimGraph?.OnDispose(this);
            this.m_Footsteps?.OnDispose(this);
            this.m_Ragdoll?.OnDispose(this);
            this.m_Props?.OnDispose(this);
            this.m_Jump?.OnDispose(this);
            
            SpatialHashCharacters.Get.Remove(this);
            Characters.Remove(this.m_UniqueID.Get);
        }

        protected virtual void OnEnable()
        {
            this.m_Kernel?.OnEnable();
            this.m_InverseKinematics?.OnEnable();
            this.m_Footsteps?.OnEnable();
            this.m_Ragdoll?.OnEnable();
            this.m_Props?.OnEnable();
            this.m_Jump?.OnEnable();
            
            this.EventEnable?.Invoke();
        }

        protected virtual void OnDisable()
        {
            this.m_Kernel?.OnDisable();
            this.m_InverseKinematics?.OnDisable();
            this.m_Footsteps?.OnDisable();
            this.m_Ragdoll?.OnDisable();
            this.m_Props?.OnDisable();
            this.m_Jump?.OnDisable();
            
            this.EventDisable?.Invoke();
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        protected virtual void Update()
        {
            this.EventBeforeUpdate?.Invoke();

            this.m_Kernel?.OnUpdate();
            this.m_InverseKinematics?.OnUpdate();
            this.Footsteps?.OnUpdate();
            
            SpatialHashCharacters.Get.Update(this);

            this.EventAfterUpdate?.Invoke();
        }

        protected void LateUpdate()
        {
            this.EventBeforeLateUpdate?.Invoke();
            
            this.m_Ragdoll.OnLateUpdate();
            
            this.EventAfterLateUpdate?.Invoke();
        }

        protected void FixedUpdate()
        {
            this.EventBeforeFixedUpdate?.Invoke();

            this.m_Kernel?.OnFixedUpdate();

            this.EventAfterFixedUpdate?.Invoke();
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected virtual void OnDrawGizmosSelected()
        {
            this.m_Kernel?.OnDrawGizmos(this);
            this.m_InverseKinematics?.OnDrawGizmos(this);
            this.m_Footsteps?.OnDrawGizmos(this);
        }

        // CALLBACKS: -----------------------------------------------------------------------------

        public void OnLand(float velocity)
        {
            if (!this.Busy.AreLegsBusy)
            {
                _ = this.Busy.Timeout(Busy.Limb.Legs, 0.2f);   
            }
            
            this.EventLand?.Invoke(velocity);
        }

        public void OnJump(float force)
        {
            if (!this.Busy.AreLegsBusy)
            {
                _ = this.Busy.Timeout(Busy.Limb.Legs, 0.2f);   
            }
            
            this.EventJump?.Invoke(force);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public struct ChangeOptions
        {
            public Skeleton skeleton;
            public MaterialSoundsAsset materials;
            public RuntimeAnimatorController controller;
            public Vector3 offset;
        }
        
        /// <summary>
        /// Change the 3D character model.
        /// </summary>
        /// <param name="prefab">A prefab representing the character model</param>
        /// <param name="options">[Optional] Default features added to the new model</param>
        /// <returns>The instance of the new model used</returns>
        public GameObject ChangeModel(GameObject prefab, ChangeOptions options)
        {
            if (prefab == null) return null;
            
            this.EventBeforeChangeModel?.Invoke();

            if (this.Animim.Animator != null)
            {
                Destroy(this.Animim.Animator.gameObject);
            }

            GameObject model = Instantiate(prefab, this.transform);
            Vector3 position = Vector3.down * (this.Motion.Height * 0.5f);

            model.name = prefab.name;
            model.transform.localPosition = position + options.offset;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;

            Animator modelAnimator = model.GetComponentInChildren<Animator>(true);
            this.Animim.Animator = modelAnimator;
            
            if (modelAnimator != null && options.controller != null)
            {
                modelAnimator.runtimeAnimatorController = options.controller;
            }

            if (options.skeleton != null)
            {
                if (this.Animim.BoneRack != null)
                {
                    this.Animim.BoneRack.Skeleton = options.skeleton;
                }
            }

            if (options.materials != null)
            {
                this.Footsteps.ChangeFootstepSounds(options.materials);
            }
            
            this.EventAfterChangeModel?.Invoke();
            return model;
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static Character GetCharacterByID(string characterID)
        {
            IdString id = new IdString(characterID);
            return GetCharacterByID(id);
        }
        
        public static Character GetCharacterByID(IdString characterID)
        {
            return Characters.TryGetValue(characterID, out Character character)
                ? character
                : null;
        }

        // INTERFACE SPATIAL HASH: ----------------------------------------------------------------

        Vector3 ISpatialHash.Position => this.transform.position;
        int ISpatialHash.UniqueCode => this.gameObject.GetInstanceID();
    }   
}
