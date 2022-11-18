using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DimensionController : MonoBehaviour
{
    
    void Start()
    {
        print($" screen height : {Screen.height}");
        print($" screen width : {Screen.width}");
        //print($" screen resolution : {Screen.}");
        GetComponent<RectTransform>().sizeDelta = new Vector2(-Screen.height, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
