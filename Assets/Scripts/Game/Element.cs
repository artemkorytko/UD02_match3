using UnityEngine;
using Zenject;

public class Element : MonoBehaviour
{
    public class Factory : PlaceholderFactory<ElementPosition, ElementConfigItem, Element>
    {
        
    }

    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
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

    [Inject]
    public void Construct(ElementPosition elementPosition, ElementConfigItem elementConfigItem, SignalBus signalBus)
    {
        _localPosition = elementPosition.LocalPosition;
        _gridPosition = elementPosition.GridPosition;
        _signalBus = signalBus;
        _configItem = elementConfigItem;
    }

    public void Initialize()
    {
        SetConfig();
        SetLocalPosition();
        Enable();
    }

    private void SetConfig()
    {
        iconSpriteRenderer.sprite = _configItem.Sprite;
    }
    
    public void SetConfig(ElementConfigItem element)
    {
        iconSpriteRenderer.sprite = element.Sprite;
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
    
    public void Enable()
    {
        gameObject.SetActive(true);
        SetSelected(false);
        //TODO animation DoTween
        IsActive = true;
        IsInitialized = true;
    }

    public void Disable()
    {
        //TODO animation 
        gameObject.SetActive(false);
        // CHECK !!!
    }

    public void SetSelected(bool isOn)
    {
        backgroundSpriteRenderer.enabled = isOn;
    }

    private void OnMouseUpAsButton()
    {
        OnClick();
    }

    public void OnClick()
    {
        _signalBus.Fire(new OnElementClickSignal(this));
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
