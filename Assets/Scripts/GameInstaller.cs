using System;
using System.Collections.Generic;
using GameBehaviour;
using Miner;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    internal class GameInstaller : MonoBehaviour
    {
        [SerializeField] private GameObject _playerMiner;
        [SerializeField] private GameObject _enemyMiner;
        [SerializeField] private List<GameObject> _leftBars;
        [SerializeField] private List<GameObject> _rightBars;
        [SerializeField] private UpdateHandler _updateHandler;
        [SerializeField] private InputHandler _input;
        [SerializeField] private GameView _gameView;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private LayerMask _enemyLayer;

        private void Start()
        {
            _gameView = _gameView.GetComponent<GameView>();
            _input = _input.GetComponent<InputHandler>();
            _updateHandler = _updateHandler.GetComponent<UpdateHandler>();

            var prefabsToType = new Dictionary<Type, GameObject>
            {
                { typeof(MinerModel), _playerMiner},
                {typeof(EnemyMinerModel), _enemyMiner}
            };

            var layersToType = new Dictionary<Type, LayerMask>
            {
                {typeof(MinerModel), _enemyLayer},
                {typeof(EnemyMinerModel), _playerLayer}
            };
            
            var gameModel = new GameModel();
            var inputHandlerPresenter = new InputHandlerPresenter(_input, gameModel);
            var gamePresenter = new GamePresenter(gameModel, _gameView, _updateHandler);
            var minerFactory = new MinerFactory(prefabsToType, gameModel, _updateHandler, layersToType);
            List<MinerModel> playerMiners = new List<MinerModel>();
            List<MinerModel> enemyMiners = new List<MinerModel>();

            for (var i = 0; i < 4; i++)
            {
                var pointer = _leftBars[i].GetComponentInChildren<Pointer>().GetComponent<Image>();
                var healthBar = _leftBars[i].GetComponentInChildren<HealthBar>().GetComponent<Slider>();
                playerMiners.Add(
                    minerFactory.CreateInstance<MinerModel>(gameModel.SlotToNumber[i], i, pointer, healthBar));

                pointer = _rightBars[3 - i].GetComponentInChildren<Pointer>().GetComponent<Image>();
                healthBar = _rightBars[3 - i].GetComponentInChildren<HealthBar>().GetComponent<Slider>();
                enemyMiners.Add(minerFactory.CreateInstance<EnemyMinerModel>(gameModel.SlotToNumber[i + 4], i + 4,
                    pointer, healthBar));
            }
            
            inputHandlerPresenter.Init();
            gameModel.Init(_updateHandler, playerMiners, enemyMiners);
        }
    }
}