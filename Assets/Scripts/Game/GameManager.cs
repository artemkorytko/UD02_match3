using System;
using UnityEngine;
using Zenject;

public class GameManager : IInitializable, IDisposable
{
    private SaveSystem _saveSystem;
    private SignalBus _signalBus;
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
    
    public GameManager(SaveSystem saveSystem, SignalBus signalBus)
    {
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
    }

    public void Dispose()
    {
        UnsubscribeSignals();
    }

    private void SubscribeSignals()
    {
        _signalBus.Subscribe<RestartGameSignal>(OnRestart);
        _signalBus.Subscribe<AddScoreSignal>(OnAddScore);
    }
    
    private void UnsubscribeSignals()
    {
        _signalBus.Unsubscribe<RestartGameSignal>(OnRestart);
        _signalBus.Unsubscribe<AddScoreSignal>(OnAddScore);
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
