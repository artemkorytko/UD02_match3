using UnityEngine;
using Zenject;

public class GameManager : IInitializable
{
    private SaveSystem _saveSystem;
    
    public GameManager(SaveSystem saveSystem)
    {
        _saveSystem = saveSystem;
        Debug.Log("Link to SaveSystem in GameManager constructor");
    }
    
    public void Initialize()
    {
        _saveSystem.Initialize();
        Debug.Log(_saveSystem.Data);
    }
}
