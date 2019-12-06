using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Vector2 moveVec;
    public Rigidbody rb;
    public float maxHealth;
    public float health;
    private Transform player_t;
    public SpriteRenderer spriteRend;
    public Animator animator;
    public AudioSource damagedSFX;

    [Header("Sprite Flashing")]
    public float flashFrequency;
    public int flashNumber;

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

        if (maxHealth == 0)
        {
            maxHealth = 5f;
        }
        health = maxHealth;

        isBeingKnockedBack = false;
        canMove = true;
        knockbackTimeCounter = 0;
        spriteRend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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

        //animation
        animator.SetFloat("Horizontal", moveVec.x);
        animator.SetFloat("Vertical", moveVec.y);
        animator.SetFloat("Speed", moveVec.sqrMagnitude);

        //death and game over condition
        if (health <= 0)
        {
            DieRespawn();
            //Debug.Log("Game Over");
            //GameManager.instance.ReloadScene();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title Screen");
        }

        //knockback duration, reduce velocity, velocity becomes 0 by the time knockback is over (determined by knockback time)
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
            //calculate vector for knockback direction (away from enemy)
            Transform enemy_t = collision.gameObject.GetComponent<Transform>();
            Vector3 KBdirVec = new Vector3(player_t.position.x - enemy_t.position.x, player_t.position.y - enemy_t.position.y, 0f);
            Knockback(KBdirVec);
        }
    }

    public void Knockback(Vector3 direction)
    {
        isBeingKnockedBack = true;
        canMove = false;

        direction.Normalize();
        knockbackForceVec = direction;

        //apply knockback force
        rb.velocity = knockbackForce * direction;
    }

    public void DamagePlayer(float damage)
    {
        health -= damage;

        //play sfx
        AudioManager.instance.PlaySingle(damagedSFX);

        //flash sprite to show feedback
        StartCoroutine(FlashSprite(flashFrequency, flashNumber));
    }

    IEnumerator FlashSprite(float frequency, int number)
    {
        for (int i = 0; i < number; i++)
        {
            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 0f);
            yield return new WaitForSeconds(1 / frequency);

            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 1f);
            yield return new WaitForSeconds(1 / frequency);
        }
    }

    private void DieRespawn()
    {
        //things to do when player dies
        GameManager.instance.activeCheckpoint.door.RespawnEnemies();
        StartCoroutine(FindObjectOfType<FadeWhenChangingFloors>().FadeAndMovePlayerTransform(new Vector3(GameManager.instance.activeCheckpoint.transform.position.x, GameManager.instance.activeCheckpoint.transform.position.y, player_t.position.z), true));
        //player_t.position = new Vector3(GameManager.instance.activeCheckpoint.transform.position.x, GameManager.instance.activeCheckpoint.transform.position.y, player_t.position.z);
        health = maxHealth;
        GetComponent<ShootBullet>().flameObject.SetActive(false);
    }
}
