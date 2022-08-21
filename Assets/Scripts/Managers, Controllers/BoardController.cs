using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class BoardController : IDisposable
{
    private readonly BoardConfig _boardConfig;
    private readonly ElementsConfig _elementsConfig;
    private readonly Element.Factory _factory;
    private readonly SignalBus _signalBus;

    private Element[,] _elements;
    private Element _firstSelected;
    private bool _isBlocked;

    public BoardController(BoardConfig boardConfig, ElementsConfig elementsConfig, Element.Factory factory, SignalBus signalBus)
    {
        _boardConfig = boardConfig;
        _elementsConfig = elementsConfig;
        _factory = factory;
        _signalBus = signalBus;
    }
    
    public async UniTask Initialize(string[] dataBoardState = null)
    {
        if (dataBoardState == null)
        {
            await GenerateElements();
        }
        else
        {
            LoadElements(dataBoardState);
        }
        
        _signalBus.Subscribe<OnElementClickSignal>(OnElementClick);
    }
    
    public void Dispose()
    {
        _signalBus.Unsubscribe<OnElementClickSignal>(OnElementClick);
    }

    public string[] GetBoardState()
    {
        var array = new string[_boardConfig.SizeY * _boardConfig.SizeX];
        var row = _boardConfig.SizeX;
        var columns = _boardConfig.SizeY;
        var index = 0;
        
        for (var y = 0; y < columns; y++)
        {
            for (var x = 0; x < row; x++)
            {
                array[index++] = _elements[x, y].Key;
            }
        }

        return array;
    }

    public async UniTask Restart()
    {
        var row = _boardConfig.SizeX;
        var columns = _boardConfig.SizeY;

        for (var y = 0; y < columns; y++)
        {
            for (var x = 0; x < row; x++)
            {
                _elements[x, y].DestroySelf();
            }
        }

        _elements = null;
        await GenerateElements();
    }

    private async UniTask GenerateElements()
    {
        var row = _boardConfig.SizeX;
        var column = _boardConfig.SizeY;
        var elementOffset = _boardConfig.ElementOffset;
        
        var startPosition = new Vector2(-elementOffset * row * 0.5f + elementOffset * 0.5f,
                                         elementOffset * column * 0.5f - elementOffset * 0.5f);
        _elements = new Element[row, column];
        
        for (var y = 0; y < column; y++)
        {
            for (var x = 0; x < row; x++)
            {
                var position = startPosition + new Vector2(elementOffset * x, -elementOffset * y);
                var element = _factory.Create(new ElementPosition(position, new Vector2(x, y)),
                    GetPossibleElement(x, y, row, column));
                
                await element.Initialize();
                _elements[x, y] = element;
            }
        }            
    }
    
    private async void LoadElements(string[] dataBoardState)
    {
        var row = _boardConfig.SizeX;
        var column = _boardConfig.SizeY;
        var elementOffset = _boardConfig.ElementOffset;
        
        _elements = new Element[row, column];
        var startPosition = new Vector2(-elementOffset * row * 0.5f + elementOffset * 0.5f,
                                         elementOffset * column * 0.5f - elementOffset * 0.5f);

        for (var i = 0; i < dataBoardState.Length; i++)
        {
            var y = i / _boardConfig.SizeX;
            var x = i % _boardConfig.SizeX;
            var position = startPosition + new Vector2(elementOffset * x, -elementOffset * y);
            var element = _factory.Create(new ElementPosition(position, new Vector2(x, y)),
                _elementsConfig.GetByKey(dataBoardState[i]));
            await element.Initialize();
            _elements[x, y] = element;
        }
    }

    private ElementConfigItem GetPossibleElement(int row, int column, int rowCount, int columnCount)
    {
        var tempList = new List<ElementConfigItem>(_elementsConfig.ConfigItem);

        var x = row;
        var y = column - 1;

        if (x >= 0 && x < rowCount && y >= 0 && y < columnCount)
        {
            if (_elements[x, y].IsInitialized)
            {
                tempList.Remove(_elements[x, y].ConfigItem);
            }
        }

        x = row - 1;
        y = column;

        if (x >= 0 && x < rowCount && y >= 0 && y < columnCount)
        {
            if (_elements[x, y].IsInitialized)
            {
                tempList.Remove(_elements[x, y].ConfigItem);
            }                
        }
        
        return tempList[Random.Range(0, tempList.Count)];
    }

    private async void OnElementClick(OnElementClickSignal signal)
    {
        if (_isBlocked) return;

        var element = signal.Element;
        
        if (_firstSelected == null)
        {
            _firstSelected = element;
            element.SetSelected(true);
        }
        else
        {
            if (IsCanSwap(_firstSelected, element))
            {
                _firstSelected.SetSelected(false);
                await Swap(_firstSelected, element);
                _firstSelected = null;
                await CheckBoard();
            }
            else
            {
                if (_firstSelected == element)
                {
                    _firstSelected.SetSelected(false);
                    _firstSelected = null;
                }
                else
                {
                    _firstSelected.SetSelected(false);
                    _firstSelected = element;
                    element.SetSelected(true);
                }                    
            }
        }
    }
    
    private async UniTask CheckBoard()
    {
        _isBlocked = true;
        bool isNeedRecheck;
        List<Element> elementsForCollecting = new List<Element>();

        do
        {
            isNeedRecheck = false;
            elementsForCollecting.Clear();

            elementsForCollecting = SearchLines();

            if (elementsForCollecting.Count > 0)
            {
                await DisableElements(elementsForCollecting);
                _signalBus.Fire(new OnBoardMatchSignal(elementsForCollecting.Count));
                await NormalizeBoard();
                isNeedRecheck = true;
            }

        } while (isNeedRecheck);
        
        _isBlocked = false;
    }

    private List<Element> SearchLines()
    {
        List<Element> elementsForCollecting = new List<Element>();
        
        var row = _boardConfig.SizeX;
        var column = _boardConfig.SizeY;

        for (var y = 0; y < column; y++)
        {
            for (var x = 0; x < row; x++)
            {
                if (_elements[x, y].IsActive && !elementsForCollecting.Contains(_elements[x, y]))
                {
                    var needAddFirst = false;
                    List<Element> checkResult = CheckHorizontal(x, y);
                    
                    if (checkResult != null && checkResult.Count >= 2)
                    {
                        needAddFirst = true;
                        elementsForCollecting.AddRange(checkResult);
                    }

                    checkResult = CheckVertical(x, y);
                    if (checkResult != null && checkResult.Count >= 2)
                    {
                        needAddFirst = true;
                        elementsForCollecting.AddRange(checkResult);
                    }

                    if (needAddFirst)
                    {
                        elementsForCollecting.Add(_elements[x, y]);
                    }
                }
            }
        }

        return elementsForCollecting;
    }
    
    private List<Element> CheckHorizontal(int x, int y)
    {
        var row = _boardConfig.SizeX;

        var nextRow = x + 1;
        var nextColumn = y;

        if (nextRow >= row)
            return null;

        List<Element> elementsInLine = new List<Element>();
        Element mainElement = _elements[x, y];

        while (_elements[nextRow, nextColumn].IsActive && mainElement.Key == _elements[nextRow, nextColumn].Key)
        {
            elementsInLine.Add(_elements[nextRow, nextColumn]);
            if (nextRow + 1 < row)
            {
                nextRow++;
            }
            else
            {
                break;
            }
        }

        return elementsInLine;
    }
    
    private List<Element> CheckVertical(int x, int y)
    {
        var columns = _boardConfig.SizeY;

        var nextRow = x;
        var nextColumn = y + 1;

        if (nextColumn >= columns)
            return null;

        List<Element> elementsInLine = new List<Element>();
        Element mainElement = _elements[x, y];

        while (_elements[nextRow, nextColumn].IsActive && mainElement.Key == _elements[nextRow, nextColumn].Key)
        {
            elementsInLine.Add(_elements[nextRow, nextColumn]);
            if (nextColumn + 1 < columns)
            {
                nextColumn++;
            }
            else
            {
                break;
            }
        }

        return elementsInLine;
    }
    
    private async UniTask DisableElements(List<Element> elementsForCollecting)
    {
        foreach (var element in elementsForCollecting)
        {
            await element.Disable();
        }
    }
    
    private async UniTask NormalizeBoard()
    {
        var row = _boardConfig.SizeX;
        var column = _boardConfig.SizeY;

        for (var x = row - 1; x >= 0; x--)
        {
            List<Element> freeElements = new List<Element>();
            for (var y = column - 1; y >= 0; y--)
            {
                while (y >= 0 && !_elements[x, y].IsActive)
                {
                    freeElements.Add(_elements[x, y]);
                    y--;
                }

                if (y >= 0 && freeElements.Count > 0)
                {
                    await Swap(_elements[x, y], freeElements[0]);
                    freeElements.Add(freeElements[0]);
                    freeElements.RemoveAt(0);
                }
            }
        }

        for (var y = column - 1; y >= 0; y--)
        {
            for (var x = row - 1; x >= 0; x--)
            {
                if (!_elements[x, y].IsActive)
                {
                    GenerateRandomElement(_elements[x, y], row, column);
                    await _elements[x, y].Enable();
                }
            }
        }
    }

    private void GenerateRandomElement(Element element, int row, int column)
    {
        var gridPosition = element.GridPosition;
        var possibleElements = GetPossibleElement((int)gridPosition.x, (int)gridPosition.y, row, column);
        element.SetConfig(possibleElements);
    }
    
    private async UniTask Swap(Element firstElement, Element secondElement)
    {
        _elements[(int)firstElement.GridPosition.x, (int)firstElement.GridPosition.y] = secondElement;
        _elements[(int)secondElement.GridPosition.x, (int)secondElement.GridPosition.y] = firstElement;

        Vector2 secondPosition = secondElement.transform.localPosition;
        Vector2 secondGridPosition = secondElement.GridPosition;

        await secondElement.SetLocalPosition(firstElement.transform.localPosition, firstElement.GridPosition);
        await firstElement.SetLocalPosition(secondPosition, secondGridPosition);
    }
    
    private bool IsCanSwap(Element first, Element second)
    {
        var firstPosition = first.GridPosition;
        var secondPosition = second.GridPosition;
        
        var comparePosition = firstPosition;
        comparePosition.x += 1;
        
        if (comparePosition == secondPosition)
        {
            return true;
        }

        comparePosition = firstPosition;
        comparePosition.x -= 1;
        
        if (comparePosition == secondPosition)
        {
            return true;
        }
        
        comparePosition = firstPosition;
        comparePosition.y += 1;
        
        if (comparePosition == secondPosition)
        {
            return true;
        }
        
        comparePosition = firstPosition;
        comparePosition.y -= 1;
        
        if (comparePosition == secondPosition)
        {
            return true;
        }

        return false;
    }
}
    