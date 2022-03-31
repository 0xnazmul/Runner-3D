using System;
using System.Collections.Generic;

namespace GameCreator.Editor.Common
{
    internal class FuzzyNode
    {
        private readonly struct Data
        {
            public readonly int priority;
            public readonly bool complete;

            public Data(int priority, bool complete)
            {
                this.priority = priority;
                this.complete = complete;
            }
        }
        
        private readonly Dictionary<char, FuzzyNode> m_Symbols;
        private readonly Dictionary<Type, Data> m_Data;

        // INITIALIZERS: --------------------------------------------------------------------------

        public FuzzyNode()
        {
            this.m_Symbols = new Dictionary<char, FuzzyNode>();
            this.m_Data = new Dictionary<Type, Data>();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Insert(Type type, List<char> text, int priority)
        {
            this.Insert(type, text, 0, priority);
        }
        
        public Dictionary<Type, int> Match(List<char> text, int maxDistance)
        {
             Dictionary<Type, int> candidates = new Dictionary<Type, int>();
             this.Match(text, ref candidates, 0, 0, maxDistance);

             return candidates;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void Insert(Type type, List<char> text, int index, int priority)
        {
            bool isComplete = index >= text.Count - 1;
            if (!this.m_Data.ContainsKey(type))
            {
                this.m_Data[type] = new Data(priority, isComplete);
            }
            else 
            {
                this.m_Data[type] = new Data(
                    Math.Max(priority, this.m_Data[type].priority),
                    this.m_Data[type].complete || isComplete
                );
            }
            
            if (index >= text.Count) return;
            char character = text[index];

            if (!this.m_Symbols.ContainsKey(character))
            {
                this.m_Symbols.Add(character, new FuzzyNode());
            }
            
            this.m_Symbols[character].Insert(type, text, index + 1, priority);
        }
        
        private void Match(List<char> text, ref Dictionary<Type, int> candidates,
            int index, int score, int maxDistance)
        {
            if (index >= text.Count) return;
            char character = text[index];

            if (this.m_Symbols.TryGetValue(character, out FuzzyNode children))
            {
                children.Match(text, ref candidates, index + 1, score + 2,  maxDistance);
            }

            if (maxDistance > 0)
            {
                foreach (KeyValuePair<char, FuzzyNode> symbol in this.m_Symbols)
                {
                    if (symbol.Key == character) continue;
                    symbol.Value.Match(text, ref candidates, index + 1, score + 1, maxDistance - 1);
                }
            }

            foreach (KeyValuePair<Type, Data> node in this.m_Data)
            {
                int exactMatch = node.Value.complete ? 2 : 1;
                int nodeScore = (index + 1) * node.Value.priority * score * exactMatch;

                if (!candidates.ContainsKey(node.Key)) candidates[node.Key] = nodeScore;
                else candidates[node.Key] = Math.Max(candidates[node.Key], nodeScore);
            }
        }
    }
}
