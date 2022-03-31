using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
	[AddComponentMenu("")]
    [DisallowMultipleComponent]
	public class PoolManager : Singleton<PoolManager>
	{
        private Dictionary<int, PoolData> Collection { get; set; }

        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnCreate()
        {
            base.OnCreate();
            this.Collection = new Dictionary<int, PoolData>();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public GameObject Pick(GameObject prefab, int count, float duration = -1f)
        {
            if (prefab == null) return null;
            int instanceID = prefab.GetInstanceID();

            if (!this.Collection.ContainsKey(instanceID)) this.CreatePool(prefab, count, duration);
            return this.Collection[instanceID].Get();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void CreatePool(GameObject prefab, int count, float duration = -1f)
        {
            int instanceID = prefab.GetInstanceID();
            this.Collection.Add(instanceID, new PoolData(prefab, count, duration));
        }
    }
}