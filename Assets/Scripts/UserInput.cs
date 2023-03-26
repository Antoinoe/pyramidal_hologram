using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    [SerializeField] 
    private float leapSwipeDelay = 2.0f;
    
    private Content _content;
    private LeapServiceProvider _leap;
    private bool _isConnectedToLeap;
    private bool _canLeapSwipeLeft, _canLeapSwipeRight;

    private void Start()
    {
        _content = FindObjectOfType<Content>();
        _leap = FindObjectOfType<LeapServiceProvider>();
        _isConnectedToLeap = _leap.IsConnected();
        print(_isConnectedToLeap);
        _canLeapSwipeLeft = true;
        _canLeapSwipeRight = true;
    }

    private void Update()
    {
        HandleKeyboard();
        StartCoroutine(HandleLeapMotion());
    }

    private void HandleKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            _content.GetCurrentItem().RotateByUser();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            _content.ActivateItem(_content.GetNextItem());
    }

    private IEnumerator HandleLeapMotion()
    {
        
        if (!_isConnectedToLeap)yield break;
        if (_canLeapSwipeLeft)
        {
            _canLeapSwipeLeft = false;
            print("left");
            //code here
            yield return new WaitForSeconds(leapSwipeDelay);
            _canLeapSwipeLeft = true;
        }
        if (_canLeapSwipeRight)
        {
            _canLeapSwipeRight = false;
            print("right");
            //code here
            yield return new WaitForSeconds(leapSwipeDelay);
            _canLeapSwipeRight = true;
        }
    }
}