using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMovement : MonoBehaviour
{
    private CharacterController controller;
    private UIManager uIManager;
    private Camera _camera;
    private Vector3 movement;
    private bool running = false;
    // private Vector3 velocity;
    private float verticalLook = 0;
    public int speed = 500;
    public GameObject projectile;
    public float bulletSpeed = 30;

    public float health = 1000;
    //Jumping stuff
    /*public float gravity = -9.81f;
    public float jumpHeight = 3f;*/


    // Start is called before the first frame update
    void Start()
    {
        uIManager = UIManager.FindFirstObjectByType<UIManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
    }
    public void shot(float damage, float knockback, GameObject projectile) 
    { 
        controller.Move(new Vector3(projectile.GetComponent<Rigidbody>().velocity.x * knockback, 1f, projectile.GetComponent<Rigidbody>().velocity.z * knockback) * Time.deltaTime);
        health -= damage;

        uIManager.refreshPlayerHealthText(health);

        if (health <= 0) 
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
    private void OnMove(InputValue value)
    {
        Vector2 m = value.Get<Vector2>();
        movement = new Vector3(m.x, 0, m.y);
    }
    private void OnLook(InputValue value)
    {
        Vector2 m = value.Get<Vector2>();
        if (m.magnitude < 20)
        {
            transform.Rotate(0, m.x * .5f, 0);
            verticalLook = Mathf.Clamp(verticalLook + m.y * .5f, -90, 90);
            _camera.transform.localEulerAngles = new Vector3(-verticalLook, 0, 0);
        }
    }

    private void OnRun(InputValue value)
    {
        if (value.isPressed)
        {
            running = true;
        }
        else
        {
            running = false;
        }
    }

    private void OnJump(InputValue value)
    {

        /*velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity);*/
        //jump 
    }
    private void OnFire(InputValue value)
    {
        Vector3 forward = _camera.transform.rotation * Vector3.forward;
        //Physics.OverlapSphere(_camera.transform.position + (forward) * 0.6f, 0.1f); //change this later 
        GameObject pj = Instantiate(projectile, _camera.transform.position + (forward) * 0.6f, _camera.transform.rotation);
        pj.GetComponent<Rigidbody>().velocity = (forward) * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        controller.SimpleMove(transform.rotation * movement * speed * (running?2:1) * Time.deltaTime);
    }
}
