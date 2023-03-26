using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Leap.Unity;
using UnityEngine;
using UnityEngine.Networking;

public class APIConnector : MonoBehaviour
{
    //HOLOGRAM
    private Content _hologramContent;

    //LEAP MOTION
    [SerializeField] private HandModelBase rightHandModel, leftHandModel;
    [SerializeField] private float swipeSpeed = 2.0f;
    [SerializeField] private float swipeDistance = 2.0f;
    [SerializeField] private bool _isRightHandSwiping, _isLeftHandSwiping;
    [SerializeField] private Vector3 _rightSwipeStartPos, _leftSwipeStartPos;

    //ESP32
    [SerializeField] private string ip;
    [SerializeField] private string url;
    [SerializeField] private int pin;

    [SerializeField] private bool pinStatus;
    [SerializeField] private string editorMessage;

    [SerializeField] private bool canInput = true;
    private bool isHandShowed;
    private bool wasHandShowed;

    enum pinMode
    {
        INPUT,
        OUTPUT
    };
    enum pinNumber
    {
        D1,
        D2,
        D3,
        D4
    };
    enum pinState
    {
        HIGH,
        LOW
    };
    [Serializable] private struct Pin
    {
        public string name;
        public pinMode pinMode;
        public pinNumber pinNumber;
        public pinState pinState;
    }
    [SerializeField] private List<Pin> pins = new List<Pin>();

    void Start()
    {
        _hologramContent = FindObjectOfType<Content>();
        ResetAll();
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckForHands();
        //CheckForUserInputs();
    }

    public void OnButtonClicked()
    {
        StartCoroutine(SendRequest(editorMessage));
    }

    private void CheckForHands()
    {
        if (leftHandModel.isActiveAndEnabled || rightHandModel.isActiveAndEnabled)
        {
            
            isHandShowed = true;
            if (!wasHandShowed)
            {
                //user has its hands visible by the LeapMotion Device
                StartCoroutine(SendRequest("OnLed3"));
                _hologramContent.ActivateItem(_hologramContent.GetNextItem());
            }
        }
        else
        {
            //user has its hands out of the LeapMotion Device's vision
            isHandShowed = false;
            if(wasHandShowed) StartCoroutine(SendRequest("OffLed3"));
        }
        wasHandShowed = isHandShowed;
    }

    public IEnumerator SendRequest(string msg, [Optional] int delay)
    {
        //delay mainly use when turning all leds off in the beginning 
        yield return new WaitForSeconds(delay);
        Debug.Log($"start request : {msg}");
        //creates a http request with the action the ESP32 needs to operate (ex : msg = "OnLed1")
        var request = UnityWebRequest.Get($"http://{ip}/{msg}/");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError($"Could not connect to http://{ip}/{msg}/ : {request.error}");
        }
        else
        {
            var answer = request.downloadHandler.text;
            //print to the editor console the answer of the ESP32. For now it just prints the message sent by Unity.
            Debug.Log($"{answer}");
        }
    }

    private void CheckForUserInputs()
    {
        //Handle Left
        HandleLeft();
        HandleRight();
        
        
        //Handle Right
        
    }

    private void HandleLeft()
    {
        try
        {
            var leftHand = leftHandModel.GetLeapHand();
            if (!leftHandModel) return;
            print(leftHand.PalmPosition.ToString());
        
            if (leftHand.IsLeft && leftHand.PalmVelocity.x > swipeSpeed && !_isLeftHandSwiping)
            {
                _isLeftHandSwiping = true;
                _leftSwipeStartPos = leftHand.PalmPosition;
            }
            // Check if the swipe gesture has ended
            else if (_isLeftHandSwiping && Vector3.Distance(_leftSwipeStartPos, leftHand.PalmPosition) < -swipeDistance)
            {
                _isLeftHandSwiping = false;
                // Trigger your event here
                Debug.Log("Left hand swiped to the left!");
                _hologramContent.ActivateItem(_hologramContent.GetPreviousItem());
            }
        }
        catch
        {
            //ignore
        }
    }

    private void HandleRight()
    {
        try
        {
            var rightHand = rightHandModel.GetLeapHand();
            if (!rightHandModel) return;
        
            if (rightHand.IsRight && rightHand.PalmVelocity.x > swipeSpeed && !_isRightHandSwiping)
            {
                _isRightHandSwiping = true;
                _rightSwipeStartPos = rightHand.PalmPosition;
            }
            // Check if the swipe gesture has ended
            else if (_isRightHandSwiping && Vector3.Distance(_rightSwipeStartPos, rightHand.PalmPosition) > swipeDistance)
            {
                _isRightHandSwiping = false;
                // Trigger your event here
                Debug.Log("Right hand swiped to the right!");
                _hologramContent.ActivateItem(_hologramContent.GetNextItem());
            }
        }
        catch
        {
            //ignore
        }

    }

    private void ResetAll()
    {
        StartCoroutine(SendRequest("OffLed1"));
        StartCoroutine(SendRequest("OffLed2"));
        StartCoroutine(SendRequest("OffLed3"));
    }

 }

