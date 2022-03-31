using System;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [AddComponentMenu("")]
    internal class AnimimAnimatorProxy : MonoBehaviour
    {
        public TUnitAnimim Animim { private get; set; }

        // MONOBEHAVIOUR METHODS: -----------------------------------------------------------------

        private void OnAnimatorIK(int layerIndex)
        {
            this.Animim?.OnAnimatorIK(layerIndex);   
        }

        private void OnAnimatorMove()
        {
            this.Animim?.OnAnimatorMove();
        }
    }
}