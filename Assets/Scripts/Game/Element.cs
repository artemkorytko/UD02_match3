using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Element : MonoBehaviour
{
    private const float ANIMATION_TIME = 0.5f;

    public class Factory : PlaceholderFactory<ElementPosition, ElementConfigItem, Element>
    {
    }

    [SerializeField] private SpriteRenderer bgSpriteRenderer;
    [SerializeField] private SpriteRenderer iconSpriteRenderer;

    private Vector2 _localPosition;
    private Vector2 _gridPosition;
    private ElementConfigItem _configItem;
    private SignalBus _signalBus;
    public string Key => _configItem.Key;
    public Vector2 GridPosition => _gridPosition;
    public bool IsActive { get; private set; }
    public bool IsInitialized { get; private set; }

    public ElementConfigItem ConfigItem => _configItem;
    private Vector2 _startLocalScale;

    [Inject]
    public void Construct(ElementPosition elementPosition, ElementConfigItem configItem, SignalBus signalBus)
    {
        _localPosition = elementPosition.LocalPosition;
        _gridPosition = elementPosition.GridPosition;
        _signalBus = signalBus;
        _configItem = configItem;
    }

    public void Initialize()
    {
        _startLocalScale = transform.localScale;
        SetConfig();
        SetLocalPosition();
        Enable();
    }

    public void SetConfig()
    {
        iconSpriteRenderer.sprite = _configItem.Sprite;
    }

    private void SetLocalPosition()
    {
        transform.localPosition = _localPosition;
    }

    public void SetLocalPosition(Vector2 newLocalPosition, Vector2 gridPosition)
    {
        transform.localPosition = newLocalPosition;
        _gridPosition = gridPosition;
    }

    public async UniTask Enable()
    {
        IsActive = true;
        gameObject.SetActive(true);
        SetSelected(false);
        transform.localScale = Vector3.zero;
        IsInitialized = true;
        await transform.DOScale(_startLocalScale, ANIMATION_TIME);
       
    }

    public async UniTask Disable()
    {
        IsActive = false;
        await transform.DOScale(Vector3.zero, ANIMATION_TIME);
        gameObject.SetActive(false);
    }

    public void SetSelected(bool isOn)
    {
        bgSpriteRenderer.enabled = isOn;
    }

    private void OnMouseUpAsButton()
    {
        OnClick();
    }

    private void OnClick()
    {
        _signalBus.Fire(new OnElementClickSignal(this));
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void SetConfig(ElementConfigItem elements)
    {
        _configItem = elements;
        iconSpriteRenderer.sprite = elements.Sprite;
    }
}