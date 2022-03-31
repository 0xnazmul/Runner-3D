using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameCreator.Runtime.Common.SaveSystem;

namespace GameCreator.Runtime.Common
{
    public enum LoadMode
    {
        /// <summary>
        /// Lazy loading disables firing the OnLoad interface method when the
        /// SaveLoadSystem.Load() is executed. Instead, relies on the object's Start method
        /// to subscribe and load its data when the object is instantiated. This is the most
        /// commonly configuration used for the wide majority of situations. 
        /// </summary>
        Lazy,

        /// <summary>
        /// Greedy loading requires a persistent target (set as DontDestroyOnLoad) and forces
        /// its loading whenever the SaveLoadSystem.Load() method is executed. Commonly
        /// used with objects that follow the Singleton pattern.
        /// </summary>
        Greedy
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////

    [AddComponentMenu("")]
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private const int SLOT_MIN = 1;
        private const int SLOT_MAX = 9999;

        private const string DB_KEY_FORMAT = "data:{0:D4}:{1}";

        // STRUCTS: -------------------------------------------------------------------------------

        private struct Reference
        {
            public IGameSave reference;
            public int priority;
        }

        private struct Value
        {
            public object value;
            public bool isShared;
        }

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Scenes m_Scenes;
        [NonSerialized] private Slots m_Slots;

        [NonSerialized] private Dictionary<string, Reference> m_Subscriptions;
        [NonSerialized] private Dictionary<string, Value> m_Values;

        // PROPERTIES: ----------------------------------------------------------------------------

        public int SlotLoaded { get; private set; } = -1;
        public bool IsGameLoaded => this.SlotLoaded > 0;

        public bool IsSaving { get; private set; }
        public bool IsLoading { get; private set; }
        public bool IsDeleting { get; private set; }

        public IDataStorage DataStorage { get; private set; }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<int> EventBeforeSave;
        public event Action<int> EventAfterSave;

        public event Action<int> EventBeforeLoad;
        public event Action<int> EventAfterLoad;

        public event Action<int> EventBeforeDelete;
        public event Action<int> EventAfterDelete;

        // INITIALIZE: ----------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        protected static void InitializeOnLoad()
        {
            Instance.WakeUp();
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            this.DataStorage = new StoragePlayerPrefs();

            this.m_Subscriptions = new Dictionary<string, Reference>();
            this.m_Values = new Dictionary<string, Value>();
            
            this.m_Scenes = new Scenes();
            this.m_Slots = new Slots();

            _ = Subscribe(this.m_Scenes, 100);
            _ = Subscribe(this.m_Slots, 100);
        }

        // REGISTRY METHODS: ----------------------------------------------------------------------

        public static async Task Subscribe(IGameSave reference, int priority = 0)
        {
            if (ApplicationManager.IsExiting) return;
            
            Instance.m_Subscriptions[reference.SaveID] = new Reference
            {
                reference = reference,
                priority = priority
            };

            switch (reference.LoadMode)
            {
                case LoadMode.Lazy:
                    if (Instance.m_Values.TryGetValue(reference.SaveID, out Value value))
                    {
                        await reference.OnLoad(value.value);
                    }
                    else if (Instance.IsGameLoaded)
                    {
                        await Instance.LoadItem(reference, Instance.SlotLoaded);
                    }
                    break;
                
                case LoadMode.Greedy:
                    if (reference.IsShared)
                    {
                        await Instance.LoadItem(reference, 0);
                    }
                    break;
            }
        }

        public static void Unsubscribe(IGameSave reference)
        {
            if (ApplicationManager.IsExiting) return;
            if (Instance.IsLoading) return;

            Instance.m_Values[reference.SaveID] = new Value
            {
                value = reference.SaveData,
                isShared = reference.IsShared
            };
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool HasSave()
        {
            return this.m_Slots.Count > 0;
        }
        
        public async Task Save(int slot)
        {
            if (this.IsSaving || this.IsLoading || this.IsDeleting) return;

            this.EventBeforeSave?.Invoke(slot);

            this.IsSaving = true;

            foreach (KeyValuePair<string, Reference> item in this.m_Subscriptions)
            {
                if (item.Value.reference == null) continue;

                this.m_Values[item.Value.reference.SaveID] = new Value
                {
                    value = item.Value.reference.SaveData,
                    isShared = item.Value.reference.IsShared
                };
            }

            this.m_Slots.Update(slot, this.m_Values.Keys.ToArray());

            foreach (KeyValuePair<string, Value> item in this.m_Values)
            {
                string key = DatabaseKey(slot, item.Value.isShared, item.Key);
                await this.DataStorage.SetBlob(key, item.Value.value);
            }

            this.IsSaving = false;

            this.EventAfterSave?.Invoke(slot);
        }

        public async Task Load(int slot, Action callback = null)
        {
            if (this.IsSaving || this.IsLoading || this.IsDeleting) return;

            this.EventBeforeLoad?.Invoke(slot);
            
            this.IsLoading = true;
            this.SlotLoaded = slot;
            
            this.m_Values.Clear();

            List<Reference> references = this.m_Subscriptions.Values.ToList();
            references.Sort((a, b) => b.priority.CompareTo(a.priority));

            for (int i = 0; i < references.Count; ++i)
            {
                IGameSave item = references[i].reference;
                if (item == null) continue;
                if (item.LoadMode == LoadMode.Lazy) continue;
                
                await this.LoadItem(references[i].reference, slot);
            }
            
            this.IsLoading = false;

            callback?.Invoke();
            this.EventAfterLoad?.Invoke(slot);
        }

        public async Task LoadLatest(Action callback = null)
        {
            int slot = this.m_Slots.LatestSlot;
            
            if (slot < 0) return;
            await this.Load(slot, callback);
        }

        public async Task Delete(int slot)
        {
            if (this.IsSaving || this.IsLoading || this.IsDeleting) return;

            this.EventBeforeDelete?.Invoke(slot);
            this.IsDeleting = true;

            if (this.m_Slots.TryGetValue(slot, out Slots.Data data))
            {
                for (int i = data.keys.Length - 1; i >= 0; --i)
                {
                    await this.DataStorage.DeleteKey(data.keys[i]);
                }

                this.m_Slots.Remove(slot);

                string key = DatabaseKey(slot, this.m_Slots.IsShared, this.m_Slots.SaveID);
                await this.DataStorage.SetBlob(key, this.m_Slots.SaveData);
            }

            this.IsDeleting = false;
            this.EventAfterDelete?.Invoke(slot);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private async Task LoadItem(IGameSave reference, int slot)
        {
            string key = DatabaseKey(slot, reference.IsShared, reference.SaveID);

            object blob = await this.DataStorage.GetBlob(key, reference.SaveType, null);
            await reference.OnLoad(blob);
        }

        // PRIVATE STATIC METHODS: ----------------------------------------------------------------

        private static string DatabaseKey(int slot, bool isShared, string key)
        {
            slot = isShared ? 0 : Mathf.Clamp(slot, SLOT_MIN, SLOT_MAX);
            return string.Format(DB_KEY_FORMAT, slot, key);
        }
    }
}