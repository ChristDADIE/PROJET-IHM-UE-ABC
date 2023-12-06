using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string name;
    public float baseHealth;
    public float baseSpeed;



    [System.NonSerialized]
    public bool isDead;
    void Start()
    {
        isDead = false;
    }

    public void Setup(int factor,Vector3 position)
    {
        baseHealth *= factor;
        baseSpeed *= factor;
        transform.position = position;
    }

    
    void Update()
    {
        
    }
}
