using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    internal class PoolData
    {
        private const string CONTAINER_NAME = "{0} (pool)";

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly GameObject m_Prefab;
        [NonSerialized] private readonly Transform m_Container;

        [NonSerialized] private readonly List<PoolObject> m_Instances;

        [NonSerialized] private readonly int m_InitCount;
        [NonSerialized] private readonly bool m_HasDuration;
        [NonSerialized] private readonly float m_Duration;

        // INITIALIZER: ---------------------------------------------------------------------------

        public PoolData(GameObject prefab, int count, float duration)
        {
            this.m_Instances = new List<PoolObject>();

            this.m_Prefab = prefab;
            this.m_InitCount = count;

            this.m_HasDuration = duration > 0f;
            this.m_Duration = duration;

            this.m_Container = new GameObject(string.Format(CONTAINER_NAME, prefab.name)).transform;
            this.m_Container.SetParent(PoolManager.Instance.transform);
            this.m_Container.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            this.Rebuild();
        }

        private void Rebuild()
        {
            this.m_Instances.Clear();

            for (int i = 0; i < this.m_InitCount; ++i)
            {
                PoolObject instance = this.CreateEntity();
                this.m_Instances.Add(instance);
            }
        }

        private PoolObject CreateEntity()
        {
            bool prevState = this.m_Prefab.gameObject.activeSelf;
            this.m_Prefab.gameObject.SetActive(false);

            GameObject go = UnityEngine.Object.Instantiate(
                m_Prefab.gameObject,
                this.m_Container,
                true
            );
            
            go.SetActive(false);

            PoolObject instance = go.GetComponent<PoolObject>();
            if (instance == null) instance = go.AddComponent<PoolObject>();

            this.m_Prefab.gameObject.SetActive(prevState);
            return instance;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public GameObject Get()
        {
            int count = this.m_Instances.Count;
            if (count == 0) this.Rebuild();

            PoolObject instance = null;

            for (int i = count - 1; instance == null && i >= 0; --i)
            {
                if (this.m_Instances[i] == null)
                {
                    this.m_Instances.RemoveAt(i);
                    continue;
                }

                if (!this.m_Instances[i].gameObject.activeSelf)
                {
                    instance = this.m_Instances[i];
                }
            }

            if (instance == null)
            {
                instance = this.CreateEntity();
                this.m_Instances.Add(instance);
            }

            instance.enabled = true;
            instance.gameObject.SetActive(true);
            instance.transform.SetParent(this.m_Container);

            if (this.m_HasDuration)
            {
                instance.SetDuration(this.m_Duration);
            }

            return instance.gameObject;
        }
    }
}