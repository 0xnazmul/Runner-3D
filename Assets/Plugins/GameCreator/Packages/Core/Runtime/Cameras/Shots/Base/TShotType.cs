using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Serializable]
    public abstract class TShotType : IShotType
    {
        protected ShotCamera m_ShotCamera;
        protected Transform m_Transform;

        protected Args m_Args;

        // PROPERTIES: ----------------------------------------------------------------------------

        public virtual Vector3 Position
        {
            get => this.m_Transform.position;
            set => this.m_Transform.position = value;
        }

        public virtual Quaternion Rotation
        {
            get => this.m_Transform.rotation;
            set => this.m_Transform.rotation = value;
        }

        public bool IsActive { get; private set; }

        public abstract Args Args { get; }

        public ShotCamera ShotCamera => this.m_ShotCamera;

        public virtual bool UseSmoothPosition => true;
        public virtual bool UseSmoothRotation => true;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TShotType()
        { }
        
        // MAIN METHODS: --------------------------------------------------------------------------

        public virtual void Awake(ShotCamera shotCamera)
        {
            this.m_ShotCamera = shotCamera;
            this.m_Args = new Args(this.m_ShotCamera);
            
            this.m_Transform = this.m_ShotCamera.transform;
        }

        public virtual void Start(ShotCamera shotCamera)
        { }

        public virtual void Destroy(ShotCamera shotCamera)
        { }
        
        public virtual void Update()
        { }

        // INTERFACE METHODS: ---------------------------------------------------------------------

        public virtual void OnAwake(ShotCamera shotCamera)
        { }

        public virtual void OnStart(ShotCamera shotCamera)
        { }
        
        public virtual void OnDestroy(ShotCamera shotCamera)
        { }
        
        public virtual void OnUpdate()
        { }

        public virtual void OnDisable(TCamera camera)
        {
            this.IsActive = false;
        }

        public virtual void OnEnable(TCamera camera)
        {
            this.IsActive = true;
        }

        // GIZMOS: --------------------------------------------------------------------------------

        public virtual void DrawGizmos()
        { }

        public virtual void DrawGizmosSelected()
        { }
    }
}