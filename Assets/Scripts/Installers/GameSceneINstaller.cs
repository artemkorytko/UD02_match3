using Configs;
using Game;
using UnityEngine;
using Zenject;

public class GameSceneINstaller : MonoInstaller
{
    [SerializeField] private Element elementPrefab; //ссылка на префаб
    [SerializeField] private GameObject ui;
    public override void InstallBindings()
    {
        Container.BindFactory<ElementPosition, ElementConfigItem, Element, Element.Factory>().FromComponentInNewPrefab(elementPrefab);
        //регистрация фактори, создание объекта, обращение к его конструктору, входящие в конструктор параметры, с помощью какоой фактори создается, из какого префаба создать экземпляр.
            
        Container.BindInstance(ui);
    }
}