using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [Serializable]
    public abstract class TLocalVariables : MonoBehaviour, IGameSave
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField]
        protected SaveUniqueID m_SaveUniqueID = new SaveUniqueID();

        // INITIALIZERS: --------------------------------------------------------------------------

        protected virtual void Awake()
        {
            _ = SaveLoadManager.Subscribe(this);
        }
        
        protected virtual void OnDestroy()
        {
            SaveLoadManager.Unsubscribe(this);
        }

        // IGAMESAVE: -----------------------------------------------------------------------------

        public string SaveID => this.m_SaveUniqueID.Get.String;

        public bool IsShared => false;
        public LoadMode LoadMode => LoadMode.Lazy;
        
        public abstract Type SaveType { get; }
        
        public abstract object SaveData { get; }
        public abstract Task OnLoad(object value);
    }
}