using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public EnemyType thisEnemyType;

    public float health;
    public float speed;

    public GameObject player;
    public Transform player_transform;
    public Rigidbody rb;

    public Vector2 playerToEnemyVector;
    public float enemyDistance; //distance at which shooting enemy tries to stay away from player;
    public float enemyMoveDistance; // distance that shooting enemy moves each time it tries to avoid the player;
    public float DistToPlayer;//distance between player and enemy
    public float PlayerDetectionRange;//distance at which enemy will detect player

    //pathfinding
    public IAstarAI ai;


    public enum EnemyType
    {
        Bacon,
        SpamCan,
        HotSauce,
        Cabbage,
        Carrot,
        Corn,
        Meatball,
        Chobani,
        PizzaBox,
        ChurroTaco,
        GingerbreadMan
    }

    // Start is called before the first frame update
    void Start()
    {
        if (health == 0)
        {
            health = 3f;
        }

        if (speed == 0)
        {
            speed = 1;
        }
        
        player = GameObject.FindWithTag("Player");
        player_transform = player.GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        AstarPath.FindAstarPath();
        AstarPath.active.Scan();

        ai = GetComponent<IAstarAI>();
    }

    // Update is called once per frame
    void Update()
    {
        playerToEnemyVector = new Vector2(transform.position.x - player_transform.position.x, transform.position.y - player_transform.position.y);
        DistToPlayer = playerToEnemyVector.magnitude;

        //pathfinding
        if (AstarPath.active.data.gridGraph.nodes == null) AstarPath.active.Scan();

        if (health <= 0)
        {
            Die();
        }

        switch (thisEnemyType)
        {
            case EnemyType.Bacon:
                Bacon();
                break;
            case EnemyType.SpamCan:
                break;
            case EnemyType.HotSauce:
                break;
            case EnemyType.Cabbage:
                break;
            case EnemyType.Carrot:
                break;
            case EnemyType.Corn:
                break;
            case EnemyType.Meatball:
                break;
            case EnemyType.Chobani:
                break;
            case EnemyType.PizzaBox:
                break;
            case EnemyType.ChurroTaco:
                break;
            case EnemyType.GingerbreadMan:
                break;
            default:
                break;
        }
    }

    public void Bacon()
    {
        //movement
        if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
        {
            ai.canMove = false;
            ai.SearchPath();
        }
    }

    /// <summary>
    /// suicide bomber
    /// </summary>
    public void SpamCan()
    {
        transform.position = Vector2.MoveTowards(transform.position, player_transform.position, speed * Time.deltaTime);

        //enable pathfinding and movement

        
    }


    /// <summary>
    /// Enemy dies
    /// </summary>
    public void Die()
    {
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Damaged by player bullet
        if (collision.gameObject.tag == "Bullet")
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
            Destroy(collision.gameObject);
            //Debug.Log("Damaged Enemy");
        }
        else if (collision.gameObject.tag == "Player")
        {
            switch (thisEnemyType)
            {
                case EnemyType.Bacon:
                    DamagePlayer(1f);
                    break;
                case EnemyType.SpamCan:
                    break;
                case EnemyType.HotSauce:
                    break;
                case EnemyType.Cabbage:
                    break;
                case EnemyType.Carrot:
                    break;
                case EnemyType.Corn:
                    break;
                case EnemyType.Meatball:
                    break;
                case EnemyType.Chobani:
                    break;
                case EnemyType.PizzaBox:
                    break;
                case EnemyType.ChurroTaco:
                    break;
                case EnemyType.GingerbreadMan:
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Damages player by d health points
    /// </summary>
    /// <param name="d"></param>
    public void DamagePlayer(float d)
    {
        player.GetComponent<PlayerMovement>().health -= d;
        Debug.Log("Damaged Player");
    }
}
