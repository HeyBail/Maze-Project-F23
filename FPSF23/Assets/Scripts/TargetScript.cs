using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private UIManager uIManager;
    public GameObject player;
    public GameObject projectile;
    public float bulletSpeed = 30;

    private float RotationSpeed = 1;
    private Quaternion lookRotation;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        uIManager = UIManager.FindFirstObjectByType<UIManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnDestroy()
    {
        uIManager.refreshTargetText();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(player.transform);

        RaycastHit playerHit;
        direction = (player.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out playerHit, 10) && playerHit.collider.CompareTag("Player"))
        {
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
            if (Mathf.Abs(transform.rotation.y - lookRotation.y) < .5) 
            {
                //move toward player
                //transform.position = new Vector3(transform.position.x + direction.x, transform.position.y, transform.position.z + direction.z) * Time.deltaTime * .01f;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y + .001f, player.transform.position.z), Time.deltaTime);


                if (Mathf.Abs(transform.rotation.y - lookRotation.y) < .02)
                {
                    Vector3 forward = lookRotation * Vector3.forward;
                    GameObject pj = Instantiate(projectile, transform.position + (forward) * 0.6f, lookRotation);
                    pj.GetComponent<Rigidbody>().velocity = (forward) * bulletSpeed;
                }
            }
        }
        
    }
}
