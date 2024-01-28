using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private List<DialoguePopupChoice> Choices;
    
    private CharacterDialogueInfo _currentDialogueInfo;

    public static DialoguePopup Instance;
    
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < Choices.Count; i++)
        {
            Choices[i].SetPopup(this);
            Choices[i].gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    
    public void SetDialogueInfo(CharacterDialogueInfo dialogueInfo)
    {
        _currentDialogueInfo = dialogueInfo;
    }

    public void ShowIntroduction()
    {
        DialogueText.text = _currentDialogueInfo.Introduction;
        gameObject.SetActive(true);
        Choices[0].SetAsNextButton();
    }

    public void ShowStudioDialogue()
    {
        SetDialogue(_currentDialogueInfo.DialogueParts[0]);
    }
    
    public void SetDialogue(DialoguePart dialoguePart)
    {
        gameObject.SetActive(true);
        DialogueText.text = dialoguePart.CharacterPhrase;

        foreach (var choice in Choices)
        {
            choice.gameObject.SetActive(false);
        }

        for (int i = 0; i < dialoguePart.Choices.Count; i++)
        {
            Choices[i].Initialize(dialoguePart.Choices[i]);
        }
    }

    public void Next()
    {
        GameManager.Instance.StartStudioMode();
        gameObject.SetActive(false);
    }

    public void ChoiceSelected(DialogueChoice currentChoice)
    {
        //todo handleChoice
        if (currentChoice.NextDialogueId == -1)
        {
            //todo close popup
            GameManager.Instance.CurrentClient.DialogueDone = true;
            gameObject.SetActive(false);
            UIManager.Instance.SetStudioMode();
            return;
        }

        GameManager.Instance.CurrentClient.AddExpressionModifier(currentChoice.ExpressionModifier);
        var nextInfo = _currentDialogueInfo.GetDialoguePartById(currentChoice.NextDialogueId);
        if (nextInfo == null)
        {
            Debug.LogError($"Error in next dialogue id:{currentChoice.NextDialogueId} Not FOUND");
            //todo close popup
            gameObject.SetActive(false);
            return;
        }
        
        SetDialogue(nextInfo);
    }
}
