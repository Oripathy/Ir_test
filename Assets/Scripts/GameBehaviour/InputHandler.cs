using System;
using System.Reflection;
using Miner;
using UnityEngine;

namespace GameBehaviour
{
    internal class InputHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private Camera _camera;

        public bool IsInputActive { get; set; }
        public bool IsInteracted { get; set;  }

        public event Action<int> SlotSelected;

        private void Awake()
        {
            IsInputActive = true;
        }
            
        private void Update()
        {
            if (!IsInputActive)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                var position = _camera.ScreenPointToRay(Input.mousePosition);

                if (IsInteracted)
                {
                    var rayHit = Physics2D.GetRayIntersection(position, Mathf.Infinity, _layer);

                    if (rayHit.collider != null && rayHit.collider.TryGetComponent<Slot>(out var slot))
                    {
                        SlotSelected?.Invoke(slot.GetIndex());
                        Debug.Log(slot.GetIndex());
                        IsInteracted = false;
                    }
                }
            }
        }

        public void SetLayer(LayerMask layer) => _layer = layer;
    }
}