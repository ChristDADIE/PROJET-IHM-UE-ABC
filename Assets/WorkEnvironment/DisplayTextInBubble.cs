using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DisplayTextInBubble : MonoBehaviour
{
    public float cooldown = 3f; // Temps d'affichage de la bulle de texte en secondes
    const float timeDisplay = 1000f;

    [SerializeField] GameObject bubble;
    [SerializeField] TextMeshProUGUI textMeshPro;


    int maxChar = 55;
    


    void Start()
    {
        // Bulle et texte invisibles au départ
        bubble.SetActive(false);
        textMeshPro.enabled = false;

        Display("hola mi amigo como estas hoy muy bien y tu muy bine tambien muchas gracias");
    }


    void StartDisplay(string text)
    {
        bubble.SetActive(true);

        textMeshPro.SetText(text);
        textMeshPro.enabled = true;
    }

    public void StopDisplay() 
    {
        bubble.SetActive(false);
        textMeshPro.enabled = false;
    }

    public void Display(string text)
    {
        // On initialise le timer et on affiche la bulle de texte
        cooldown = timeDisplay;
        StartDisplay(text);
    }

    void Update()
    {
        // Tant que le temps d'affichage n'est pas écoulé on continue d'afficher 
        // et on décrémente le timer
        if (cooldown >= 0) 
        {
            cooldown -= Time.deltaTime;
        }
        else 
        {
            StopDisplay();
        }
    }



    public ArrayList DivideLongText(string text) {

        ArrayList textChunks = new ArrayList();

        string[] words = text.Split(' ');

        string chunk = "";
        int length = 0; 

        foreach (string word in words) 
        {
            if (length + word.Length > maxChar) 
            {
                textChunks.Add(chunk);
                chunk = "";    // est-ce que ça marche ?
                length = 0;
            }

            else 
            {
                chunk = string.Concat(chunk, string.Concat(word, ' '));  // Remet un espace après chaque mot
                length += word.Length;
            }
        }

        return textChunks;
    }


}

