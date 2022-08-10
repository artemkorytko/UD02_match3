using UnityEngine;
using Zenject;

namespace Game
{
    public class BaseUiElement : MonoBehaviour
    {
        protected SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
    }
}