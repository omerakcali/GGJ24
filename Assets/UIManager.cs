using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup RootCanvasGroup;    
    [SerializeField] private GameObject StoreSceneRoot;
    [SerializeField] private GameObject StudioSceneRoot;
    [SerializeField] private GameObject PhotoSceneRoot;

    [SerializeField] private CanvasGroup AddItemCanvasGroup;
    [SerializeField] private Transform AddItemPanelClosedPosition;
    [SerializeField] private Transform AddItemPanelOpenPosition;
    [SerializeField] private Transform AddItemPanel;
    [SerializeField] private AccessoryAddButton AccessoryButtonPrefab;
    [SerializeField] private Transform AccessoryButtonParent;

    private List<AccessoryAddButton> _accessoryAddButtons = new();

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Hide(Action onComplete =null, bool instant = false)
    {
        RootCanvasGroup.interactable = false;
        RootCanvasGroup.DOFade(0f, instant? 0f:.5f).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void Show(Action onComplete =null)
    {
        RootCanvasGroup.DOFade(1f, .5f).OnComplete(() =>
        {
            RootCanvasGroup.interactable = true;
            onComplete?.Invoke();
        });
    }
    
    public void SetStoreMode()
    {
        StoreSceneRoot.SetActive(true);
        StudioSceneRoot.SetActive(false);
        PhotoSceneRoot.SetActive(false);
    }

    public void SetStudioMode(ClientCharacter character)
    {
        StoreSceneRoot.SetActive(false);
        StudioSceneRoot.SetActive(true);
        PhotoSceneRoot.SetActive(false);
        
        RefreshAccessoryButtons();
    }

    public void RefreshAccessoryButtons()
    {
        foreach (var button in _accessoryAddButtons)
        {
            button.Reset();
        }
        
        var accessories = GameManager.Instance.CurrentClient.GetNotEquippedAccessories();
        if (_accessoryAddButtons.Count < accessories.Count)
        {
            var delta = accessories.Count - _accessoryAddButtons.Count;
            for (int i = 0; i < delta; i++)
            {
                var instance = Instantiate(AccessoryButtonPrefab, AccessoryButtonParent);
                instance.gameObject.SetActive(false);
                _accessoryAddButtons.Add(instance);
            }
        }

        for (int i = 0; i < accessories.Count; i++)
        {
            var button = _accessoryAddButtons[i];
            button.SetAccessory(accessories[i]);
        }
    }

    public void SetAddItemPanel(bool state)
    {
        if (state)
        {
            AddItemCanvasGroup.blocksRaycasts = true;
            AddItemPanel.transform.DOMove(AddItemPanelOpenPosition.position, .5f).OnComplete(() =>
            { 
                AddItemCanvasGroup.interactable = true;
            });
        }
        else
        {
            AddItemCanvasGroup.interactable = false;

            AddItemPanel.transform.DOMove(AddItemPanelClosedPosition.position, .5f).OnComplete(() =>
            {
                AddItemCanvasGroup.blocksRaycasts = false;
            });
        }
    }

    public void SetPhotoMode()
    {
        StoreSceneRoot.SetActive(false);
        StudioSceneRoot.SetActive(false);
        PhotoSceneRoot.SetActive(true);
    }
}
