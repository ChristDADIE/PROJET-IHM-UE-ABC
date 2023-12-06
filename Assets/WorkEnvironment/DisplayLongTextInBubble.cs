using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using static DisplayTextInBubble;

public class DisplayLongTextInBubble : MonoBehaviour
{
    DisplayTextInBubble displayTextInBubble = new DisplayTextInBubble();

    ArrayList textChunks;
    int chunkIndex = 0;
    int maxIndex;

    public void DisplayLongText(string text) 
    {
        textChunks = displayTextInBubble.DivideLongText(text);
        maxIndex = textChunks.Count;
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
