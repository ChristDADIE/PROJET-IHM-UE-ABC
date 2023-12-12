using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public GameObject menu; // Référence au GameObject du menu

    public AudioClip forestSound;
    public AudioClip desertSound;
    public AudioClip spaceSound;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        source = GetComponent<AudioSource>();
        source.PlayOneShot(forestSound);
        MenuHide();
    }

    public void PlayForestSound(){
        source.Stop();
        source.PlayOneShot(forestSound);
        MenuHide();
    }

    public void PlayDesertSound(){
        source.Stop();
        source.PlayOneShot(desertSound);
        MenuHide();
    }

    public void PlaySpaceSound(){
        source.Stop();
        source.PlayOneShot(spaceSound);
        MenuHide();
    }

    public void MenuDisplay(){
        if (menu != null){
            menu.SetActive(true);
            MenuHide();
        }
    }

    public void MenuHide(){
        if (menu != null){
            menu.SetActive(false);
            MenuHide();
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
