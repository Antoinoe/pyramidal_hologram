using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private Content _content;
    private LeapServiceProvider _leap;
    private bool _isConnectedToLeap;
    private bool _canLeapSwipeLeft, _canLeapSwipeRight;

    private void Start()
    {
        _content = FindObjectOfType<Content>();
        _leap = FindObjectOfType<LeapServiceProvider>();
        _isConnectedToLeap = _leap.IsConnected();
        print(_isConnectedToLeap.ToString());
        _canLeapSwipeLeft = true;
        _canLeapSwipeRight = true;
    }

    private void Update()
    {
        HandleKeyboard();
    }

    private void HandleKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            _content.GetCurrentItem().RotateByUser();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            _content.ActivateItem(_content.GetNextItem());
    }
}