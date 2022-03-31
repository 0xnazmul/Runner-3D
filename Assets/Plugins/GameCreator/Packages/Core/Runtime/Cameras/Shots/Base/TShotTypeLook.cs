using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Serializable]
    public abstract class TShotTypeLook : TShotType
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private ShotSystemLook m_ShotSystemLook;

        // PROPERTIES: ----------------------------------------------------------------------------

        public ShotSystemLook Look
        {
            get => this.m_ShotSystemLook;
            protected set => this.m_ShotSystemLook = value;
        }

        public override Args Args
        {
            get
            {
                this.m_Args ??= new Args(this.m_ShotCamera, null);
                this.m_Args.ChangeTarget(this.Look.GetLookTarget(this));
        
                return this.m_Args;
            }
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TShotTypeLook() : base()
        {
            this.m_ShotSystemLook = new ShotSystemLook();
        }
        
        // MAIN METHODS: --------------------------------------------------------------------------

        public override void Awake(ShotCamera shotCamera)
        {
            base.Awake(shotCamera);
            
            this.Look.OnAwake(this);
            this.OnAwake(shotCamera);
        }

        public override void Start(ShotCamera shotCamera)
        {
            base.Start(shotCamera);
            
            this.Look.OnStart(this);
            this.OnStart(shotCamera);
        }

        public override void Destroy(ShotCamera shotCamera)
        {
            base.Destroy(shotCamera);
            
            this.Look.OnDestroy(this);
            this.OnDestroy(shotCamera);
        }

        public override void Update()
        {
            base.Update();
            
            this.OnUpdate();
            this.Look.OnUpdate(this);
        }

        // INTERFACE METHODS: ---------------------------------------------------------------------

        public override void OnAwake(ShotCamera shotCamera)
        { }

        public override void OnStart(ShotCamera shotCamera)
        { }
        
        public override void OnDestroy(ShotCamera shotCamera)
        { }
        
        public override void OnUpdate()
        { }

        public override void OnDisable(TCamera camera)
        {
            base.OnDisable(camera);
            this.Look.OnDisable(this, camera);
        }

        public override void OnEnable(TCamera camera)
        {
            base.OnEnable(camera);
            this.Look.OnEnable(this, camera);
        }

        // GIZMOS: --------------------------------------------------------------------------------

        public override void DrawGizmos()
        {
            if (!Application.isPlaying) return;
            this.Look.OnDrawGizmos(this);
        }

        public override void DrawGizmosSelected()
        {
            if (!Application.isPlaying) return;
            this.Look.OnDrawGizmosSelected(this);
        }
    }
}