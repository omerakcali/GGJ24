using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIPolaroid : MonoBehaviour
{
    [SerializeField] private Image Content;
    [SerializeField] private CanvasGroup RootCanvasGroup;
    [SerializeField] private List<Image> Stars;

    public void Initialize(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture,
            new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        Content.sprite = sprite;
    }

    public void PlayShowAnimation(Action onComplete)
    {
        RootCanvasGroup.DOFade(1f, .5f).From(0f).OnStart(() =>
        { 
            transform.DOScale(1f, .5f);
        }).OnComplete(onComplete.Invoke);
    }

    public void DisplayScore(float score)
    {
        Debug.Log(score);
        int starCount = Mathf.CeilToInt(score * 5);
        for (int i = 0; i < starCount; i++)
        {
            Stars[i].gameObject.SetActive(true);
        }
    }
}
