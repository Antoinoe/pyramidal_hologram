using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemyBehaviour : MonoBehaviour
{
    [SerializeField] private float rotSpeed;

    private Transform _transform;
    private float _speed;
    void Start()
    {
        _transform = GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _speed = rotSpeed * Time.deltaTime;
        _transform.Rotate(0,_speed,0, Space.World);
    }
}
