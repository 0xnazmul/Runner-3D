using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class PoolField
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private GameObject m_Prefab;
        
        [SerializeField] private bool m_UsePooling;
        [SerializeField] private int m_Size = 5;
        [SerializeField] private bool m_HasDuration;
        [SerializeField] private float m_Duration = 10f;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public GameObject Create(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (this.m_Prefab == null) return null;
            GameObject instance;

            switch (this.m_UsePooling)
            {
                case true:
                    instance = PoolManager.Instance.Pick(
                        this.m_Prefab, this.m_Size,
                        this.m_HasDuration ? this.m_Duration : -1
                    );

                    instance.transform.SetPositionAndRotation(position, rotation);
                    if (parent != null) instance.transform.SetParent(parent);
                    break;

                case false:
                    instance = UnityEngine.Object.Instantiate(this.m_Prefab, position, rotation, parent);
                    break;
            }

            return instance;
        }
    }
}