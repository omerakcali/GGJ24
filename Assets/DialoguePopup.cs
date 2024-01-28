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

    private void Awake()
    {
        for (int i = 0; i < Choices.Count; i++)
        {
            Choices[i].SetPopup(this);
        }
    }


    public void SetDialogueInfo(CharacterDialogueInfo dialogueInfo)
    {
        _currentDialogueInfo = dialogueInfo;
    }

    public void ShowIntroduction()
    {
        DialogueText.text = _currentDialogueInfo.Introduction;
        //todo show okayButton;
    }

    public void ShowStudioDialogue()
    {
        SetDialogue(_currentDialogueInfo.DialogueParts[0]);
    }
    
    public void SetDialogue(DialoguePart dialoguePart)
    {
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

    public void ChoiceSelected(DialogueChoice currentChoice)
    {
        //todo handleChoice
        if (currentChoice.NextDialogueId == -1)
        {
            //todo close popup
            gameObject.SetActive(false);
            return;
        }

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
