using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NaughtyAttributes;
using UnityEngine;

public class ClientCharacter : MonoBehaviour
{
    [Header("Accessory")]
    [HorizontalLine( color: EColor.Orange)]
    [SerializeField] private List<AccessorySlot> Slots;
    public float AccessoryScoreMultiplier;

    [Header("Expression")]
    [HorizontalLine(color: EColor.Orange)]
    public ExpressionType DesiredExpressionType;
    public float ExpressionScoreMultiplier;
    private Dictionary<ExpressionType, ExpressionFace> _faces = new();
    private ExpressionFace _currentFace;

    [Header("Dialogue")] [HorizontalLine(color: EColor.Orange)]
    [ResizableTextArea]
    public string OpeningDialogue;

    private void Awake()
    {
        var expressions = GetComponentsInChildren<ExpressionFace>(true);

        foreach (var expression in expressions)
        {
            _faces[expression.Type] = expression;
            if (expression.gameObject.activeSelf) _currentFace = expression;
        }
    }

    public void SwitchExpression(ExpressionType type)
    {
        if(_currentFace.Type == type) return;
        _currentFace.gameObject.SetActive(false);
        _currentFace = _faces[type];
        _currentFace.gameObject.SetActive(true);
    }

    [Button()]
    private void TESTMakeSad()
    {
        SwitchExpression(ExpressionType.Negative);
    }
    
    [Button()]
    private void TESTMakeNeutral()
    {
        SwitchExpression(ExpressionType.Neutral);
    }
    
    [Button()]
    private void TESTMakeHappy()
    {
        SwitchExpression(ExpressionType.Positive);
    }
}

[Serializable]
public class AccessorySlot
{
    public string Name;
    public List<Accessory> Accessories;
    public int EmptyScoreValue;

    public bool HasAccessory(int id)
    {
        return Accessories.Any(x => x.Id == id);
    }

    public void Equip(int id)
    {
        foreach (var accessory in Accessories)
        {
            accessory.SetState(accessory.Id == id);
        }
    }

    public void TakeoffAll()
    {
        Equip(-1);
    }

    public int GetCurrentScore()
    {
        foreach (var accessory in Accessories)
        {
            if (accessory.State) return accessory.ScoreValue;
        }

        return EmptyScoreValue;
    }
}

public enum ExpressionType
{
    Negative,
    Neutral,
    Positive
}