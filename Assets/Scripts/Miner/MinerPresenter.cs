using System;
using System.Collections.Generic;
using DefaultNamespace;
using Miner.States;
using UnityEngine;

namespace Miner
{
    internal class MinerPresenter
    {
        private MinerModel _model;
        private IMinerView _view;
        private UpdateHandler _updateHandler;
        private Dictionary<Type, BaseState> _stateToType;

        public MinerPresenter(MinerModel model, IMinerView view, UpdateHandler updateHandler)
        {
            _model = model;
            _view = view;
            _updateHandler = updateHandler;
        }

        public MinerPresenter Init()
        {
            _stateToType = new Dictionary<Type, BaseState>
            {
                {typeof(IdleState), new IdleState(_view, _model, _updateHandler, this)},
                {typeof(AttackState), new AttackState(_view, _model, _updateHandler, this)},
                {typeof(ReceiveDamageState), new ReceiveDamageState(_view, _model, _updateHandler, this)}
            };

            _view.SetPointer(false);
            Subscribe();
            _model.CurrentState = _stateToType[typeof(IdleState)];
            _model.CurrentState.OnEnter();
            return this;
        }

        private void Subscribe()
        {
            _updateHandler.UpdateTicked += Update;
            _model.ActionStarted += OnActionStarted;
            _model.ActionDone += OnActionDone;
            _model.TargetReceived += OnTargetReceived;
            _model.DamageTaken += OnDamageTaken;
            _model.PositionChanged += OnPositionChanged;
            _model.TurnReceived += OnTurnReceived;
        }

        private void Update() => _model.CurrentState.Update();

        public void SwitchState<T>()
            where T : BaseState
        {
            var type = typeof(T);

            if (_stateToType.TryGetValue(type, out var state))
            {
                _model.CurrentState.OnExit();
                _model.CurrentState = state;
                state.OnEnter();
            }
        }

        private void OnTargetReceived(MinerModel miner)
        {
            SwitchState<AttackState>();
            var attackState = _model.CurrentState as AttackState;
            attackState?.StartAttack(miner);
        }
        

        private void OnDamageTaken(int health, int maxHealth)
        {
            SwitchState<ReceiveDamageState>();
            _view.UpdateHealth(health, maxHealth);
        }

        private void OnActionStarted() => _view.SetPointer(true);
        

        private void OnActionDone() => _view.SetPointer(false);

        private void OnPositionChanged(Vector3 position) => _view.Transform.position = position;

        private void OnTurnReceived(bool isActive) => _view.SetUI(isActive);
    }
}