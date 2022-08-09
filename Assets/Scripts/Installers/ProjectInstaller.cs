using Game.Managers;
using Game.SaveSystem;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SaveSystem>().AsSingle().NonLazy(); //создаст экземпляр класса SaveSystem в единичном виде(синглтон), NonLazy - создаст при старет
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy(); //BindInterfacesAndSelfTo - бинд интерфейсов, от которых мы наследуемся, создание экземпляра GameManager
    }
}