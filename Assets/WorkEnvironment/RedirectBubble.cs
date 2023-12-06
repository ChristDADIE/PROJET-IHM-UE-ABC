using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectBubble : MonoBehaviour
{

    public Transform targetCamera;
    public GameObject canvas;


    void Update()
    {
        canvas.transform.LookAt(targetCamera);
    }
}
