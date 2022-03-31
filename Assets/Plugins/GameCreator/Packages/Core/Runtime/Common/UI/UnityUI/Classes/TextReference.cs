using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameCreator.Runtime.Common.UnityUI
{
    [Serializable]
    public class TextReference
    {
        private enum Type
        {
            Text = 0,
            TMP = 1
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private Type m_Type = Type.Text;
        [SerializeField] private Text m_Text;
        [SerializeField] private TMP_Text m_TMP;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public string Text
        {
            get
            {
                return this.m_Type switch
                {
                    Type.Text => this.m_Text != null 
                        ? this.m_Text.text 
                        : string.Empty,
                    Type.TMP => this.m_TMP != null 
                        ? this.m_TMP.text 
                        : string.Empty,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            set
            {
                switch (this.m_Type)
                {
                    case Type.Text:
                        if (this.m_Text == null) return;
                        this.m_Text.text = value;
                        break;
                    
                    case Type.TMP:
                        if (this.m_TMP == null) return;
                        this.m_TMP.text = value;
                        break;
                    
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TextReference()
        { }

        public TextReference(Text text) : this()
        {
            this.m_Type = Type.Text;
            this.m_Text = text;
        }

        // TO STRING: -----------------------------------------------------------------------------

        public override string ToString()
        {
            return this.m_Type switch
            {
                Type.Text => this.m_Text != null ? this.m_Text.gameObject.name : "(none)",
                Type.TMP => this.m_TMP != null ? this.m_TMP.gameObject.name : "(none)",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}