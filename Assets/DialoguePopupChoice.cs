using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePopupChoice : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private TextMeshProUGUI Text;
    
    private DialoguePopup _popup;
    private DialogueChoice _currentChoice;

    private void Awake()
    {
        Button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _popup.ChoiceSelected(_currentChoice);
    }

    public void SetPopup(DialoguePopup popup)
    {
        _popup = popup;
    }

    public void Initialize(DialogueChoice choice)
    {
        Text.text = choice.Text;
        _currentChoice = choice;
    }
}
