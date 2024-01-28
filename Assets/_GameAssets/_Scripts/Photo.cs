using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
public class Photo : MonoBehaviour
{
    [SerializeField] private RectTransform PhotoCaptureFrame;
    [SerializeField] private Image FlashImage;
    
    private Texture2D _lastPhoto;
    private List<Texture2D> _photos = new();

    public IEnumerator CapturePhoto()
    {
        _lastPhoto = new Texture2D(700, 700, TextureFormat.RGB24, false);
        PhotoCaptureFrame.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        Rect rect = new Rect(PhotoCaptureFrame.position, PhotoCaptureFrame.rect.size);
        _lastPhoto.ReadPixels(rect,0,0,false);
        _lastPhoto.Apply();
        _photos.Add(_lastPhoto);
        PhotoCaptureFrame.gameObject.SetActive(true);
        FlashImage.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        FlashImage.color = Color.clear;

    }

    public Texture2D GetLastPhoto()
    {
        return _lastPhoto;
    }
}
