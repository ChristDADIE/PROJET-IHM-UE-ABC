using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public enum context
    {
        mainMenu,
        selection,
        level
    };

    context globalContext;
    public context GlobalContext
    {
        set
        {
            globalContext = value;
        }
    }


    void Start()
    {
        globalContext = context.mainMenu;
    }

    void StartLevel(int id)
    {
        globalContext = context.level;
        GetComponent<LevelManager>().Id = id;
        GetComponent<LevelManager>().StartLevel();
    }

    
    void Update()
    {
        
    }



}
