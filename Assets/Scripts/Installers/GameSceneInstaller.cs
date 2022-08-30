using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Element elementPrefab;
    [SerializeField] private GameObject scoreView;
    
    public override void InstallBindings()
    {
        Container.BindFactory<ElementPosition, ElementConfigItem, Element, Element.Factory>().FromComponentInNewPrefab(elementPrefab);
        Container.BindInstance(scoreView);
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BoardController>().AsSingle().NonLazy();
    }
}