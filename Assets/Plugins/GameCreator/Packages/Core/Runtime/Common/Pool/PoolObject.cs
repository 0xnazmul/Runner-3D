using System;
using System.Collections;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [AddComponentMenu("")]
    internal class PoolObject : MonoBehaviour
    {
        [NonSerialized] private IEnumerator m_Coroutine;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void SetDuration(float duration)
        {
            this.m_Coroutine = this.DisableInSeconds(duration);
            this.StartCoroutine(this.m_Coroutine);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private IEnumerator DisableInSeconds(float duration)
        {
            WaitForSeconds wait = new WaitForSeconds(duration);
            yield return wait;

            this.gameObject.SetActive(false);
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnDisable()
        {
            this.CancelInvoke();

            if (this.m_Coroutine == null) return;
            this.StopCoroutine(this.m_Coroutine);
        }
    }
}