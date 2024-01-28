using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

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


    [Header("Framing")] [HorizontalLine(color: EColor.Orange)] [SerializeField]
    private Transform FramingCenter;
    private float _framingDelta;

    [SerializeField] private float FramingScoreMultiplier;
    
    [Header("Dialogue")] [HorizontalLine(color: EColor.Orange)]
    public CharacterDialogueInfo DialogueInfo;
    
    public bool DialogueDone { get; set; }

    private void Awake()
    {
        foreach (var slot in Slots)
        {
            slot.Init();
        }
        
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

    public void AddExpressionModifier(int modifier)
    {
        int current = 0;
        switch (_currentFace.Type)
        {
            case ExpressionType.Negative:
                current = -1;
                break;
            case ExpressionType.Neutral:
                current = 0;
                break;
            case ExpressionType.Positive:
                current = 1;
                break;
        }

        int newVal = current + modifier;
        switch (newVal)
        {
            case <= -1:
                SwitchExpression(ExpressionType.Negative);
                break;
            case 0:
                SwitchExpression(ExpressionType.Neutral);
                break;
            case >= 1:
                SwitchExpression(ExpressionType.Positive);
                break;
        }

    }

    public void Equip(int id)
    {
        foreach (var slot in Slots)
        {
            if (slot.HasAccessory(id))
            {
                slot.Equip(id);
                return;
            }
        }
    }

    public List<Accessory> GetAllAccessories()
    {
        List<Accessory> list = new();
        foreach (var slot in Slots)
        {
            foreach (var acc in slot.Accessories)
            {
                list.Add(acc);
            }
        }

        return list;
    }

    public List<Accessory> GetNotEquippedAccessories()
    {
        
        List<Accessory> list = new();
        foreach (var slot in Slots)
        {
            foreach (var acc in slot.Accessories)
            {
                if(!acc.State)
                    list.Add(acc);
            }
        }

        return list;
    }

    private float CalculateAccessoryScore()
    {
        int sum = 0;
        foreach (var slot in Slots)
        {
            sum += slot.GetCurrentScore();
        }

        return sum * AccessoryScoreMultiplier;
    }

    private float CalculateExpressionScore()
    {
        var delta = Mathf.Abs(_currentFace.Type - DesiredExpressionType);
        var score = 100 - delta;
        return score * ExpressionScoreMultiplier;
    }

    public Vector2 GetFramingCenterScreenPosition()
    {
        return Camera.main.WorldToScreenPoint(FramingCenter.position);
    }
    
    public void SetFramingDelta(float delta)
    {
        _framingDelta = delta;
    }
    
    private float CalculateFramingScore()
    {
        return (100 - (_framingDelta)) * 0; //todo fix 0
    }
    
    public float CalculateCurrentScore()
    {
        var multiplierSum = AccessoryScoreMultiplier + ExpressionScoreMultiplier + 0; //todo fix 0
        var max = multiplierSum * 100;

        var current = CalculateAccessoryScore() + CalculateExpressionScore() + CalculateFramingScore();
        return current / max;
    }
}

[Serializable]
public class AccessorySlot
{
    public string Name;
    public List<Accessory> Accessories;
    public int EmptyScoreValue;

    public void Init()
    {
        foreach (var accessory in Accessories)
        {
            accessory.Init(this);
        }
    }
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
        UIManager.Instance.RefreshAccessoryButtons();
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
    Negative=0,
    Neutral=50,
    Positive=100
}

[Serializable]
public class CharacterDialogueInfo
{
    [ResizableTextArea]
    public string Introduction;
    public List<DialoguePart> DialogueParts;

    public DialoguePart GetDialoguePartById(int id)
    {
        foreach (var part in DialogueParts)
        {
            if (part.Id == id) return part;
        }

        return null;
    }

}
[Serializable]
public class DialoguePart
{
    public int Id;
    
    [ResizableTextArea]
    public string CharacterPhrase;
    public List<DialogueChoice> Choices;
}

[Serializable]
public class DialogueChoice
{
    [ResizableTextArea]
    public string Text;
    public int NextDialogueId;
    public int ExpressionModifier;
    public string AnimationCase;
}