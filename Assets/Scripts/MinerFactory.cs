using System;
using System.Collections.Generic;
using DefaultNamespace;
using GameBehaviour;
using Miner;
using UnityEngine;
using UnityEngine.UI;

internal class MinerFactory
{
    private Dictionary<Type, GameObject> _prefabsToType;
    private GameModel _gameModel;
    private UpdateHandler _updateHandler;
    private Dictionary<Type, LayerMask> _layersToType;

    public MinerFactory(Dictionary<Type, GameObject> prefabsToType, GameModel gameModel, UpdateHandler updateHandler,
        Dictionary<Type, LayerMask> layersToType)
    {
        _prefabsToType = prefabsToType;
        _gameModel = gameModel;
        _updateHandler = updateHandler;
        _layersToType = layersToType;
    }

    public T CreateInstance<T>(Vector3 position, int slot, Image pointer, Slider healthBar)
        where T : MinerModel, new()
    {
        var type = typeof(T);
        var prefab = _prefabsToType[type];
        var layer = _layersToType[type];
        var model = new T();
        var view = GameObject.Instantiate(prefab, position, Quaternion.identity).GetComponent<MinerView>();
        view.Init(pointer, healthBar);
        var presenter = new MinerPresenter(model, view, _updateHandler).Init();
        model.Init(slot, _gameModel, layer);
        return model;
    }
}