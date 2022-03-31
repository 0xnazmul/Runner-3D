using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Title("Camera Shot")]
    
    public interface IShotType
    {
        bool IsActive { get; }
        
        ShotCamera ShotCamera { get; }

        Vector3 Position { get; }
        Quaternion Rotation { get; }

        bool UseSmoothPosition { get; }
        bool UseSmoothRotation { get; }
        
        // METHODS: -------------------------------------------------------------------------------

        void Awake(ShotCamera shotCamera);
        void Start(ShotCamera shotCamera);
        void Destroy(ShotCamera shotCamera);

        void Update();

        void OnEnable(TCamera camera);
        void OnDisable(TCamera camera);

        void DrawGizmos();
        void DrawGizmosSelected();
    }
}