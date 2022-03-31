using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [HelpURL("https://docs.gamecreator.io/gamecreator/variables/local-name-variables")]
    [AddComponentMenu("Game Creator/Variables/Local Name Variables")]
    [Icon(RuntimePaths.GIZMOS + "GizmoLocalNameVariables.png")]
    
    [Serializable]
    public class LocalNameVariables : TLocalVariables, INameVariable
    {
        // MEMBERS: -------------------------------------------------------------------------------
    
        [SerializeField] private NameVariableRuntime m_Runtime = new NameVariableRuntime();

        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void Awake()
        {
            this.m_Runtime.OnStartup();
            base.Awake();
        }
        
        public static LocalNameVariables Create(GameObject target, NameVariableRuntime variables)
        {
            LocalNameVariables instance = target.Add<LocalNameVariables>();
            instance.m_Runtime = variables;
            instance.m_Runtime.OnStartup();

            return instance;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public bool Exists(string name)
        {
            return this.m_Runtime.Exists(name);
        }
        
        public object Get(string name)
        {
            return this.m_Runtime.Get(name);
        }

        public void Set(string name, object value)
        {
            this.m_Runtime.Set(name, value);
        }
        
        public void Register(Action<string> callback)
        {
            this.m_Runtime.EventChange += callback;
        }
        
        public void Unregister(Action<string> callback)
        {
            this.m_Runtime.EventChange -= callback;
        }

        // IGAMESAVE: -----------------------------------------------------------------------------

        public override Type SaveType => typeof(SaveSingleNameVariables);

        public override object SaveData => this.m_SaveUniqueID.SaveValue
            ? new SaveSingleNameVariables(this.m_Runtime)
            : null;

        public override Task OnLoad(object value)
        {
            SaveSingleNameVariables saveData = value as SaveSingleNameVariables;
            if (saveData != null && this.m_SaveUniqueID.SaveValue)
            {
                NameVariable[] variables = saveData.Variables.ToArray();
                this.m_Runtime = new NameVariableRuntime(variables);
            }

            this.m_Runtime.OnStartup();
            return Task.FromResult(saveData != null || !this.m_SaveUniqueID.SaveValue);
        }
    }
}