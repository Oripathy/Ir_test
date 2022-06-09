using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameBehaviour
{
    internal class GameView : MonoBehaviour, IGameView
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _skipButton;

        public event Action AttackButtonPressed;
        public event Action SkipButtonPressed;

        private void Awake()
        {
            _attackButton.onClick.AddListener(() => AttackButtonPressed?.Invoke());
            _skipButton.onClick.AddListener(() => SkipButtonPressed?.Invoke());
        }

        public void IsButtonsActive(bool isActive)
        {
            _attackButton.gameObject.SetActive(isActive);
            _skipButton.gameObject.SetActive(isActive);
        }
    }
}