using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using static DisplayTextInBubble;

[RequireComponent(typeof(DisplayTextInBubble))]
public class DisplayLongTextInBubble : MonoBehaviour
{

    ArrayList textChunks;
    int chunkIndex = 0;
    int maxIndex;
    
    
    void Start() 
    {
        DisplayLongText("Lorsqu on ajoute une solution basique dans une solution acide, le pH de la solution acide augmente. Les industriels utilisent cette technique appelée neutralisation de manière à obtenir des solutions neutres (pH = 7) avant de les rejeter à l’égout. Les ions hydrogène réagissent avec les ions hydroxyde pour donner de l’eau");
    }

    public void DisplayLongText(string text) 
    {
        textChunks = GetComponent<DisplayTextInBubble>().DivideLongText(text);
        maxIndex = textChunks.Count;
        GetComponent<DisplayTextInBubble>().Display((string)textChunks[chunkIndex]);
    }

    public void NextText() 
    {
        if (chunkIndex == maxIndex) 
        {
            GetComponent<DisplayTextInBubble>().StopDisplay();
        }

        else 
        {
            chunkIndex += 1;
            GetComponent<DisplayTextInBubble>().Display((string)textChunks[chunkIndex]);
        }
    }

    public void PreviousText() 
    {
        if (chunkIndex > 0) 
        {
            chunkIndex -= 1;
            GetComponent<DisplayTextInBubble>().Display((string)textChunks[chunkIndex]);
        }
    }

}