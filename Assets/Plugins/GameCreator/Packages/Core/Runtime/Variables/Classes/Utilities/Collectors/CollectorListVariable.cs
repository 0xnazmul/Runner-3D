using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [Serializable]
    public class CollectorListVariable
    {
        private enum Type
        {
            LocalList,
            GlobalList
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private Type m_ListVariable = Type.LocalList;

        [SerializeField] private LocalListVariables m_LocalList;
        [SerializeField] private GlobalListVariables m_GlobalList;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public IdString TypeID => this.m_ListVariable switch
        {
            Type.LocalList => this.m_LocalList != null
                ? this.m_LocalList.TypeID
                : ValueNull.TYPE_ID,
            
            Type.GlobalList => this.m_GlobalList != null
                ? this.m_GlobalList.TypeID
                : ValueNull.TYPE_ID,
            
            _ => ValueNull.TYPE_ID
        };

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Fill(GameObject[] gameObjects)
        {
            object[] array = new object[gameObjects.Length];
            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = gameObjects[i];
            }

            this.Fill(array);
        }
        
        public void Fill(object[] values)
        {
            switch (this.m_ListVariable)
            {
                case Type.LocalList:
                    if (this.m_LocalList == null) return;
                    this.m_LocalList.Clear();
                    foreach (object value in values)
                    {
                        if (value == null) continue;
                        this.m_LocalList.Push(value);
                    }
                    break;
                
                case Type.GlobalList:
                    if (this.m_GlobalList == null) return;
                    this.m_GlobalList.Clear();
                    foreach (object value in values)
                    {
                        if (value == null) continue;
                        this.m_GlobalList.Push(value);
                    }
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void Clear()
        {
            switch (this.m_ListVariable)
            {
                case Type.LocalList:
                    if (this.m_LocalList == null) return;
                    this.m_LocalList.Clear();
                    break;
                
                case Type.GlobalList:
                    if (this.m_GlobalList == null) return;
                    this.m_GlobalList.Clear();
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void Remove(IListGetPick pick)
        {
            switch (this.m_ListVariable)
            {
                case Type.LocalList:
                    if (this.m_LocalList == null) return;
                    this.m_LocalList.Remove(pick);
                    break;
                
                case Type.GlobalList:
                    if (this.m_GlobalList == null) return;
                    this.m_GlobalList.Remove(pick);
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public List<object> Get()
        {
            List<object> list = new List<object>();
            
            switch (this.m_ListVariable)
            {
                case Type.LocalList:
                    if (this.m_LocalList != null)
                    {
                        for (int i = 0; i < this.m_LocalList.Count; ++i)
                        {
                            list.Add(this.m_LocalList.Get(i));
                        }   
                    }
                    break;
                
                case Type.GlobalList:
                    if (this.m_GlobalList != null)
                    {
                        for (int i = 0; i < this.m_GlobalList.Count; ++i)
                        {
                            list.Add(this.m_GlobalList.Get(i));
                        }   
                    }
                    break;
            }

            return list;
        }
        
        // OVERRIDES: -----------------------------------------------------------------------------

        public override string ToString()
        {
            return this.m_ListVariable switch
            {
                Type.LocalList => this.m_LocalList != null
                    ? this.m_LocalList.gameObject.name
                    : "(none)",
                
                Type.GlobalList => this.m_GlobalList != null 
                    ? this.m_GlobalList.name 
                    : "(none)",
                
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}