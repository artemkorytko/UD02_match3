using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementsConfig", menuName = "Configs/ElementsConfig", order = 0)]
public class ElementsConfig : ScriptableObject
{
    [SerializeField] private ElementConfigItem[] configItem;

    public ElementConfigItem[] ConfigItem => configItem;

    public ElementConfigItem GetByKey(string key)
    {
        return ConfigItem.FirstOrDefault(item => item.Key == key);
    }
}

[System.Serializable]
public class ElementConfigItem
{
    [SerializeField] private string key;
    [SerializeField] private Sprite sprite;

    public string Key => key;
    public Sprite Sprite => sprite;
}