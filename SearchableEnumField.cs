using System;
using RoboRyanTron.SearchableEnum.Editor;
using UnityEngine;
using UnityEngine.UIElements;

public class SearchableEnumField : Button
{
    private Type _enumType;

    private string[] _displayNames;

    private int _currentIndex = 0;

    private TextElement _textElement = new TextElement();
    private VisualElement _arrowElement = new VisualElement();

    public event Action OnItemSelectedEvent;

    public SearchableEnumField(Enum defaultValue, int width = 150)
    {
        this.style.width = width;

        Init();
        Populate(defaultValue);
        OnItemSelected(0);

        clicked += () => SearchablePopup.Show(this.worldBound, _displayNames, _currentIndex, OnItemSelected);
    }

    private void OnItemSelected(int index)
    {
        _currentIndex = index;
        _textElement.text = _displayNames[index];
        OnItemSelectedEvent?.Invoke();
    }

    public void SetValue(Enum value)
    {
        if (value.GetType() != _enumType)
        {
            Debug.LogError("Couldn't set Enum value, wrong Enum Type.");
            return;
        }

        for (int i = 0; i < _displayNames.Length; i++)
        {
            if (_displayNames[i] != value.ToString()) continue;

            _currentIndex = i;
            _textElement.text = _displayNames[i];
        }
    }

    public Enum GetValue()
    {
        return (Enum)Enum.Parse(_enumType, _displayNames[_currentIndex]);
    }

    private void Populate(Enum defaultValue)
    {
        _enumType = defaultValue.GetType();
        _displayNames = Enum.GetNames(defaultValue.GetType());
    }

    private void Init()
    {
        this.contentContainer.style.flexDirection = FlexDirection.Row;
        this.AddToClassList(EnumField.ussClassName);
        this._textElement.AddToClassList(EnumField.textUssClassName);
        this._textElement.pickingMode = PickingMode.Ignore;
        this.contentContainer.Add((VisualElement)this._textElement);
        this._arrowElement.AddToClassList(EnumField.arrowUssClassName);
        this._arrowElement.pickingMode = PickingMode.Ignore;
        this.contentContainer.Add(this._arrowElement);
    }
}
