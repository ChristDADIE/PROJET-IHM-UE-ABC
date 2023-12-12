using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    public Splash spawn;

    public Liquide liquid;

    public float DistanceSpawn = 3;

    public float minVelocityInSpawn = 10;



    void OnCollisionEnter()
    {
        if (transform.position.magnitude > DistanceSpawn)
            breaking();
        else if(GetComponent<Rigidbody>().velocity.magnitude > minVelocityInSpawn)
        {
            breaking();
        }
    }

    void breaking()
    {
        Splash splash = Instantiate(spawn);
        splash.transform.position = transform.position;
        splash.GetComponent<ParticleSystemRenderer>().material.color = liquid.Property.color;
#pragma warning disable CS0618
        splash.GetComponent<ParticleSystem>().startSpeed = 4 * liquid.Property.quantity;
        splash.GetComponent<ParticleSystem>().startSize = liquid.Property.quantity;
#pragma warning restore CS0618

        MainManager.main.GetComponent<LevelManager>().AOEDamage(transform.position, liquid.Property.quantity*2, 200);
        Destroy(this.gameObject);
        
    }


}
