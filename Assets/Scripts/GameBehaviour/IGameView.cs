using System;

namespace GameBehaviour
{
    internal interface IGameView
    {
        public event Action AttackButtonPressed;
        public event Action SkipButtonPressed;

        public void IsButtonsActive(bool isActive);
    }
}