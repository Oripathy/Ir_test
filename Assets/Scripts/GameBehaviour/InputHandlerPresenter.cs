using Miner;
using UnityEngine;

namespace GameBehaviour
{
    internal class InputHandlerPresenter
    {
        private InputHandler _view;
        private GameModel _model;
        private MinerModel _miner;

        public InputHandlerPresenter(InputHandler view, GameModel model)
        {
            _view = view;
            _model = model;
        }

        public void Init()
        {
            _view.SlotSelected += OnSlotSelected;
            _model.TurnDone += OnTurnDone;
            _model.AttackButtonPressed += OnAttackButtonPressed;
            _model.SkipButtonPressed += OnSkipButtonPressed;
        }

        private void OnTurnDone() => _view.IsInputActive = true;

        private void OnSlotSelected(int index) => _model.StartDuel(index);
        
        private void OnAttackButtonPressed(LayerMask layer)
        {
            _view.IsInputActive = true;
            _view.IsInteracted = true;
            _view.SetLayer(layer);
        }

        private void OnSkipButtonPressed()
        {
            _view.IsInputActive = false;
            _view.IsInteracted = false;
        }

    }
}