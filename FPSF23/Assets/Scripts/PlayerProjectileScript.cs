using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileScript : MonoBehaviour
{
    private float spawnTime;
    public float lifetime = 3;
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Destroy(gameObject,lifetime); // this also just works
        if (Time.time > spawnTime+lifetime)
        { 
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Target"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (!collision.gameObject.name.Contains("Player"))
        { 
            Destroy(gameObject);
        }
    }
}
