using System;
using Zenject;

public class GameManager : IInitializable, IDisposable
{
    private readonly SaveSystem _saveSystem;
    private readonly SignalBus _signalBus;
    private readonly BoardController _boardController;
    private int _score = -1;
    private const int SCORE_MULTIPLIER = 10;

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
    }
    
    public void Initialize()
    {
        _saveSystem.Initialize();
        SubscribeSignals();
        Score = _saveSystem.Data.Score;
        CreateGame();
    }

    public void Dispose()
    {
        UnsubscribeSignals();
        _saveSystem.SaveData(_boardController.GetBoardState(), Score);
    }

    private void SubscribeSignals()
    {
        _signalBus.Subscribe<CreateGameSignal>(CreateGame);
        _signalBus.Subscribe<RestartGameSignal>(OnRestart);
        _signalBus.Subscribe<AddScoreSignal>(OnAddScore);
        _signalBus.Subscribe<OnBoardMatchSignal>(OnMatch);
    }
    
    private void UnsubscribeSignals()
    {
        _signalBus.Unsubscribe<CreateGameSignal>(CreateGame);
        _signalBus.Unsubscribe<RestartGameSignal>(OnRestart);
        _signalBus.Unsubscribe<AddScoreSignal>(OnAddScore);
        _signalBus.Unsubscribe<OnBoardMatchSignal>(OnMatch);
    }

    private async void CreateGame()
    {
        if (_saveSystem.Data.BoardState == null || _saveSystem.Data.BoardState.Length == 0)
        {
            await _boardController.Initialize();
            Score = 0;
        }
        else
        {
            await _boardController.Initialize(_saveSystem.Data.BoardState);
        }
    }
    
    private void OnAddScore(AddScoreSignal signal)
    {
        Score += signal.Value;
    }
    
    private void OnMatch(OnBoardMatchSignal matchCount)
    {
        _signalBus.Fire(new AddScoreSignal(SCORE_MULTIPLIER * matchCount.Value));
    }

    private async void OnRestart()
    {
        Score = 0;
        await _boardController.Restart();
    }
}
