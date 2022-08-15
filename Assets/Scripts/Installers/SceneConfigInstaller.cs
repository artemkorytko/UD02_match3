using Configs;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SceneConfigInstaller", menuName = "Installers/SceneConfigInstaller")]
public class SceneConfigInstaller : ScriptableObjectInstaller<SceneConfigInstaller>
{
    [SerializeField] private ElementsConfig elementsConfig;
    public override void InstallBindings()
    {
        Container.BindInstance(elementsConfig); //есть BindInstances - можно биндить через запятую разные типы
    }
}