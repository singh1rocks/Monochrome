using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Vector2 target;
    public Transform transform;
    public Rigidbody rb;
    public float health = 5f;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        health = 5f;
        //init bullet values
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //target = new Vector2(transform.position.x + Input.GetAxisRaw("Horizontal") * speed, transform.position.y + Input.GetAxisRaw("Vertical") * speed);
        //transform.position = Vector2.MoveTowards(transform.position, target, Mathf.Infinity);

        //movement
        target = new Vector3(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed);
        rb.velocity = target;

        //death condition
        if (health<=0)
        {
            //Time.timeScale = 0f;
            Debug.Log("Game Over");

            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("esc");
            GameManager.instance.ReloadScene();
        }

        //flip sprite based on direction mouse is pointing
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
