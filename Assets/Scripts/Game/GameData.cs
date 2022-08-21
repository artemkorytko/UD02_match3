using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GameData : MonoBehaviour
    {
        public int Score;
        public string[] BoardState;
    }
}