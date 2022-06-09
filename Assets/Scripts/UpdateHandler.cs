using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    internal class UpdateHandler : MonoBehaviour
    {
        public event Action UpdateTicked;

        private void Update() => UpdateTicked?.Invoke();

        public void ExecuteCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);
    }
}