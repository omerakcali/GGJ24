using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
public class Photo : MonoBehaviour
{
    [Header("Photo Taker")] [SerializeField]
    private Image photoDisplayArea;

    [SerializeField] private GameObject photoFrame;
    
    private Texture2D screenCapture;
    private bool viewingPhoto;

    void Start()
    {
        screenCapture = new Texture2D(700, 700, TextureFormat.RGB24, false);
    }

    [Button()]
    private void TakePhoto()
    {
        if (!viewingPhoto)
        {
            StartCoroutine(CapturePhoto());
        }
        else
        {
            RemovePhoto();
        }
    }
    IEnumerator CapturePhoto()
    {
        viewingPhoto = true;
        
        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, 700, 700);
        screenCapture.ReadPixels(regionToRead,0,0,false);
        screenCapture.Apply();
        ShowPhoto();
    }

    void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture,
            new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;
        photoDisplayArea.DOFade(1f, .5f).From(0f);
        photoFrame.SetActive(true);
    }

    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);
        
        // UI 
    }
}
