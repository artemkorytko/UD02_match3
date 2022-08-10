using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public class RestartButton : BaseUiElement
    {
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _signalBus.Fire<RestartGameSignal>();
        }
    }
}