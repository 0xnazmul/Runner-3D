using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [AddComponentMenu("Game Creator/Cameras/Shot Camera")]
    [Icon(RuntimePaths.GIZMOS + "GizmoShot.png")]
    public class ShotCamera : MonoBehaviour
    {
        public const string INPUT_CAMERA_DEFAULT_MAP = "Camera";
        public const string INPUT_CAMERA_DEFAULT_PATH = 
            RuntimePaths.CAMERAS + "Input/Default-Camera.inputactions";

        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        protected TimeMode m_TimeMode = new TimeMode();
        
        [SerializeReference]
        protected IShotType m_ShotType = new ShotTypeFixed();

        // PROPERTIES: ----------------------------------------------------------------------------

        public IShotType ShotType => this.m_ShotType;

        public Vector3 Position => this.m_ShotType?.Position ?? transform.position;
        public Quaternion Rotation => this.m_ShotType?.Rotation ?? transform.rotation;
        
        public Transform Target
        {
            get
            {
                ShotSystemLook systemLook = (this.m_ShotType as TShotTypeLook)?.Look;
                return systemLook?.GetLookTarget(this.m_ShotType);
            }
        }

        public virtual bool UseSmoothPosition => this.m_ShotType?.UseSmoothPosition ?? false;
        public virtual bool UseSmoothRotation => this.m_ShotType?.UseSmoothRotation ?? false;

        public TimeMode TimeMode => this.m_TimeMode;

        // INITIALIZERS: --------------------------------------------------------------------------

        protected virtual void Awake()
        {
            this.m_ShotType?.Awake(this);
        }

        protected virtual void Start()
        {
            this.m_ShotType?.Start(this);
        }

        protected void OnDestroy()
        {
            this.m_ShotType?.Destroy(this);
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        protected virtual void Update()
        {
            this.m_ShotType?.Update();
        }

        private void OnDrawGizmos()
        {
            this.m_ShotType?.DrawGizmos();
        }
        
        private void OnDrawGizmosSelected()
        {
            this.m_ShotType?.DrawGizmosSelected();;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual void OnEnableShot(TCamera camera)
        {
            this.m_ShotType?.OnEnable(camera);
        }

        public virtual void OnDisableShot(TCamera camera)
        {
            this.m_ShotType?.OnDisable(camera);
        }
    }
}