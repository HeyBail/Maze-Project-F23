using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    private float spawnTime;
    public float lifetime = 3;
    public float knockback = 3;
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
        if (collision.gameObject.name.Contains("Player"))
        {
            //damage player or just knock back lazer
            //collision.gameObject.GetComponent<CharacterController>().Move(new Vector3(GetComponent<Rigidbody>().velocity.x * knockback, 1f, GetComponent<Rigidbody>().velocity.z * knockback) * Time.deltaTime);
            collision.gameObject.GetComponent<FirstPersonMovement>().shot(1f, knockback, gameObject);
            Destroy(gameObject);
        }
        else if (!collision.gameObject.name.Contains("Enemey") && !collision.gameObject.name.Contains("Target") && !collision.gameObject.name.Contains("IceRod"))
        { 
            Destroy(gameObject);
        }
    }
}
