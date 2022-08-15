using System;

namespace Game.SaveSystem
{
    [Serializable]
    public class GameData
    {
        public int Score;
        public string[] BoardState;
    }
}