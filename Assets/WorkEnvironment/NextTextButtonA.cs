using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputFeatureUsage;

public class NextTextButtonA : MonoBehaviour
{

    //public InputDeviceRole controllerRole = InputDeviceRole.RightHanded;
    //public InputDevice targetDevice;

    DisplayLongTextInBubble displayLongTextInBubble;
    public InputActionProperty nextText;

    void Awake()
    {
        displayLongTextInBubble = new DisplayLongTextInBubble();
    }

    void Start()
    {
        nextText.action.performed += cxt => 
        {
            displayLongTextInBubble.NextText();
        };
    }

}


