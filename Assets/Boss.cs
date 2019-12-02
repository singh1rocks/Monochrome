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

    [Header("Pathfinding")]
    public IAstarAI ai;
    public AIDestinationSetter AIdest;
    public AIPath aiPath;

    [Header("Bools")]
    public bool FSCoroutineRunning;

    [Header("Phase 1")]
    [Space]
    [Header("Spiral Attack Values")]
    public float spiralShootTime;
    private float spiralShootTimeCounter;
    public float spiralAngleOffset;
    private float thisBulletAngleOffset;
    public float spiralBulletSpeed;
    public GameObject spiralBulletPrefab;

    [Header("Shotgun")]
    public float shotgunBurstShootTime;
    private float shotgunBurstShootTimeCounter;
    public int numOfShotgunBullets;
    public float shotgunAngleOffsetRange;
    public float shotgunBulletSpeed;
    public GameObject shotgunBurstBulletPrefab;
    


    public enum BossAttackType
    {
        SpinningDisk,
        Meteor,
        ShotgunBurst,
        Spiral,
        Ads
    }

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

        AstarPath.FindAstarPath();
        AstarPath.active.Scan();
        AIdest = gameObject.GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        aiPath.canMove = true;

        ai = GetComponent<IAstarAI>();

        ai.maxSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        //death condition
        if (health <= 0f)
        {
            Die();
        }

        playerToEnemyVector = new Vector3(transform.position.x - player_transform.position.x, transform.position.y - player_transform.position.y, 0f);
        playerToEnemyVector.Normalize();

        SetPlayerAsAITarget();

        if (health >= maxHealth/2)
        {
            //phase 1
            //shotgun burst
            //spiral

            //Spiral();
            //
            if (shotgunBurstShootTimeCounter >= shotgunBurstShootTime)
            {
                SingleShotgunBurst();
                shotgunBurstShootTimeCounter = 0f;
            }

            shotgunBurstShootTimeCounter += Time.deltaTime;
        }
        else
        {
            //phase 2
            //spinning disk
            //ads
            //meteor


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
        }
        else
        {
            spriteRend.flipX = false;
        }
    }

    private void Spiral()
    {
        if (spiralShootTimeCounter >= spiralShootTime)
        {
            spiralShootTimeCounter = 0f;
            CreateBulletAtAngle(spiralBulletPrefab, spiralBulletSpeed, -Vector3.right, thisBulletAngleOffset);
            thisBulletAngleOffset += spiralAngleOffset;
        }

        spiralShootTimeCounter += Time.deltaTime;
    }

    private void SingleShotgunBurst()
    {
        for (int i=0; i<numOfShotgunBullets; i++)
        {
            CreateBulletAtAngle(shotgunBurstBulletPrefab, Random.Range(shotgunBulletSpeed * 0.8f, shotgunBulletSpeed * 1.2f), -playerToEnemyVector, Random.Range(-shotgunAngleOffsetRange, shotgunAngleOffsetRange));
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
        Destroy(gameObject);
    }
}
