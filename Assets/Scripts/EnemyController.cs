using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health;
    public float speed;

    public GameObject player;
    public Transform player_transform;

    public enum EnemyType
    {
        FollowPlayer,
        ShootAtPlayer
    }

    public EnemyType thisEnemyType;

    // Start is called before the first frame update
    void Start()
    {
        health = 3f;

        player = GameObject.FindWithTag("Player");
        player_transform = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (thisEnemyType == EnemyType.FollowPlayer)
        {
            //behavior of enemy that just follows player
            transform.position = Vector2.MoveTowards(transform.position, player_transform.position, speed * Time.deltaTime);
        }
        else if (thisEnemyType == EnemyType.ShootAtPlayer)
        {
            //TODO: behavior of enemy that shoots at player

        }
        

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DamagePlayer();
        }
        else if (collision.tag == "Bullet")
        {
            health -= collision.GetComponent<Bullet>().damage;
            Destroy(collision.gameObject);
            Debug.Log("Damaged Enemy");
        }
    }

    public void DamagePlayer()
    {
        player.GetComponent<PlayerMovement>().health--;
        Debug.Log("Damaged Player");
    }

}
