using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryAddButton : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private Image Image;

    private int _id;
    
    public void SetAccessory(Accessory accessory)
    {
        _id = accessory.Id;
        Image.sprite = accessory.Sprite;
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        _id = -1;
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        Button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if(_id == -1) return;
        GameManager.Instance.CurrentClient.Equip(_id);
    }
}
