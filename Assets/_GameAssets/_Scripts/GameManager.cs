using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject StoreScene;
    [SerializeField] private GameObject StudioScene;
    [SerializeField] private Transform ClientPosition;
    [SerializeField] private Transform LinePosition;
    [SerializeField] private float LineDelta = 1.75f;

    [SerializeField] private CinemachineVirtualCamera MainCamera;
    [SerializeField] private CinemachineVirtualCamera PhotoCamera;
    [SerializeField] private CameraTargetMovement CameraTarget;
    [SerializeField] private Photo Photo;
    [SerializeField] private UIPolaroid PolaroidPrefab;
    [SerializeField] private Transform PolaroidAnimationParent;
    
    public List<GameDayInfo> Days;

    public ClientCharacter CurrentClient => _currentClient;
    
    private int _currentDay = 0;
    private Queue<ClientCharacter> _clientQueue = new();
    private ClientCharacter _currentClient;

    public static GameManager Instance;
    public bool RemoveItemMode { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    [Button]
    private void StartDay()
    {
        var day = Days[_currentDay];
        for (int i = 0; i < day.Characters.Count; i++)
        {
            var character = Instantiate(day.Characters[i]);
            _clientQueue.Enqueue(character);
            character.transform.position = LinePosition.position + Vector3.right * i * LineDelta;
            character.transform.localScale = Vector3.one*.35f;
        }
        UIManager.Instance.Hide(instant:true);
        UIManager.Instance.SetStoreMode();
    }

    [Button()]
    private void BringNextClient()
    {
        if(_currentClient != null)
            Destroy(_currentClient.gameObject);
        _currentClient = _clientQueue.Dequeue();
        _currentClient.transform.DOMove(ClientPosition.transform.position, 1f).OnStart(() =>
        {
            _currentClient.transform.DOScale(1f, 1f);
        }).OnComplete(()=>UIManager.Instance.Show());

        foreach (var queueClient in _clientQueue)
        {
            queueClient.transform.DOMoveX(queueClient.transform.position.x - LineDelta, 1f);
        }
    }
    public void StartStudioMode()
    {
        MainCamera.gameObject.SetActive(true);
        PhotoCamera.gameObject.SetActive(false);
        CameraTarget.Disable();
        StoreScene.gameObject.SetActive(false);
        StudioScene.gameObject.SetActive(true);
        UIManager.Instance.SetStudioMode();
        SetQueueVisibility(false);
    }

    public void StartPhotoshoot()
    {
        MainCamera.gameObject.SetActive(false);
        PhotoCamera.gameObject.SetActive(true);
        CameraTarget.Enable();
        
        UIManager.Instance.SetPhotoMode();

        /*StoreScene.gameObject.SetActive(false);
        StudioScene.gameObject.SetActive(true);
        UIManager.Instance.SetStudioMode(_currentClient);
        SetQueueVisibility(false);*/
    }

    public void ShootPhoto()
    {
        StartCoroutine(PhotoRoutine());
    }

    private IEnumerator PhotoRoutine()
    {
        UIManager.Instance.Hide();
        yield return Photo.CapturePhoto();
        yield return new WaitForSeconds(.25f);
        var photo = Photo.GetLastPhoto();
        var polaroid = Instantiate(PolaroidPrefab,PolaroidAnimationParent);
        polaroid.Initialize(photo);
        polaroid.PlayShowAnimation(OnShowAnimComplete);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        Destroy(polaroid.gameObject);
        void OnShowAnimComplete()
        {
            UIManager.Instance.SetShowingPolaroidMode();
            UIManager.Instance.Show();
        }
    }
    
    public void SetQueueVisibility(bool state)
    {
        foreach (var queuedClient in _clientQueue)
        {
            queuedClient.gameObject.SetActive(state);
        }
        
    }
}
