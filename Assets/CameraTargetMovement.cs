using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetMovement : MonoBehaviour
{
    [SerializeField] private float Speed = 2f;
    [SerializeField] private Vector2 Bounds;
    private Vector2 _lastMousePosition;
    private void Awake()
    {
        enabled = false;
    }

    public void Enable()
    {
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
        transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) _lastMousePosition = Input.mousePosition;
        if(!Input.GetMouseButton(0)) return;
        var delta = (Vector2) Input.mousePosition - _lastMousePosition;
        transform.position += (Vector3) delta * (Speed * Time.deltaTime);
        _lastMousePosition = Input.mousePosition;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -Bounds.x, Bounds.x),
            Mathf.Clamp(transform.position.y, -Bounds.y, Bounds.y), 0);
    }
}
