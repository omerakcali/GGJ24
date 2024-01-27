using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory : MonoBehaviour
{
    public int Id;
    public int ScoreValue;
    [SerializeField] private SpriteRenderer SpriteRenderer;

    public Sprite Sprite => SpriteRenderer.sprite;

    public bool State => SpriteRenderer.gameObject.activeSelf;
    
    public void SetState(bool state)
    {
        SpriteRenderer.gameObject.SetActive(state);
    }
}
