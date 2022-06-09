using System;
using GameBehaviour;
using Miner.States;
using UnityEngine;

namespace Miner
{
    internal class MinerModel
    {
        private Vector3 _initialPosition;
        private int _health;
        private int _maxHealth;
        private GameModel _gameModel; 
        
        public BaseState CurrentState { get; set; }
        public int Damage { get; private set; }
        public LayerMask AttackLayer { get; private set; }
        public int Slot { get; set; }

        public event Action<MinerModel> TargetReceived;
        public event Action<int, int> DamageTaken;
        public event Action<Vector3> PositionChanged;
        public event Action ActionDone;
        public event Action ActionStarted;
        public event Action<bool> TurnReceived;

        public void Init(int slot, GameModel gameModel, LayerMask attackLayer)
        {
            _maxHealth = 100;
            _health = _maxHealth;
            Damage = 10;
            Slot = slot;
            AttackLayer = attackLayer;
            _gameModel = gameModel;
            _gameModel.TurnStarted += StartedTurnReceived;
            _gameModel.TurnDone += OnTurnEnd;
        }

        public void StartAction()
        {
            ActionStarted?.Invoke();
        }

        public void OnActionDone() => ActionDone?.Invoke();

        public void Move(Vector3 position)
        {
            PositionChanged?.Invoke(position);
        }

        public void OnTargetReceived(MinerModel target)
        {
            TargetReceived?.Invoke(target);
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            
            if (_health < 0)
                _health = 0;
            
            DamageTaken?.Invoke(_health, _maxHealth);
        }

        public void StartedTurnReceived() => TurnReceived?.Invoke(false);
        public void OnTurnEnd() => TurnReceived?.Invoke(true);

    }
}
