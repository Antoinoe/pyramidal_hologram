using System.Collections;
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
    //ESP32
    [SerializeField] private string ip;
    [SerializeField] private string editorMessage;
    
    private bool _isHandShowed;
    private bool _wasHandShowed;
    void Start()
    {
        _hologramContent = FindObjectOfType<Content>();
        ResetAll();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForHands();
    }

    public void OnButtonClicked()
    {
        //just a debug function, there was a button in the scene back in the days...
        StartCoroutine(SendRequest(editorMessage));
    }

    private void CheckForHands()
    {
        if (leftHandModel.isActiveAndEnabled || rightHandModel.isActiveAndEnabled)
        {
            
            _isHandShowed = true;
            if (!_wasHandShowed)
            {
                //user has its hands visible by the LeapMotion Device
                StartCoroutine(SendRequest("OnLed3"));
                _hologramContent.ActivateItem(_hologramContent.GetNextItem());
            }
        }
        else
        {
            //user has its hands out of the LeapMotion Device's vision
            _isHandShowed = false;
            if(_wasHandShowed) StartCoroutine(SendRequest("OffLed3"));
        }
        _wasHandShowed = _isHandShowed;
    }

    public IEnumerator SendRequest(string msg, [Optional] float delay)
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
    private void ResetAll()
    {
        StartCoroutine(SendRequest("OffLed1",0.5f));
        StartCoroutine(SendRequest("OffLed2",0.5f));
        StartCoroutine(SendRequest("OffLed3",0.5f));
    }

 }

