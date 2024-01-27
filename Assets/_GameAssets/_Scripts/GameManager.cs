using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public List<GameDayInfo> Days;
    
    private int _currentDay = 0;

    private Queue<ClientCharacter> _clientQueue = new();
    private ClientCharacter _lastClient;
    
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
    }

    [Button()]
    private void BringNextClient()
    {
        if(_lastClient != null)
            Destroy(_lastClient.gameObject);
        _lastClient = _clientQueue.Dequeue();
        _lastClient.transform.DOMove(ClientPosition.transform.position, 1f).OnStart(() =>
        {
            _lastClient.transform.DOScale(1f, 1f);
        });

        foreach (var queueClient in _clientQueue)
        {
            queueClient.transform.DOMoveX(queueClient.transform.position.x - LineDelta, 1f);
        }
    }
}
