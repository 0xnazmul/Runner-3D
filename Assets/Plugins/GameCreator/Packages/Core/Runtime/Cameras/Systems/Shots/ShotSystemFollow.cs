using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Serializable]
    public class ShotSystemFollow : TShotSystem
    {
        private static readonly Vector3 DEFAULT_DISTANCE = new Vector3(0f, 3f, -5f);
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Follow = GetGameObjectPlayer.Create();
        
        [SerializeField] private PropertyGetOffset m_Distance = new PropertyGetOffset(DEFAULT_DISTANCE);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Transform Follow
        {
            set => this.m_Follow = GetGameObjectInstance.Create(value);
        }
        
        public Vector3 Distance
        {
            set => this.m_Distance = new PropertyGetOffset(value);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void OnUpdate(TShotType shotType)
        {
            base.OnUpdate(shotType);

            if (shotType.IsActive && this.Active)
            {
                GameObject followValue = this.m_Follow.Get(shotType.Args);
                Vector3 distanceValue = this.m_Distance.Get(shotType.Args);
                
                if (followValue != null)
                {
                    shotType.Position = followValue.transform.position + distanceValue;
                }
            }
        }
    }
}