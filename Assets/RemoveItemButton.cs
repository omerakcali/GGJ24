using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveItemButton : MonoBehaviour
{
    [SerializeField] private Toggle Toggle;
    [SerializeField] private Sprite OnSprite, OffSprite;
    [SerializeField] private Image Image;
    
    private void Awake()
    {
        Toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool state)
    {
        Image.sprite = state ? OnSprite : OffSprite;

        GameManager.Instance.RemoveItemMode = state;
    }
}
