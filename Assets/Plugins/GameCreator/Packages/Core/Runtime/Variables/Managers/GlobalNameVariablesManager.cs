using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [AddComponentMenu("")]
    public class GlobalNameVariablesManager : Singleton<GlobalNameVariablesManager>, IGameSave
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        private Dictionary<IdString, NameVariableRuntime> Values;

        private HashSet<IdString> SaveValues;

        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnCreate()
        {
            base.OnCreate();

            this.Values = new Dictionary<IdString, NameVariableRuntime>();
            this.SaveValues = new HashSet<IdString>();

            _ = SaveLoadManager.Subscribe(this);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Exists(GlobalNameVariables asset, string name)
        {
            this.RequireInit(asset);

            return this.Values.TryGetValue(
                asset.UniqueID,
                out NameVariableRuntime runtime
            ) && runtime.Exists(name);
        }
        
        public object Get(GlobalNameVariables asset, string name)
        {
            this.RequireInit(asset);

            return this.Values.TryGetValue(asset.UniqueID, out NameVariableRuntime runtime) 
                ? runtime.Get(name) 
                : null;
        }
        
        public string Title(GlobalNameVariables asset, string name)
        {
            this.RequireInit(asset);

            return this.Values.TryGetValue(asset.UniqueID, out NameVariableRuntime runtime) 
                ? runtime.Title(name) 
                : string.Empty;
        }
        
        public Texture Icon(GlobalNameVariables asset, string name)
        {
            this.RequireInit(asset);

            return this.Values.TryGetValue(asset.UniqueID, out NameVariableRuntime runtime) 
                ? runtime.Icon(name) 
                : null;
        }

        public void Set(GlobalNameVariables asset, string name, object value)
        {
            this.RequireInit(asset);
            if (!this.Values.TryGetValue(asset.UniqueID, out NameVariableRuntime runtime)) return;
            
            runtime.Set(name, value);
            if (asset.Save) this.SaveValues.Add(asset.UniqueID);
        }

        public void Register(GlobalNameVariables asset, Action<string> callback)
        {
            this.RequireInit(asset);

            if (this.Values.TryGetValue(asset.UniqueID, out NameVariableRuntime runtime))
            {
                runtime.EventChange += callback;
            }
        }
        
        public void Unregister(GlobalNameVariables asset, Action<string> callback)
        {
            this.RequireInit(asset);

            if (this.Values.TryGetValue(asset.UniqueID, out NameVariableRuntime runtime))
            {
                runtime.EventChange -= callback;
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RequireInit(GlobalNameVariables asset)
        {
            if (this.Values.ContainsKey(asset.UniqueID)) return;
            
            NameVariableRuntime runtime = new NameVariableRuntime(asset.NameList);
            runtime.OnStartup();

            this.Values[asset.UniqueID] = runtime;
        }

        // IGAMESAVE: -----------------------------------------------------------------------------

        public string SaveID => "global-name-variables";

        public LoadMode LoadMode => LoadMode.Greedy;
        public bool IsShared => false;

        public Type SaveType => typeof(SaveGroupNameVariables);

        public object SaveData
        {
            get
            {
                Dictionary<string, NameVariableRuntime> saveValues =
                    new Dictionary<string, NameVariableRuntime>();
                        
                foreach (KeyValuePair<IdString, NameVariableRuntime> entry in this.Values)
                {
                    if (!this.SaveValues.Contains(entry.Key)) continue;
                    saveValues.Add(entry.Key.String, entry.Value);
                }

                SaveGroupNameVariables saveData = new SaveGroupNameVariables(saveValues);
                return saveData;
            }
        }

        public Task OnLoad(object value)
        {
            this.Values = new Dictionary<IdString, NameVariableRuntime>();
            this.SaveValues = new HashSet<IdString>();

            SaveGroupNameVariables saveData = value as SaveGroupNameVariables;
            if (saveData == null) return Task.FromResult(false);

            int numGroups = saveData.Count();
            for (int i = 0; i < numGroups; ++i)
            {
                IdString uniqueID = new IdString(saveData.GetID(i));

                NameVariable[] variables = saveData.GetData(i).Variables.ToArray();
                NameVariableRuntime data = new NameVariableRuntime(variables);

                this.SaveValues.Add(uniqueID);
                this.Values.Add(uniqueID, data);
                
                data.OnStartup();
            }
            
            return Task.FromResult(true);
        }
    }
}