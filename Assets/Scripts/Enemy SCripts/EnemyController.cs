﻿using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public EnemyType thisEnemyType;

    public float health;
    public float speed;

    public GameObject player;
    public Transform player_transform;
    public Rigidbody rb;
    public Animator animator;
    public SpriteRenderer spriteRend;

    public Vector2 playerToEnemyVector;
    public float enemyDistance; //distance at which shooting enemy tries to stay away from player;
    public float enemyMoveDistance; // distance that shooting enemy moves each time it tries to avoid the player;
    public float DistToPlayer;//distance between player and enemy
    public float PlayerDetectionRange;//distance at which enemy will detect player
    [SerializeField]
    private bool isShooting;
    public bool dropsWeaponOnDeath;
    public GameObject weaponDropPrefab;

    //pathfinding
    [Header("Pathfinding")]
    public IAstarAI ai;
    public AIDestinationSetter AIdest;
    public AIPath aiPath;

    [Header("Hot Sauce")]
    public GameObject hotSauceBulletPrefab;
    public float hotSauceAlternateTime; // time for hot sauce enemy to alternate between moving towards player and stopping to shoot
    private float hotSauceAlternateTimeCounter;
    public float hotSauceShootTime; // time for hot sauce to fire all bullets
    private float hotSauceShootTimeCounter;
    
    [Header("Pizza")]
    public GameObject pizzaBulletPrefab;
    public float pizzaAlternateTime; // time for hot sauce enemy to alternate between moving towards player and stopping to shoot
    private float pizzaAlternateTimeCounter;
    public float pizzaShootTime; // time for hot sauce to fire all bullets
    private float pizzaShootTimeCounter;
    public float pizzaBulletOffset;

    /*
    public bool isBeingKnockedBack;
    public GameObject spamExplosionPrefab;
    public float knockbackForce;
    public float knockbackSlowdownForce;
    */

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

        player = GameObject.FindWithTag("Player");
        player_transform = player.GetComponent<Transform>();
        //isBeingKnockedBack = false;
        rb = GetComponent<Rigidbody>();

        AstarPath.FindAstarPath();
        AstarPath.active.Scan();
        AIdest = gameObject.GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();

        ai = GetComponent<IAstarAI>();

        aiPath.maxSpeed = speed;

        //initialize values for different enemy types
        switch (thisEnemyType)
        {
            case EnemyType.Bacon:
                if (speed == 0)
                {
                    aiPath.maxSpeed = 1f;
                }
                break;
            case EnemyType.SpamCan:
                break;
            case EnemyType.HotSauce:
                if (speed == 0)
                {
                    aiPath.maxSpeed = 0.7f;
                }
                hotSauceShootTimeCounter = 0;
                break;
            case EnemyType.Cabbage:
                break;
            case EnemyType.Carrot:
                break;
            case EnemyType.Corn:
                break;
            case EnemyType.Meatball:
                if (speed == 0)
                {
                    aiPath.maxSpeed = 0.3f;
                }
                break;
            case EnemyType.Chobani:
                break;
            case EnemyType.PizzaBox:
                if (speed == 0)
                {
                    aiPath.maxSpeed = 0.7f;
                }
                pizzaShootTimeCounter = 0;
                break;
            case EnemyType.ChurroTaco:
                break;
            case EnemyType.GingerbreadMan:
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerToEnemyVector = new Vector3(transform.position.x - player_transform.position.x, transform.position.y - player_transform.position.y, 0f);
        DistToPlayer = playerToEnemyVector.magnitude;

        //pathfinding
        if (AstarPath.active.data.gridGraph.nodes == null) AstarPath.active.Scan();

        if (health <= 0)
        {
            Die();
        }

        /*
        //pathfinding disabled while enemy is being knocked back (from explosion)
        if (isBeingKnockedBack)
        {
            ai.isStopped = true;
            ai.canMove = true;

            return;
        }
        else
        {
            ai.isStopped = false;
            ai.canMove = true;
        }
        */

        //run behaviour or specific enemy types
        switch (thisEnemyType)
        {
            case EnemyType.Bacon:
                SetPlayerAsAITarget();
                PointBaconAtPlayer();
                break;
            case EnemyType.SpamCan:
                break;
            case EnemyType.HotSauce:
                HotSauce();
                FlipSprite();
                break;
            case EnemyType.Cabbage:
                break;
            case EnemyType.Carrot:
                break;
            case EnemyType.Corn:
                break;
            case EnemyType.Meatball:
                SetPlayerAsAITarget();
                break;
            case EnemyType.Chobani:
                break;
            case EnemyType.PizzaBox:
                Pizza();
                FlipSprite();
                break;
            case EnemyType.ChurroTaco:
                break;
            case EnemyType.GingerbreadMan:
                break;
            default:
                break;
        }
    }

    private void PointBaconAtPlayer()
    {
        transform.right = -playerToEnemyVector;
    }

    private void FlipSprite()
    {
        //flip sprite when player is to the right
        if (playerToEnemyVector.x <= 0)
        {
            spriteRend.flipX = true;
        }
        else
        {
            spriteRend.flipX = false;
        }
    }

    public void SetPlayerAsAITarget()
    {
        //set target
        AIdest.target = player_transform;
    }

    /// <summary>
    /// Hot Sauce Behaviour
    /// moves for hotSauceAlternateTime seconds
    /// next, stops and enters shooting mode for another hotSauceAlternateTime seconds
    /// while in shooting mode, shoots a bullet every hotSauceShootTime seconds
    /// </summary>
    public void HotSauce()
    {
        SetPlayerAsAITarget();

        //set isShooting bool for animation
        animator.SetBool("isShooting", isShooting);

        if (!isShooting && hotSauceAlternateTimeCounter >= hotSauceAlternateTime)
        {
            isShooting = true;
            aiPath.enabled = false;
            hotSauceAlternateTimeCounter = 0;
            hotSauceShootTimeCounter = 0;
        }
        else if(!isShooting && hotSauceAlternateTimeCounter <= hotSauceAlternateTime)
        {
            //moving mode
            hotSauceAlternateTimeCounter += Time.deltaTime;
        }

        if (isShooting && hotSauceAlternateTimeCounter <= hotSauceAlternateTime)
        {
            //shooting mode
            hotSauceAlternateTimeCounter += Time.deltaTime;
            hotSauceShootTimeCounter += Time.deltaTime;

            if (hotSauceShootTimeCounter >= hotSauceShootTime)
            {
                //shoot
                Instantiate(hotSauceBulletPrefab, transform.position, Quaternion.identity);
                hotSauceShootTimeCounter = 0;
            }

        }
        else if(isShooting && hotSauceAlternateTimeCounter >= hotSauceAlternateTime)
        {
            SetPlayerAsAITarget();
            isShooting = false;
            aiPath.enabled = true;
            hotSauceAlternateTimeCounter = 0;
            hotSauceShootTimeCounter = 0;
        }
    }

    /// <summary>
    /// fires 8 bullets outwards, evenly spread around the pizza
    /// </summary>
    public void Pizza()
    {
        SetPlayerAsAITarget();

        //set isShooting bool for animation
        //animator.SetBool("isShooting", isShooting);

        if (!isShooting && pizzaAlternateTimeCounter >= pizzaAlternateTime)
        {
            isShooting = true;
            aiPath.enabled = false;
            pizzaAlternateTimeCounter = 0;
            pizzaShootTimeCounter = 0;
        }
        else if (!isShooting && pizzaAlternateTimeCounter <= pizzaAlternateTime)
        {
            //moving mode
            pizzaAlternateTimeCounter += Time.deltaTime;
        }

        if (isShooting && pizzaAlternateTimeCounter <= pizzaAlternateTime)
        {
            //shooting mode
            pizzaAlternateTimeCounter += Time.deltaTime;
            pizzaShootTimeCounter += Time.deltaTime;

            if (pizzaShootTimeCounter >= pizzaShootTime)
            {
                //TODO shooting PIZZA
                CreateBulletAtAngle(pizzaBulletPrefab, Vector3.right, pizzaBulletOffset);
                CreateBulletAtAngle(pizzaBulletPrefab, Vector3.right + Vector3.up, pizzaBulletOffset);
                CreateBulletAtAngle(pizzaBulletPrefab, Vector3.up, pizzaBulletOffset);
                CreateBulletAtAngle(pizzaBulletPrefab, Vector3.left + Vector3.up, pizzaBulletOffset);
                CreateBulletAtAngle(pizzaBulletPrefab, Vector3.left, pizzaBulletOffset);
                CreateBulletAtAngle(pizzaBulletPrefab, Vector3.left + Vector3.down, pizzaBulletOffset);
                CreateBulletAtAngle(pizzaBulletPrefab, Vector3.down, pizzaBulletOffset);
                CreateBulletAtAngle(pizzaBulletPrefab, Vector3.right + Vector3.down, pizzaBulletOffset);

                pizzaShootTimeCounter = 0;
            }
        }
        else if (isShooting && pizzaAlternateTimeCounter >= pizzaAlternateTime)
        {
            SetPlayerAsAITarget();
            isShooting = false;
            aiPath.enabled = true;
            pizzaAlternateTimeCounter = 0;
            pizzaShootTimeCounter = 0;
        }
    }

    /// <summary>
    /// function is customised for pizza bullets
    /// </summary>
    /// <param name="bulletPrefab"></param>
    /// <param name="dirVec"></param>
    /// <param name="bulletOffset"></param>
    /// <param name="angleOffset"></param>
    private void CreateBulletAtAngle(GameObject bulletPrefab, Vector3 dirVec, float bulletOffset, float angleOffset = 0f)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position + dirVec * bulletOffset;

        bullet.transform.up = dirVec; // orientate bullet
        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        rigidbody.AddForce(Quaternion.AngleAxis(angleOffset, Vector3.forward) * dirVec * 100.0f, ForceMode.Acceleration);
        
    }

    /*
    /// <summary>
    /// suicide bomber
    /// </summary>
    public void SpamCan()
    {
        //set target
        AIdest.target = player_transform;

        //disable movement once spam reaches player, then trigger explosion, destroy spam enemy game object
        if (DistToPlayer <= 1)
        {
            //stop movement
            ai.canMove = false;

            //explode
            Instantiate(spamExplosionPrefab, transform.position, Quaternion.identity);
        }
    }
    */


    /// <summary>
    /// Enemy dies
    /// </summary>
    public void Die()
    {
        //drops weapon
        if (dropsWeaponOnDeath)
        {
            GameObject pickup = Instantiate(weaponDropPrefab, transform.position, Quaternion.identity);

            switch (thisEnemyType)
            {
                case EnemyType.Bacon:
                    pickup.GetComponent<WeaponPickup>().weaponType = GameManager.WeaponType.BaconCrossbow;
                    break;
                case EnemyType.SpamCan:
                    break;
                case EnemyType.HotSauce:
                    pickup.GetComponent<WeaponPickup>().weaponType = GameManager.WeaponType.HotSauceSquirtGun;
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
                    pickup.GetComponent<WeaponPickup>().weaponType = GameManager.WeaponType.CookieShuriken;
                    break;
                case EnemyType.ChurroTaco:
                    break;
                case EnemyType.GingerbreadMan:
                    break;
                default:
                    break;
            }
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        { //TODO:
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
                    DamagePlayer(3f);
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
        //Debug.Log("Damaged Player");
    }
}
