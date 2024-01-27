using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup RootCanvasGroup;    
    [SerializeField] private Button StartShootButton;

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
        StartShootButton.gameObject.SetActive(true);
    }

    public void SetStudioMode()
    {
        StartShootButton.gameObject.SetActive(false);

    }

    public void SetPhotoMode()
    {
        StartShootButton.gameObject.SetActive(false);

    }
}
