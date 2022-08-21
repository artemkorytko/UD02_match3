using Game;
using Scripts;
using Scripts.Signals;
using Signals;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private Element elementPrefab;
        [SerializeField] private GameObject ui;

        public override void InstallBindings()
        {
            BindSignals();
            Container.BindInstance(ui);
            Container.BindFactory<ElementPosition, ElementConfigItem, Element, Element.Factory>().FromComponentInNewPrefab(elementPrefab);
            Container.BindInterfacesAndSelfTo<BoardController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        }

        private void BindSignals()
        {
            Container.DeclareSignal<CreateGameSignal>();
            Container.DeclareSignal<OnElementClickSignal>();
            Container.DeclareSignal<ScoreChangedSignal>();
            Container.DeclareSignal<RestartGameSignal>();
            Container.DeclareSignal<AddScoreSignal>();
            Container.DeclareSignal<OnBoardMatchSignal>();
            Container.DeclareSignal<OnBoardClosedSignal>();
        }
    }
}