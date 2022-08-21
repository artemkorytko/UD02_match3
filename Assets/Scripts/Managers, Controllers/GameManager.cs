using System;
using UnityEngine;
using Zenject;

public class GameManager : IInitializable, IDisposable
{
    private SaveSystem _saveSystem;
    private SignalBus _signalBus;
    private BoardController _boardController;
    private int _score = -1;

    private int Score
    {
        get => _score;
        set
        {
            if (value == _score)
                return;

            _score = value;
            _signalBus.Fire(new ScoreChangedSignal(_score));
        }
    }
    
    public GameManager(SaveSystem saveSystem, SignalBus signalBus, BoardController boardController)
    {
        _boardController = boardController;
        _saveSystem = saveSystem;
        _signalBus = signalBus;
        Debug.Log("Link to SaveSystem in GameManager constructor");
    }
    
    public void Initialize()
    {
        _saveSystem.Initialize();
        SubscribeSignals();
        Score = _saveSystem.Data.Score;
        Debug.Log(_saveSystem.Data);
        CreateGame();
    }

    public void Dispose()
    {
        UnsubscribeSignals();
    }

    private void SubscribeSignals()
    {
        _signalBus.Subscribe<CreateGameSignal>(CreateGame);
        _signalBus.Subscribe<RestartGameSignal>(OnRestart);
        _signalBus.Subscribe<AddScoreSignal>(OnAddScore);
    }
    
    private void UnsubscribeSignals()
    {
        _signalBus.Unsubscribe<CreateGameSignal>(CreateGame);
        _signalBus.Unsubscribe<RestartGameSignal>(OnRestart);
        _signalBus.Unsubscribe<AddScoreSignal>(OnAddScore);
    }

    private void CreateGame()
    {
        if (_saveSystem.Data.BoardState == null || _saveSystem.Data.BoardState.Length == 0)
        {
            _boardController.Initialize();
        }
        else
        {
            _boardController.Initialize();
        }
    }
    
    private void OnAddScore(AddScoreSignal signal)
    {
        Score += signal.Value;
    }

    private void OnRestart()
    {
        Debug.Log("Restart");
    }
}
