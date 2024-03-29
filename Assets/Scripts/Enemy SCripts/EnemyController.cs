﻿using UnityEngine;
using Pathfinding;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public EnemyType thisEnemyType;

    public float maxHealth;
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
    public bool FSCoroutineRunning;
    public Coroutine FSCoroutine;

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
    public AudioSource hotSauceBulletSFX;
    
    [Header("Pizza")]
    public GameObject pizzaBulletPrefab;
    public float pizzaAlternateTime; // time for hot sauce enemy to alternate between moving towards player and stopping to shoot
    private float pizzaAlternateTimeCounter;
    public float pizzaShootTime; // time for hot sauce to fire all bullets
    private float pizzaShootTimeCounter;
    public float pizzaBulletOffset;
    public AudioSource pizzaBulletSFX;

    [Header("Knockback")]
    public bool canMove;
    public bool isBeingKnockedBack;
    public float knockbackForce;
    public Vector3 knockbackForceVec;
    public float knockbackTime;
    private float knockbackTimeCounter;

    [Header("Spam Can")]
    public GameObject explosionPrefab;
    public float explosionDelay;
    private float explosionDelayCounter;

    [Header("Corn")]
    public GameObject cornBulletPrefab;
    public float cornShootTime;
    private float cornShootTimeCounter;

    [Header("Bacon")]
    public float baconDamage;

    [Header("Meatball")]
    public float meatballDamage;

    [Header("Carrot")]
    public float carrotDamage;

    public enum EnemyType
    {
        Bacon,
        SpamCan,
        HotSauce,
        Carrot,
        Corn,
        Meatball,
        ChobaniYogurt,
        PizzaBox,
        ChurroTaco,
    }

    // Start is called before the first frame update
    void Start()
    {
        if (maxHealth == 0)
        {
            maxHealth = 3f;
        }
        health = maxHealth;

        player = GameObject.FindWithTag("Player");
        player_transform = player.GetComponent<Transform>();
        //isBeingKnockedBack = false;
        rb = GetComponent<Rigidbody>();
        isBeingKnockedBack = false;
        FSCoroutineRunning = false;

        AstarPath.FindAstarPath();
        AstarPath.active.Scan();
        AIdest = gameObject.GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        aiPath.canMove = true;

        ai = GetComponent<IAstarAI>();




        //initialize values for different enemy types
        switch (thisEnemyType)
        {
            case EnemyType.Bacon:
                if (speed == 0)
                {
                    speed = 1f;
                }
                break;
            case EnemyType.SpamCan:
                if (speed == 0)
                {
                    speed = 1.5f;
                }
                break;
            case EnemyType.HotSauce:
                if (speed == 0)
                {
                    speed = 0.7f;
                }
                hotSauceShootTimeCounter = 0;
                break;
            case EnemyType.Carrot:
                if (speed == 0)
                {
                    speed = 1.3f;
                }
                break;
            case EnemyType.Corn:
                speed = 0f;
                break;
            case EnemyType.Meatball:
                if (speed == 0)
                {
                    speed = 0.3f;
                }
                break;
            case EnemyType.PizzaBox:
                if (speed == 0)
                {
                    speed = 0.7f;
                }
                pizzaShootTimeCounter = 0;
                break;
            case EnemyType.ChurroTaco:
                break;
            default:
                break;
        }

        aiPath.maxSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        playerToEnemyVector = new Vector3(transform.position.x - player_transform.position.x, transform.position.y - player_transform.position.y, 0f);
        DistToPlayer = playerToEnemyVector.magnitude;

        //pathfinding
        if (AstarPath.active.data.gridGraph.nodes == null) AstarPath.active.Scan();
        AstarPath.FindAstarPath();

        if (health <= 0)
        {
            Die();
        }

        //knockback duration, reduce velocity, velocity becomes 0 by the time knockback is over (determined by knockback time)
        if (isBeingKnockedBack && rb.velocity.magnitude >= 0 && !canMove)
        {
            knockbackTimeCounter += Time.deltaTime;
            rb.velocity -= Time.deltaTime / knockbackTime * knockbackForce * knockbackForceVec;

            if (knockbackTimeCounter >= knockbackTime || rb.velocity.magnitude <= 0)
            {
                isBeingKnockedBack = false;
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            aiPath.enabled = true;
            isBeingKnockedBack = false;
            knockbackTimeCounter = 0f;
        }

        //run behaviour or specific enemy types
        switch (thisEnemyType)
        {
            case EnemyType.Bacon:
                SetPlayerAsAITarget();
                PointBaconAtPlayer();
                break;
            case EnemyType.SpamCan:
                SetPlayerAsAITarget();
                break;
            case EnemyType.HotSauce:
                HotSauce();
                FlipSprite();
                break;
            case EnemyType.Carrot:
                SetPlayerAsAITarget();
                break;
            case EnemyType.Corn:
                Corn();
                break;
            case EnemyType.Meatball:
                SetPlayerAsAITarget();
                break;
            case EnemyType.PizzaBox:
                Pizza();
                FlipSprite();
                break;
            case EnemyType.ChurroTaco:
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
            aiPath.canMove = false;
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

                //play sfx
                AudioManager.instance.PlaySingle(hotSauceBulletSFX);
            }

        }
        else if(isShooting && hotSauceAlternateTimeCounter >= hotSauceAlternateTime)
        {
            SetPlayerAsAITarget();
            isShooting = false;
            aiPath.canMove = true;
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
            aiPath.canMove = false;
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

                //play sfx
                AudioManager.instance.PlaySingle(pizzaBulletSFX);
            }
        }
        else if (isShooting && pizzaAlternateTimeCounter >= pizzaAlternateTime)
        {
            SetPlayerAsAITarget();
            isShooting = false;
            aiPath.canMove = true;
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
        rigidbody.AddForce(Quaternion.AngleAxis(angleOffset, Vector3.forward) * dirVec * 200.0f, ForceMode.Acceleration);
        
    }

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
                    pickup.GetComponent<WeaponPickup>().weaponType = GameManager.WeaponType.PopcornGrenade;
                    break;
                case EnemyType.HotSauce:
                    pickup.GetComponent<WeaponPickup>().weaponType = GameManager.WeaponType.HotSauceSquirtGun;
                    break;
                case EnemyType.Carrot:
                    break;
                case EnemyType.Corn:
                    break;
                case EnemyType.Meatball:
                    pickup.GetComponent<WeaponPickup>().weaponType = GameManager.WeaponType.StrawberryDrill;
                    break;
                case EnemyType.PizzaBox:
                    pickup.GetComponent<WeaponPickup>().weaponType = GameManager.WeaponType.CookieShuriken;
                    break;
                case EnemyType.ChurroTaco:
                    break;
                default:
                    break;
            }
            Debug.Log("enemy die");

            /*
            if (FSCoroutine != null && FSCoroutineRunning)
            {
                StopCoroutine(FSCoroutine);
                FSCoroutineRunning = false;
            }
            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 1f);
            */
            gameObject.SetActive(false);
            return;
        }
        else
        {
            /*
            if (FSCoroutine != null && FSCoroutineRunning)
            {
                StopCoroutine(FSCoroutine);
                FSCoroutineRunning = false;
            }
            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 1f);
            */
            gameObject.SetActive(false);
            return;
        }

    }

    //contact damage
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        { //TODO:
            switch (thisEnemyType)
            {
                case EnemyType.Bacon:
                    DamagePlayer(baconDamage);
                    break;
                case EnemyType.SpamCan:
                    Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    break;
                case EnemyType.HotSauce:
                    break;
                case EnemyType.Carrot:
                    DamagePlayer(carrotDamage);
                    break;
                case EnemyType.Corn:
                    break;
                case EnemyType.Meatball:
                    DamagePlayer(meatballDamage);
                    break;
                case EnemyType.PizzaBox:
                    break;
                case EnemyType.ChurroTaco:
                    break;
                default:
                    break;
            }
        }

        if (collision.gameObject.tag == "Reflected Bullet")
        {
            if (collision.gameObject.GetComponent<EnemyBullet>())
            {
                Damaged(collision.gameObject.GetComponent<EnemyBullet>().damage);
            }
            else if (collision.gameObject.GetComponent<SpiralBullet>())
            {
                Damaged(collision.gameObject.GetComponent<SpiralBullet>().damage);
            }
            else if (collision.gameObject.GetComponent<BossShotgunBullet>())
            {
                Damaged(collision.gameObject.GetComponent<BossShotgunBullet>().damage);
            }
        }
    }
    
    /// <summary>
    /// Damages player by d health points
    /// </summary>
    /// <param name="d"></param>
    public void DamagePlayer(float d)
    {
        player.GetComponent<PlayerMovement>().DamagePlayer(d);
    }

    /// <summary>
    /// enemy damaged by d health points
    /// </summary>
    /// <param name="d"></param>
    public void Damaged(float d)
    {
        health -= d;
        FSCoroutine = StartCoroutine(FlashSprite(10f, 4));
    }

    public IEnumerator FlashSprite(float frequency, int number)
    {
        FSCoroutineRunning = true;
        for (int i = 0; i < number; i++)
        {
            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 0f);
            yield return new WaitForSeconds(1 / frequency);

            spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 1f);
            yield return new WaitForSeconds(1 / frequency);
        }
        FSCoroutineRunning = false;
    }

    public void Knockback(Vector3 direction)
    {
        isBeingKnockedBack = true;
        aiPath.enabled = false;

        direction.Normalize();
        knockbackForceVec = direction;

        //apply knockback force
        rb.velocity = knockbackForce * direction;
    }

    public void Corn()
    {
        cornShootTimeCounter += Time.deltaTime;

        if (cornShootTimeCounter >= cornShootTime)
        {
            //shoot
            Instantiate(cornBulletPrefab, transform.position, Quaternion.identity);
            cornShootTimeCounter = 0;

            //play sfx
            //AudioManager.instance.PlaySingle(hotSauceBulletSFX);
        }
    }

}
