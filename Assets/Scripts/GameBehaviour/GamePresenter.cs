using System.Collections;
using DefaultNamespace;
using UnityEngine;

namespace GameBehaviour
{
    internal class GamePresenter
    {
        private GameModel _model;
        private IGameView _view;
        private UpdateHandler _updateHandler;

        public GamePresenter(GameModel model, IGameView view, UpdateHandler updateHandler)
        {
            _model = model;
            _view = view;
            _updateHandler = updateHandler;
            _view.AttackButtonPressed += OnAttackButtonPressed;
            _view.SkipButtonPressed += OnSkipButtonPressed;
            _model.TurnAccepted += OnTurnAccepted;
            _model.TurnStarted += OnTurnStarted;
            _model.TurnDone += OnTurnEnd;
        }

        private void OnTurnAccepted(Vector3 pos)
        {
            _updateHandler.ExecuteCoroutine(MoveCamera(pos));
        }

        private IEnumerator MoveCamera(Vector3 destinationPosition)
        {
            var initialPosition = _model.Camera.transform.position;
            var startTime = Time.time;

            while (Time.time <= startTime + _model.MoveDuration)
            {
                _model.Camera.transform.position = Vector3.Slerp(initialPosition, destinationPosition,
                    (Time.time - startTime) / _model.MoveDuration);

                yield return null;
            }
        }

        private void OnAttackButtonPressed() => _model.OnAttackButtonPressed();

        private void OnSkipButtonPressed() => _model.OnSkipButtonPressed();

        private void OnTurnStarted() => _view.IsButtonsActive(false);

        private void OnTurnEnd() => _view.IsButtonsActive(true);
    }
}