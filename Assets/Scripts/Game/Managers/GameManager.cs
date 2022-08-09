using UnityEngine;
using Zenject;

namespace Game.Managers
{
    public class GameManager: IInitializable
    {
        private readonly SaveSystem.SaveSystem _saveSystem;

        public GameManager(SaveSystem.SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
        }
        public void Initialize() //аналог метода старт
        {
            _saveSystem.Initialize();
            Debug.Log("here");
        }
    }
}