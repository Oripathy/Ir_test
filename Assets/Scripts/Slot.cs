using UnityEngine;

internal class Slot : MonoBehaviour
{
    [SerializeField] private int _slotIndex;

    public int GetIndex() => _slotIndex;
}