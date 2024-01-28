using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(PolygonCollider2D))]
public class Accessory : MonoBehaviour
{
    public int Id;
    public int ScoreValue;
    [SerializeField] private SpriteRenderer SpriteRenderer;

    public Sprite Icon;

    private TextMeshProUGUI a;
    
    public Sprite Sprite => Icon ? Icon : SpriteRenderer.sprite;
    public bool State => SpriteRenderer.gameObject.activeSelf;

    private AccessorySlot _slot;
    
    public void Init(AccessorySlot slot)
    {
        _slot = slot;
    }
    
    public void SetState(bool state)
    { 
        SpriteRenderer.color = Color.white;
        SpriteRenderer.gameObject.SetActive(state);
        
    }

    private void OnValidate()
    {
        if(SpriteRenderer == null) 
            SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        if(!GameManager.Instance.RemoveItemMode) return;
        SpriteRenderer.color = Color.cyan;
        
    }

    private void OnMouseExit()
    {
        if(!GameManager.Instance.RemoveItemMode) return;
        SpriteRenderer.color = Color.white;
    }

    private void OnMouseUpAsButton()
    {
        if(!GameManager.Instance.RemoveItemMode) return;
        _slot.TakeoffAll();
    }
}
