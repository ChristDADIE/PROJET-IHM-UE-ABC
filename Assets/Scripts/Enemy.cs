using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public string enemyName;
    public float baseHealth;
    public float baseSpeed;

    public float attack;
    public float attackSpeed;

    public float baseRange;
    Vector3 objective;

    LevelManager level;
    



    [System.NonSerialized]
    public bool isDead;
    void Start()
    {
        isDead = false;
    }

    public void Setup(LevelManager level, int factor,Vector3 position)
    {
        this.level = level;
        baseHealth *= factor;
        baseSpeed *= factor;
        transform.position = position;
        float size = Mathf.Sqrt(factor);
        ((RectTransform)transform).localScale = new Vector3(size, size, size);
        baseRange *= size;
        cooldown = 0;
    }

    public void Damage(float amount,string type)
    {
        baseHealth -= amount;
        if (baseHealth <= 0)
            Death();
    }

    void Death()
    {
        isDead = true;
    }

    

    float cooldown;
    void FixedUpdate()
    {
        if((transform.position-objective).magnitude < baseRange)
        {
            cooldown += Time.fixedDeltaTime;
            if(cooldown > 1/attackSpeed)
            {
                // deal damages
                cooldown -= 1 / attackSpeed;
            }
        }
        else
        {
            transform.position += (objective - transform.position).normalized * baseSpeed * Time.fixedDeltaTime;
            transform.forward = objective;
        }
    }
}
