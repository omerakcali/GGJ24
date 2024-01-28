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
    private bool _nextButton;
    
    private void Awake()
    {
        Button.onClick.AddListener(OnClick);
    }

    public void SetAsNextButton()
    {
        gameObject.SetActive(true);
        _nextButton = true;
        Text.text = "Continue";
    }
    private void OnClick()
    {
        if(_nextButton) _popup.Next();
        else _popup.ChoiceSelected(_currentChoice);
    }

    public void SetPopup(DialoguePopup popup)
    {
        _popup = popup;
    }

    public void Initialize(DialogueChoice choice)
    {
        _nextButton = false;
        gameObject.SetActive(true);
        Text.text = choice.Text;
        _currentChoice = choice;
    }
}
