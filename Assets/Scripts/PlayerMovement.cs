using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Vector2 moveVec;
    public Rigidbody rb;
    public float health = 5f;
    private Transform player_t;
    public SpriteRenderer spriteRend;

    [Header("Knockback")]
    public bool canMove;
    public bool isBeingKnockedBack;
    public float knockbackForce;
    public Vector3 knockbackForceVec;
    public float knockbackTime;
    private float knockbackTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player_t = GetComponent<Transform>();
        health = 5f;
        isBeingKnockedBack = false;
        canMove = true;
        knockbackTimeCounter = 0;
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //movement
        if (canMove)
        {
            moveVec = new Vector3(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed);
            rb.velocity = moveVec;
        }

        //death and game over condition
        if (health<=0)
        {
            //Debug.Log("Game Over");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("esc");
            GameManager.instance.ReloadScene();
        }

        //flip sprite based on direction mouse is pointing
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                spriteRend.flipX = true;
            }
            else if (Input.GetAxisRaw("Horizontal") == -1)
            {
                spriteRend.flipX = false;
            }
        }

        //knockback duration
        if (isBeingKnockedBack && rb.velocity.magnitude >= 0 && !canMove)
        {
            knockbackTimeCounter += Time.deltaTime;
            rb.velocity -= Time.deltaTime / knockbackTime * knockbackForce * knockbackForceVec;

            if (knockbackTimeCounter >= knockbackTime || rb.velocity.magnitude <= 0)
            {
                isBeingKnockedBack = false;
            }
        }
        else
        {
            canMove = true;
            isBeingKnockedBack = false;
            knockbackTimeCounter = 0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("player knocked back");
            isBeingKnockedBack = true;
            canMove = false;

            //calculate vector for knockback direction (away from enemy)
            Transform enemy_t = collision.gameObject.GetComponent<Transform>();
            Vector3 KBdirVec = new Vector3(player_t.position.x - enemy_t.position.x, player_t.position.y - enemy_t.position.y, 0f);
            KBdirVec.Normalize();
            knockbackForceVec = KBdirVec;

            //apply knockback force
            rb.velocity = knockbackForce * KBdirVec;
        }
    }
}
