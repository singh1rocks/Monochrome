using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health;
    public float speed;

    public GameObject player;
    public Transform player_transform;

    public Vector2 playerToEnemyVector;
    public GameObject EnemyBulletPrefab;
    public float enemyDistance; //distance at which shooting enemy tries to stay away from player;
    public float enemyMoveDistance; // distance that shooting enemy moves each time it tries to avoid the player;
    public float DistToPlayer;//distance between player and enemy
    public float PlayerDetectionRange;//distance at which enemy will detect player

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
        playerToEnemyVector = new Vector2(transform.position.x - player_transform.position.x, transform.position.y - player_transform.position.y);
        DistToPlayer = playerToEnemyVector.magnitude;
        if (thisEnemyType == EnemyType.FollowPlayer && DistToPlayer <= PlayerDetectionRange)
        {
            //behavior of enemy that just follows player
            transform.position = Vector2.MoveTowards(transform.position, player_transform.position, speed * Time.deltaTime);
        }
        else if (thisEnemyType == EnemyType.ShootAtPlayer)
        {
            //TODO: behavior of enemy that shoots at player
            if (DistToPlayer <= enemyDistance)
            {
                //move away from player
                StartCoroutine("ShootingEnemyBehaviourCoroutine");
            }
            else
            {
                //move random direction

            }

        }

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("ENemy died");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DamagePlayer();
        }
        else if (collision.gameObject.tag == "Bullet")
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
            Destroy(collision.gameObject);
            Debug.Log("Damaged Enemy");
        }
    }

    public void DamagePlayer()
    {
        player.GetComponent<PlayerMovement>().health--;
        Debug.Log("Damaged Player");
    }

    IEnumerator ShootingEnemyBehaviourCoroutine()
    {
        yield return StartCoroutine("MoveAway");

        //stop and fire 2 bullets
        Instantiate(EnemyBulletPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        Instantiate(EnemyBulletPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
    }

    IEnumerator MoveAway()
    {
        float tempSpeed = speed;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + playerToEnemyVector, tempSpeed * Time.deltaTime);
        yield return new WaitForSeconds(2f);

        tempSpeed = 0f;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + playerToEnemyVector, tempSpeed * Time.deltaTime);
        yield return null;
    }
}
