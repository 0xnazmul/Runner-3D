using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Shuffle List")]
    [Description("Randomly shuffles the position of each element on a List Variable")]
    
    [Image(typeof(IconShuffle), ColorTheme.Type.Teal)]
    
    [Category("Variables/Shuffle List")]
    
    [Parameter("List Variable", "Local List or Global List which elements are shuffled")]

    [Keywords("Randomize", "Sort", "Array", "List", "Variables")]
    [Serializable]
    public class InstructionVariablesShuffle : Instruction
    {
        [SerializeField] 
        private CollectorListVariable m_ListVariable = new CollectorListVariable();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Shuffle {this.m_ListVariable}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            List<object> elements = this.m_ListVariable.Get();
            int numElements = elements.Count;  
            while (numElements > 1)
            {  
                numElements -= 1;  
                int newIndex = UnityEngine.Random.Range(0, numElements + 1);  
                
                object value = elements[newIndex];  
                elements[newIndex] = elements[numElements];  
                elements[numElements] = value;  
            }

            this.m_ListVariable.Fill(elements.ToArray());
            return DefaultResult;
        }
    }
}