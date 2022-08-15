using System;
using Configs;
using Game;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Element : MonoBehaviour
{
    public class Factory : PlaceholderFactory<ElementPosition , ElementConfigItem, Element>
        //то какие элементы можно передавать в конструктор, последним элементоом указывается, к какому классу относится factory
    {
    }

    [SerializeField] private SpriteRenderer bgSpriteRenderer;
    [SerializeField] private SpriteRenderer iconSpriteRenderer;


    private Vector2 _localPosition;
    private Vector2 _gridPosition; 
    private ElementConfigItem _elementConfigItem;
    private SignalBus _signalBus;

    public string Key => _elementConfigItem.Key;
    public Vector2 GridPosition => _gridPosition;
    public bool IsActive { get; private set; }
    public bool IsInitialized { get; private set; }

    [Inject]
    public void Construct(ElementPosition elementPosition, ElementConfigItem elementConfigItem, SignalBus signalBus)
    {
        _localPosition = elementPosition.LocalPosition;
        _gridPosition = elementPosition.GridPosition;
        _signalBus = signalBus;
        _elementConfigItem = elementConfigItem;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        SetConfig();
        SetLocalPosition();
        Enable();
    }

    private void Enable() //включить спрайт
    {
        gameObject.SetActive(true);
        SetSelected(false);
        //DoTween logic
        IsActive = true;
        IsInitialized = true;
    }

    public void Disable()
    {
        //DoTween logic
        gameObject.SetActive(false);
    }
    private void SetLocalPosition() //выставить позицию
    {
        transform.localPosition = _localPosition;
    }

    private void SetConfig() //подключить конфиги
    {
        iconSpriteRenderer.sprite = _elementConfigItem.Sprite;
    }

    public void SetSelected(bool isOn) //background 
    {
        bgSpriteRenderer.enabled = isOn;
    }

    private void OnMouseUpAsButton() // MonoBehaviour method
    {
        OnClick();
    }

    private void OnClick() //клик по иконке
    {
        _signalBus.Fire(new OnElementClickSignal(this)); //вызвать сигнал у всех , кто подписан 
    }
}