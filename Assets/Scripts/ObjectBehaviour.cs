using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
{
    [SerializeField] private float rotSpeed;
    [SerializeField] private float userRotationForce;
    [SerializeField] public int ledNumber; 
    private Rigidbody _rb;
    private bool _canIdleRotate;
    private Transform _transform;
    private float _speed;
    private APIConnector _api;
    public bool isEnabled = false;
    private bool _wasEnabled = false;

    void Start()
    {
        _canIdleRotate = true;
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _api = FindObjectOfType<APIConnector>();
        print(_api);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_canIdleRotate) return;
        _speed = rotSpeed * Time.deltaTime;
        _transform.Rotate(0,_speed,0, Space.World);
    }

    public void RotateByUser()
    {
        _rb.AddTorque(Vector3.right * userRotationForce, ForceMode.Impulse);
    }

    // private void OnEnable()
    // {
    //     if (!_api) return;
    //     StartCoroutine(_api.SendRequest($"OnLed{ledNumber.ToString()}", 1));
    // }
    //
    // private void OnDisable()
    // {
    //     if (!_api) return;
    //     print($"disable {name}");
    //     StartCoroutine(_api.SendRequest($"OffLed{ledNumber.ToString()}", 1));
    // }
}
