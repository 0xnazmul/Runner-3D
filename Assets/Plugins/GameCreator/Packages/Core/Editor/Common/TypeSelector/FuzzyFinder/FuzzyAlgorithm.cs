using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Common
{
    internal class FuzzyAlgorithm
    {
        private static readonly char[] WORD_SEPARATOR = { ' ' };

        // MEMBERS: -------------------------------------------------------------------------------
        
        private readonly Type m_TypeBase;
        private readonly FuzzyNode m_Domain;
        
        private Type[] m_TypesCollection;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Type[] Types => this.m_TypesCollection;

        // INITIALIZERS: --------------------------------------------------------------------------

        public FuzzyAlgorithm(Type mTypeBase)
        {
            this.m_TypeBase = mTypeBase;
            this.m_Domain = new FuzzyNode();
            
            this.CreateDomain();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public List<Type> Match(string search, int maxAmount, int maxDistance)
        {
            string text = TextUtils.Humanize(search).ToLowerInvariant();
            string[] queries = text.Split(WORD_SEPARATOR);
            
            Dictionary<Type, int[]> candidateResults = new Dictionary<Type, int[]>();

            for (int i = 0; i < queries.Length; ++i)
            {
                List<char> characters = new List<char>(queries[i]);
                Dictionary<Type, int> matches = this.m_Domain.Match(characters, maxDistance);

                foreach (KeyValuePair<Type, int> match in matches)
                {
                    if (!candidateResults.ContainsKey(match.Key))
                    {
                        candidateResults[match.Key] = new int[queries.Length];
                        candidateResults[match.Key][i] = match.Value;
                    }
                    else
                    {
                        int priority = Math.Max(candidateResults[match.Key][i], match.Value);
                        candidateResults[match.Key][i] = priority;
                    }
                }
            }
            
            Dictionary<Type, int> candidates = new Dictionary<Type, int>();
            foreach (KeyValuePair<Type, int[]> candidateResult in candidateResults)
            {
                int sum = candidateResult.Value.Sum();
                candidates[candidateResult.Key] = sum;
            }

            int currentAmount = 0;
            List<Type> list = new List<Type>();
            
            foreach (KeyValuePair<Type, int> candidate in candidates.OrderByDescending(entry => entry.Value))
            {
                if (currentAmount >= maxAmount) break;
                if (candidate.Value <= 0) continue;
                
                list.Add(candidate.Key);
                currentAmount += 1;
            }

            return list;
        }
        
        // PRIVATE BUILD METHODS: -----------------------------------------------------------------

        private void CreateDomain()
        {
            this.m_TypesCollection = TypeUtils.GetTypesDerivedFrom(m_TypeBase).ToArray();
            
            foreach (Type type in this.m_TypesCollection)
            {
                this.Index(type, type.GetCustomAttributes<TitleAttribute>().FirstOrDefault());
                this.Index(type, type.GetCustomAttributes<CategoryAttribute>().FirstOrDefault());
                
                var keywords = type.GetCustomAttributes<KeywordsAttribute>();
                foreach (KeywordsAttribute keyword in keywords) this.Index(type, keyword);
                
                var parameters = type.GetCustomAttributes<ParameterAttribute>();
                foreach (ParameterAttribute param in parameters) this.Index(type, param);
                
                this.Index(type, type.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault());

                string[] typeTokens = TextUtils
                    .Humanize(type.Name)
                    .ToLowerInvariant()
                    .Split(WORD_SEPARATOR);
                
                foreach (string typeToken in typeTokens)
                {
                    if (string.IsNullOrEmpty(typeToken)) continue;
                    if (typeToken.Length < 3) continue;
                    
                    this.m_Domain.Insert(type, new List<char>(typeToken), 1);
                }
            }
        }

        private void Index(Type type, ISearchable attribute)
        {
            if (attribute == null) return;

            string text = TextUtils.Humanize(attribute.SearchText).ToLowerInvariant();
            string[] queries = text.Split(WORD_SEPARATOR);

            foreach (string query in queries)
            {
                if (string.IsNullOrEmpty(query)) continue;
                this.m_Domain.Insert(type, new List<char>(query), attribute.SearchPriority);
            }
        }
    }
}