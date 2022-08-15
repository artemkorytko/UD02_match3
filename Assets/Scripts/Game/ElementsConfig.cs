using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ElementsConfig", menuName = "Config/ElementsConfig", order = 0)] //название файла-конфига, название меню,  очередность менюшки 
    public class ElementsConfig : ScriptableObject
    {
        [SerializeField] private ElementConfigItem[] configItems;

        public ElementConfigItem[] ConfigItems => configItems;
    }

    [System.Serializable]
    public class ElementConfigItem
    {
        [SerializeField] private string key;
        [SerializeField] private Sprite sprite;

        public Sprite Sprite => sprite;
        public string Key => key;
    }
}