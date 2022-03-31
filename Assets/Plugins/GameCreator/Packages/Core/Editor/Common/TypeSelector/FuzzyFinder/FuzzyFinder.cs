using System;
using System.Collections.Generic;

namespace GameCreator.Editor.Common
{
    internal static class FuzzyFinder
    {
        private const int MAX_RESULTS = 10;

        // STATIC DATA: ---------------------------------------------------------------------------

        private static readonly Dictionary<Type, FuzzyAlgorithm> Finders = 
            new Dictionary<Type, FuzzyAlgorithm>();

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static void Awake(Type type)
        {
            if (!Finders.ContainsKey(type))
            {
                Finders.Add(type, new FuzzyAlgorithm(type));
            }
        }
        
        public static List<Type> Search(Type type, string search)
        {
            Awake(type);
            return Finders[type].Match(search, MAX_RESULTS, search.Length > 3 ? 2 : 1);
        }
    }
}