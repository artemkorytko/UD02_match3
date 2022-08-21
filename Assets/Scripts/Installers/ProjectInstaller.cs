using Game;
using Scripts.Signals;
using Signals;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.Bind<SaveSystem>().AsSingle().NonLazy();
        }

    }
}