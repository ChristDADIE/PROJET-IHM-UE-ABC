using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    public GameObject spawn;

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
        Instantiate(spawn);
        Destroy(this);
    }


}
