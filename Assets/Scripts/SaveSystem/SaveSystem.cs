using UnityEngine;

public class SaveSystem
{
    private GameData _gameData;
    private const string DATA_KEY = "GameData";

    public GameData Data => _gameData;

    public void Initialize()
    {
        if (PlayerPrefs.HasKey(DATA_KEY))
        {
            LoadData();
        }
        else
        {
            _gameData = new GameData();
        }
    }

    private void LoadData()
    {
        var jsonData = PlayerPrefs.GetString(DATA_KEY);
        _gameData = JsonUtility.FromJson<GameData>(jsonData);
    }

    public int LoadScore()
    {
        return _gameData.Score;
    }

    public void SaveData(string[] boardToSave, int score)
    {
        _gameData.Score = score;
        _gameData.BoardState = boardToSave;
        var jsonData = JsonUtility.ToJson(_gameData);
        PlayerPrefs.SetString(DATA_KEY, jsonData);
    }
}
