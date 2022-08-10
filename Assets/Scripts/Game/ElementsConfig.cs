using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ElementsConfig", menuName = "Configs/ElementsConfig", order = 0)]
    public class ElementsConfig : ScriptableObject
    {
        [SerializeField] private ElementConfigItem[] configItem;

        public ElementConfigItem[] ConfigItem => configItem;
    }

    [System.Serializable]
    public class ElementConfigItem
    {
        [SerializeField] private string key;
        [SerializeField] private Sprite sprite;

        public string Key => key;

        public Sprite Sprite => sprite;
    }
}