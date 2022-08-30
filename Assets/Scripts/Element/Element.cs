using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class Element : MonoBehaviour
{
    public class Factory : PlaceholderFactory<ElementPosition, ElementConfigItem, Element>
    {
        
    }

    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
    [SerializeField] private SpriteRenderer iconSpriteRenderer;
    
    private readonly Vector3 effectSmallScale = Vector3.zero;
    private readonly Vector3 effectNormalScale = Vector3.one;
    
    
    private Vector2 _localPosition;
    private Vector2 _gridPosition;
    private ElementConfigItem _configItem;
    private ElementsConfig _elementsConfig;
    private SignalBus _signalBus;

    public string Key => _configItem.Key;
    public Vector2 GridPosition => _gridPosition;
    public bool IsActive { get; private set; }
    public bool IsInitialized { get; private set; }

    public ElementConfigItem ConfigItem => _configItem;

    [Inject]
    public void Construct(ElementPosition elementPosition, ElementConfigItem elementConfigItem, SignalBus signalBus, ElementsConfig elementsConfig)
    {
        _localPosition = elementPosition.LocalPosition;
        _gridPosition = elementPosition.GridPosition;
        _signalBus = signalBus;
        _configItem = elementConfigItem;
        _elementsConfig = elementsConfig;
    }

    public async UniTask Initialize()
    {
        SetConfig();
        SetLocalPosition();
        await Enable();
    }

    private void SetConfig()
    {
        iconSpriteRenderer.sprite = _configItem.Sprite;
    }
    
    public void SetConfig(ElementConfigItem element)
    {
        _configItem = element;
        iconSpriteRenderer.sprite = element.Sprite;
    }
    
    private void SetLocalPosition()
    {
        transform.localPosition = _localPosition;
    }
    
    public async UniTask SetLocalPosition(Vector2 newLocalPosition, Vector2 gridPosition)
    {
        await transform.DOMove(newLocalPosition, _elementsConfig.EffectsDuration).OnComplete(() =>
        {
            _gridPosition = gridPosition;
        });
    }
    
    public async UniTask Enable()
    {
        gameObject.SetActive(true);
        SetSelected(false);
        
        transform.localScale = effectSmallScale;
        
        await transform.DOScale(effectNormalScale, _elementsConfig.EffectsDuration).OnComplete(() =>
        {
            IsActive = true;
            IsInitialized = true;
        });
    }

    public async UniTask Disable()
    {
        await transform.DOScale(effectSmallScale, _elementsConfig.EffectsDuration).OnComplete(() =>
        {
            IsActive = false;
            gameObject.SetActive(false);
        });
    }

    public void SetSelected(bool isOn)
    {
        backgroundSpriteRenderer.enabled = isOn;
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
}
