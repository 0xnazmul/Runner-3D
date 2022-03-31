using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [Serializable]
    public class NameVariableRuntime : TVariableRuntime<NameVariable>
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private NameList m_List = new NameList();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        internal Dictionary<string, NameVariable> Variables { get; private set; }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<string> EventChange;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NameVariableRuntime()
        { }
        
        public NameVariableRuntime(NameList nameList) : this()
        {
            this.m_List = nameList;
        }

        public NameVariableRuntime(params NameVariable[] nameList) : this()
        {
            this.m_List = new NameList(nameList);
        }
        
        // INITIALIZERS: --------------------------------------------------------------------------

        public override void OnStartup()
        {
            this.Variables = new Dictionary<string, NameVariable>();
            
            for (int i = 0; i < this.m_List.Length; ++i)
            {
                NameVariable variable = this.m_List.Get(i) as NameVariable;
                if (variable == null) continue;
                
                if (this.Variables.ContainsKey(variable.Name)) continue;
                this.Variables.Add(variable.Name, variable.Copy as NameVariable);
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public bool Exists(string name)
        {
            return this.Variables.ContainsKey(name);
        }

        public object Get(string name)
        {
            return this.Variables.TryGetValue(name, out NameVariable entry) && entry != null
                ? entry.Value
                : null;
        }

        public string Title(string name)
        {
            return this.Variables.TryGetValue(name, out NameVariable entry) && entry != null
                ? entry.Title
                : string.Empty;
        }
        
        public Texture Icon(string name)
        {
            return this.Variables.TryGetValue(name, out NameVariable entry) && entry != null
                ? entry.Icon
                : null;
        }

        public void Set(string name, object value)
        {
            if (!this.Variables.TryGetValue(name, out NameVariable entry) || entry == null) return;
            
            entry.Value = value;
            this.EventChange?.Invoke(name);
        }
        
        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        public override IEnumerator<NameVariable> GetEnumerator()
        {
            return this.Variables.Values.GetEnumerator();
        }
    }
}