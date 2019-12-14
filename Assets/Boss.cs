using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Boss : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float speed;

    private GameObject player;
    private Transform player_transform;
    private Rigidbody rb;
    private SpriteRenderer spriteRend;
    private Vector3 playerToEnemyVector;
    private Animator animator;


    [Space]
    [Header("Pathfinding")]
    public IAstarAI ai;
    public AIDestinationSetter AIdest;
    public AIPath aiPath;

    [Header("SFX")]
    public AudioSource bossDying;
    public AudioSource bossAttackOne;
    public AudioSource bossAttackTwo;

    [Header("Bools")]
    public bool FSCoroutineRunning;
    public bool inAttackAnimation;

    [Header("Attack Change")]
    [SerializeField] private float alternateAttackTime;
    private float alternateAttackTimeCounter;
    private int attackType = 1;

    [Header("Spiral Attack Values")]
    public float spiralShootTime;
    private float spiralShootTimeCounter;
    public float spiralAngleOffset;
    private float thisBulletAngleOffset;
    public float spiralBulletSpeed;
    public GameObject spiralBulletPrefab;

    [Header("Shotgun Attack Values")]
    public float shotgunBurstShootTime;
    private float shotgunBurstShootTimeCounter;
    public int numOfShotgunBullets;
    public float shotgunAngleOffsetRange;
    public float shotgunBulletSpeed;
    public GameObject shotgunBurstBulletPrefab;

    [Header("Meteor Attack Values")]
    public GameObject meteorPrefab;
    public float meteorShootTime;
    private float meteorShootTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        if (maxHealth == 0)
        {
            maxHealth = 3f;
        }
        health = maxHealth;

        if (spiralBulletSpeed == 0)
        {
            spiralBulletSpeed = 30f;
        }

        player = GameObject.FindWithTag("Player");
        player_transform = player.GetComponent<Transform>();
        //isBeingKnockedBack = false;
        rb = GetComponent<Rigidbody>();
        FSCoroutineRunning = false;
        shotgunAngleOffsetRange = shotgunAngleOffsetRange / 2;
        spriteRend = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
        AstarPath.FindAstarPath();
        AstarPath.active.Scan();
        AIdest = gameObject.GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        aiPath.canMove = true;
        inAttackAnimation = false;

        ai = GetComponent<IAstarAI>();

        ai.maxSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        spriteRend.color = new Color(spriteRend.color.r, health/maxHealth, health / maxHealth, 1f);

        //death condition
        if (health <= 0f)
        {
            Die();
        }

        //animation
        animator.SetBool("inAttackAnimation", inAttackAnimation);

        playerToEnemyVector = new Vector3(transform.position.x - player_transform.position.x, transform.position.y - player_transform.position.y, 0f);
        playerToEnemyVector.Normalize();

        FlipSprite();
        SetPlayerAsAITarget();

        if (health >= maxHealth * 2 / 3)
        {
            //phase 1
            //shotgun burst
            //spiral

            switch (attackType % 2)
            {
                case 1:
                    ShotgunBehavior();
                    break;
                case 0:
                    MeteorBehavior();
                    break;
            }

            if (alternateAttackTimeCounter >= alternateAttackTime)
            {
                attackType += 1;
                alternateAttackTimeCounter = 0f;
            }

            alternateAttackTimeCounter += Time.deltaTime;

        }
        else if (health >= maxHealth * 1 / 3)
        {

            switch (attackType % 2)
            {
                case 1:
                    MeteorBehavior();
                    break;
                case 0:
                    SpiralBehavior();
                    break;
            }

            if (alternateAttackTimeCounter >= alternateAttackTime)
            {
                attackType += 1;
                alternateAttackTimeCounter = 0f;
            }

            alternateAttackTimeCounter += Time.deltaTime;



        }
        else if (health > 0)
        {

            switch (attackType % 2)
            {
                case 1:
                    ShotgunBehavior();
                    SpiralBehavior();
                    break;
                case 0:
                    ShotgunBehavior();
                    MeteorBehavior();
                    break;
            }

            if (alternateAttackTimeCounter >= alternateAttackTime)
            {
                attackType += 1;
                alternateAttackTimeCounter = 0f;
            }

            alternateAttackTimeCounter += Time.deltaTime;
        }
    }

    public void SetPlayerAsAITarget()
    {
        //set target
        AIdest.target = player_transform;
    }

    public void SetTransformAsAITarget(Transform t)
    {
        AIdest.target = t;
    }

    private void FlipSprite()
    {
        //flip sprite when player is to the right
        if (playerToEnemyVector.x <= 0)
        {
            spriteRend.flipX = true;
            //transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            spriteRend.flipX = false;
            //transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private AudioSource RandomBossAttackSound()
    {
        int number = Random.Range(0, 1);
        if (number == 1)
        {
            return bossAttackOne;
        }
        else
        {
            return bossAttackTwo;
        }
    }

    private void SpiralBehavior()
    {
        if (spiralShootTimeCounter >= spiralShootTime)
        {
            spiralShootTimeCounter = 0f;
            CreateBulletAtAngle(spiralBulletPrefab, spiralBulletSpeed, -Vector3.right, thisBulletAngleOffset);
            thisBulletAngleOffset += spiralAngleOffset;
        }

        spiralShootTimeCounter += Time.deltaTime;
    }

    private void ShotgunBehavior()
    {
        if (shotgunBurstShootTimeCounter >= shotgunBurstShootTime)
        {
            SingleShotgunBurst();
            shotgunBurstShootTimeCounter = 0f;
        }

        shotgunBurstShootTimeCounter += Time.deltaTime;
    }

    private void SingleShotgunBurst()
    {
        for (int i=0; i<numOfShotgunBullets; i++)
        {
            inAttackAnimation = true;
            CreateBulletAtAngle(shotgunBurstBulletPrefab, Random.Range(shotgunBulletSpeed * 0.8f, shotgunBulletSpeed * 1.2f), -playerToEnemyVector, Random.Range(-shotgunAngleOffsetRange, shotgunAngleOffsetRange));
            AudioManager.instance.PlaySingle(RandomBossAttackSound());
        }
    }

    private void CreateBulletAtAngle(GameObject bulletPrefab, float bulletSpeed, Vector3 direction, float angleOffset = 0f)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        //bullet.transform.position = transform.position + dirVec * bulletOffset;

        direction.Normalize();
        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        rigidbody.AddForce(Quaternion.AngleAxis(angleOffset, Vector3.forward) * direction * bulletSpeed, ForceMode.Acceleration);
    }

    public void Damaged(float damage)
    {
        health -= damage;
    }

    public void Die()
    {
        //TODO:
        StartCoroutine(PlayDieSound());
    }

    IEnumerator PlayDieSound()
    {
        AudioManager.instance.PlaySingle(bossDying);
        yield return new WaitForSeconds(bossDying.clip.length + 1f);
        Destroy(gameObject);
    }

    private void SummonMeteors()
    {
        inAttackAnimation = true;
        Instantiate(meteorPrefab, player_transform.position, Quaternion.identity);
        AudioManager.instance.PlaySingle(RandomBossAttackSound());
    }

    private void MeteorBehavior()
    {
        if (meteorShootTimeCounter >= meteorShootTime)
        {
            SummonMeteors();
            meteorShootTimeCounter = 0f;
        }

        meteorShootTimeCounter += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
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
}
