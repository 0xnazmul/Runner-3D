using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Characters.IK
{
    public interface ILookTrack
    {
        public int Layer { get; }
        public bool Exists { get; }
        public Vector3 Position { get; }
    }
    
    internal class ILookTrackComparer : IComparer<ILookTrack>
    {
        public int Compare(ILookTrack x, ILookTrack y)
        {
            return (x?.Layer ?? 0).CompareTo(y?.Layer ?? 0);
        }
    }
}