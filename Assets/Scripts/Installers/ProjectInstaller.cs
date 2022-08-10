using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.Bind<SaveSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        BindSignals();
    }

    private void BindSignals()
    {
        Container.DeclareSignal<CreateGameSignal>();
        Container.DeclareSignal<OnElementClickSignal>();
        Container.DeclareSignal<ScoreChangedSignal>();
        Container.DeclareSignal<RestartGameSignal>();
        Container.DeclareSignal<AddScoreSignal>();
    }
}