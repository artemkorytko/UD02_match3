using Game.Managers;
using Game.SaveSystem;
using Signals;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container); //можно создавать бесконечное количество классов-сигналов, подписываться и отписываться на классы-сигналы
        Container.Bind<SaveSystem>().AsSingle().NonLazy(); //создаст экземпляр класса SaveSystem в единичном виде(синглтон), NonLazy - создаст при старте
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy(); //BindInterfacesAndSelfTo - бинд интерфейсов, от которых мы наследуемся, создание экземпляра GameManager
        BindSignals();
    }

    private void BindSignals() //аналог событий - экшенов 
    {
        Container.DeclareSignal<CreateGameSignal>();
        Container.DeclareSignal<OnElementClickSignal>();
        Container.DeclareSignal<ScoreChangedSignal>();
        Container.DeclareSignal<ReStartGameSignal>();
    }
}