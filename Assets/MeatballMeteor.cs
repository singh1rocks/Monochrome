using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatballMeteor : MonoBehaviour
{
    private SpriteRenderer spriteRend;

    public float damage;
    [SerializeField] private float spawnMeteorAfterSeconds;
    private float spawnMeteorCounter;
    public GameObject meteorPrefab;
    private GameObject thisMeteorProjectile;
    [SerializeField] private float meteorOffset;
    private bool playerCanBeDamaged;
    public float playerDamageTimeWindow;

    [Header("Flashing")]
    [SerializeField] private float frequency;
    [SerializeField] private int number;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        spawnMeteorCounter = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FlashSprite(frequency, number));

        if (spawnMeteorCounter >= spawnMeteorAfterSeconds)
        {
            thisMeteorProjectile = Instantiate(meteorPrefab, transform.position + meteorOffset * Vector3.up, Quaternion.identity);
            thisMeteorProjectile.GetComponent<MeteorProjectile>().target = transform.position;
            GameObject nextProjectile = Instantiate(meteorPrefab, transform.position + meteorOffset * Vector3.up + Vector3.right, Quaternion.identity);
            nextProjectile.GetComponent<MeteorProjectile>().target = transform.position + Vector3.right/2;
            nextProjectile = Instantiate(meteorPrefab, transform.position + meteorOffset * Vector3.up - Vector3.right, Quaternion.identity);
            nextProjectile.GetComponent<MeteorProjectile>().target = transform.position - Vector3.right/2;
            nextProjectile = Instantiate(meteorPrefab, transform.position + meteorOffset * Vector3.up + Vector3.up, Quaternion.identity);
            nextProjectile.GetComponent<MeteorProjectile>().target = transform.position + Vector3.up / 2;
            nextProjectile = Instantiate(meteorPrefab, transform.position + meteorOffset * Vector3.up - Vector3.up, Quaternion.identity);
            nextProjectile.GetComponent<MeteorProjectile>().target = transform.position - Vector3.up / 2;
            spawnMeteorCounter = 0f;
        }

        if (spawnMeteorCounter > 0f)
        {
            spawnMeteorCounter += Time.deltaTime;
        }
        

        if (thisMeteorProjectile != null)
        {
            if (thisMeteorProjectile.GetComponent<MeteorProjectile>().hasHit)
            {
                playerCanBeDamaged = true;
                spriteRend.enabled = false;
            }
        }

        if (playerCanBeDamaged)
        {
            if (playerDamageTimeWindow <= 0)
            {
                Destroy(gameObject);
            }

            playerDamageTimeWindow -= Time.deltaTime;
        }
    }

    IEnumerator FlashSprite(float frequency, int number)
    {
        for (int i = 0; i < number; i++)
        {
            spriteRend.color = Color.red;
            yield return new WaitForSeconds(1 / frequency);

            spriteRend.color = new Color(0.6320754f, 0.01684094f, 0f, 1f);
            yield return new WaitForSeconds(1 / frequency);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerCanBeDamaged)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerMovement>().DamagePlayer(damage);
                Destroy(gameObject);
            }
        }
    }


}
