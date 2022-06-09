using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Miner;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameBehaviour
{
    internal class GameModel
    {
        private int _turn;
        private MinerModel _selectedMiner;
        private MinerModel _opponentMiner;
        private UpdateHandler _updateHandler;
        private Dictionary<int, Vector3> _slotToNumber;
        private Vector3 _leftPosition = new Vector3(-3.5f, 0f, -2f);
        private Vector3 _rightPosition = new Vector3(3.5f, 0f, -2f);
        
        public Camera Camera { get; private set; }
        public float MoveDuration { get; private set; }
        public Vector3 InitialCameraPosition { get; private set; }
        public Vector3 ApproachedCameraPosition { get; private set; }

        public List<MinerModel> AvailableMiners = new List<MinerModel>();
        public List<MinerModel> AllMiners = new List<MinerModel>();
        public List<MinerModel> PlayerMiners = new List<MinerModel>();
        public List<MinerModel> EnemyMiners = new List<MinerModel>();
        public Dictionary<int, Vector3> SlotToNumber => _slotToNumber;

        public event Action<LayerMask> AttackButtonPressed;
        public event Action SkipButtonPressed;
        public event Action TurnDone;
        public event Action TurnStarted;
        public event Action<Vector3> TurnAccepted;

        public GameModel()
        {
            _slotToNumber = new Dictionary<int, Vector3>
            {
                {0, new Vector3(-22.01f, 0.31f, 1f)},
                {1, new Vector3(-16.55f, 0.31f, 1f)},
                {2, new Vector3(-10.42f, 0.31f, 1f)},
                {3, new Vector3(-4.83f, 0.31f, 1f)},
                {4, new Vector3(23.66f, 0.31f, 1f)},
                {5, new Vector3(17.9f, 0.31f, 1f)},
                {6, new Vector3(12.23f, 0.31f, 1f)},
                {7, new Vector3(6.56f, 0.31f, 1f)}
            };
        }
        
        public void Init(UpdateHandler updateHandler, List<MinerModel> playerMiners, List<MinerModel> enemyMiners)
        {
            MoveDuration = 0.5f;
            InitialCameraPosition = new Vector3(0f, 6f, -10f);
            ApproachedCameraPosition = new Vector3(0f, 6f, -8f);
            PlayerMiners = playerMiners;
            EnemyMiners = enemyMiners;
            _updateHandler = updateHandler;
            
            foreach (var miner in PlayerMiners)
            {
                AvailableMiners.Add(miner);
                AllMiners.Add(miner);
            }

            foreach (var miner in EnemyMiners)
            {
                AvailableMiners.Add(miner);
                AllMiners.Add(miner);
            }

            _selectedMiner = ChoseMinerFrom(AvailableMiners);
            _selectedMiner.ActionDone += OnActionDone;
            _selectedMiner.StartAction();
            Camera = Camera.main;
        }

        public void StartDuel(int index)
        {
            var opponent = AllMiners[index];
            _opponentMiner = opponent;
            _updateHandler.StartCoroutine(Move(opponent));
        }

        public void OnAttackButtonPressed()
        {
            AttackButtonPressed?.Invoke(_selectedMiner.AttackLayer);
        }

        public void OnSkipButtonPressed()
        {
            SkipButtonPressed?.Invoke();
            _selectedMiner.ActionDone -= OnActionDone;
            AvailableMiners.Remove(_selectedMiner);
            TurnDone?.Invoke();
            
            if (AvailableMiners.Count != 0)
            {
                _selectedMiner = ChoseMinerFrom(AvailableMiners);
                _selectedMiner.ActionDone += OnActionDone;
                _selectedMiner.StartAction();
            }
            else
            {
                StartTurn();
            }
        }
        
        private MinerModel ChoseMinerFrom(List<MinerModel> miners)
        {
            int randomNumber = Random.Range(0, miners.Count);
            return miners[randomNumber];
        }

        private void OnActionDone()
        {
            _selectedMiner.ActionDone -= OnActionDone;
            AvailableMiners.Remove(_selectedMiner);
            _updateHandler.ExecuteCoroutine(MoveBack(_opponentMiner));
        }

        private void StartTurn()
        {
            foreach (var miner in PlayerMiners)
                AvailableMiners.Add(miner);

            foreach (var miner in EnemyMiners)
                AvailableMiners.Add(miner);
            

            _selectedMiner = ChoseMinerFrom(AvailableMiners);
            _selectedMiner.ActionDone += OnActionDone;
            _selectedMiner.StartAction();
        }

        private IEnumerator Move(MinerModel miner)
        {
            var startTime = Time.time;
            TurnAccepted?.Invoke(ApproachedCameraPosition);
            TurnStarted?.Invoke();
            
            while (Time.time <= startTime + 0.5f)
            {
                var slot = _selectedMiner.Slot;
                Vector3 movePos;

                if (slot <= 3)
                    movePos = _leftPosition;
                else
                    movePos = _rightPosition;
                
                var prevPos = _slotToNumber[slot];
                var position = Vector3.Slerp(prevPos, movePos, (Time.time - startTime) / 0.5f);
               _selectedMiner.Move(position);

               slot = miner.Slot;

               if (slot <= 3)
                   movePos = _leftPosition;
               else
                   movePos = _rightPosition;
               
               prevPos = _slotToNumber[slot];
               position = Vector3.Slerp(prevPos, movePos, (Time.time - startTime) / 0.5f);
               miner.Move(position);
               
               yield return null;
            }
            
            _selectedMiner.OnTargetReceived(miner);
        }

        private IEnumerator MoveBack(MinerModel miner)
        {
            var startTime = Time.time;
            TurnAccepted?.Invoke(InitialCameraPosition);

            while (Time.time <= startTime + 0.5f)
            {
                var slot = _selectedMiner.Slot;
                Vector3 prevPos;

                if (slot <= 3)
                    prevPos = _leftPosition;
                else
                    prevPos = _rightPosition;

                var movePos = _slotToNumber[slot];
                var position = Vector3.Slerp(prevPos, movePos, (Time.time - startTime) / 0.5f);
                _selectedMiner.Move(position);

                slot = miner.Slot;

                if (slot <= 3)
                    prevPos = _leftPosition;
                else
                    prevPos = _rightPosition;

                movePos = _slotToNumber[slot];
                position = Vector3.Slerp(prevPos, movePos, (Time.time - startTime) / 0.5f);
                miner.Move(position);

                yield return null;
            }
            
            TurnDone?.Invoke();
            
            if (AvailableMiners.Count != 0)
            {
                _selectedMiner = ChoseMinerFrom(AvailableMiners);
                _selectedMiner.ActionDone += OnActionDone;
                _selectedMiner.StartAction();
            }
            else
            {
                StartTurn();
            }
        }
    }
}