using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DisplayLongTextInBubble
{
    DisplayTextInBubble displayTextInBubble;

    ArrayList textChunks;
    int chunkIndex = 0;
    int maxIndex;


    void Awake()
    {
        displayTextInBubble = new DisplayTextInBubble();
    }

    void Start() 
    {
        DisplayLongText("Lorsqu’on ajoute une solution basique dans une solution acide, le pH de la solution acide augmente. Les industriels utilisent cette technique appelée neutralisation de manière à obtenir des solutions neutres (pH = 7) avant de les rejeter à l’égout. Les ions hydrogène réagissent avec les ions hydroxyde pour donner de l’eau");
    }

    public void DisplayLongText(string text) 
    {
        textChunks = displayTextInBubble.DivideLongText(text);
        maxIndex = textChunks.Count - 1;
        displayTextInBubble.Display((string)textChunks[chunkIndex]);
    }

    public void NextText() 
    {
        if (chunkIndex == maxIndex) 
        {
            displayTextInBubble.StopDisplay();
        }

        else 
        {
            chunkIndex += 1;
            displayTextInBubble.Display((string)textChunks[chunkIndex]);
        }
    }

    public void PreviousText() 
    {
        if (chunkIndex > 0) 
        {
            chunkIndex -= 1;
            displayTextInBubble.Display((string)textChunks[chunkIndex]);
        }
    }

}
