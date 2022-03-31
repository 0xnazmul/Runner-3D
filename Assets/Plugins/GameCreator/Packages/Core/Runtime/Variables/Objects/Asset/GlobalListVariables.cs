using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [CreateAssetMenu(
        fileName = "Global Variables",
        menuName = "Game Creator/Variables/List Variables"
    )]
    [Icon(RuntimePaths.GIZMOS + "GizmoGlobalListVariables.png")]
    public class GlobalListVariables : TGlobalVariables, IListVariable
    {
        // MEMBERS: -------------------------------------------------------------------------------
    
        [SerializeField] private IndexList m_IndexList = new IndexList(
            ValueNumber.TYPE_ID,
            new IndexVariable(new ValueNumber(5f))
        );

        // PROPERTIES: ----------------------------------------------------------------------------

        internal IndexList IndexList => this.m_IndexList;

        public IdString TypeID => this.m_IndexList.TypeID;
        
        public int Count => GlobalListVariablesManager.Instance.Count(this);

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public object Get(IListGetPick pick)
        {
            return GlobalListVariablesManager.Instance.Get(this, pick);
        }
        
        public object Get(IListSetPick pick)
        {
            return GlobalListVariablesManager.Instance.Get(this, pick);
        }

        public object Get(int index)
        {
            return GlobalListVariablesManager.Instance.Get(this, index);
        }

        public void Set(IListSetPick pick, object value)
        {
            GlobalListVariablesManager.Instance.Set(this, pick, value);
        }

        public void Set(int index, object value)
        {
            GlobalListVariablesManager.Instance.Set(this, index, value);
        }
        
        public void Insert(IListGetPick pick, object value)
        {
            TValue content = TValue.CreateValue(this.TypeID, value);
            GlobalListVariablesManager.Instance.Insert(this, pick, content);
        }

        public void Insert(int index, object value)
        {
            TValue content = TValue.CreateValue(this.TypeID, value);
            GlobalListVariablesManager.Instance.Insert(this, index, content);
        }

        public void Push(object value)
        {
            TValue content = TValue.CreateValue(this.TypeID, value);
            GlobalListVariablesManager.Instance.Push(this, content);
        }

        public void Remove(IListGetPick pick)
        {
            GlobalListVariablesManager.Instance.Remove(this, pick);
        }
        
        public void Remove(int index)
        {
            GlobalListVariablesManager.Instance.Remove(this, index);
        }
        
        public void Clear()
        {
            GlobalListVariablesManager.Instance.Clear(this);
        }

        public void Move(IListGetPick pickSource, IListGetPick pickDestination)
        {
            GlobalListVariablesManager.Instance.Move(this, pickSource, pickDestination);
        }
        
        public void Move(int source, int destination)
        {
            GlobalListVariablesManager.Instance.Move(this, source, destination);
        }
        
        public void Register(Action<ListVariableRuntime.Change, int> callback)
        {
            if (ApplicationManager.IsExiting) return;
            GlobalListVariablesManager.Instance.Register(this, callback);
        }
        
        public void Unregister(Action<ListVariableRuntime.Change, int> callback)
        {
            if (ApplicationManager.IsExiting) return;
            GlobalListVariablesManager.Instance.Unregister(this, callback);
        }
        
        // EDITOR METHODS: ------------------------------------------------------------------------
        
        public string Title(int index)
        {
            return GlobalListVariablesManager.Instance.Title(this, index);
        }

        public Texture Icon(int index)
        {
            return GlobalListVariablesManager.Instance.Icon(this, index);
        }
    }
}