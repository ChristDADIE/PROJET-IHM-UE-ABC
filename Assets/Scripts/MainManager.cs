using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    enum context
    {
        mainMenu,
        selection,
        level
    };

    context globalContext;


    void Start()
    {
        globalContext = context.mainMenu;
    }

    
    void Update()
    {
        
    }



}
