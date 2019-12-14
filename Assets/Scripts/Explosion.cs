using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage;
    [SerializeField] private float lifetime; //so the explosion object exists until the animation finishes playing
    public AudioSource explosionSFX;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySingle(explosionSFX);
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerMovement PM = other.gameObject.GetComponent<PlayerMovement>();
            PM.DamagePlayer(damage);

            Vector3 KBdirVec = new Vector3(PM.transform.position.x - transform.position.x, PM.transform.position.y - transform.position.y, 0f);
            PM.Knockback(KBdirVec);
        }
        else if(other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<Boss>())
            {
                other.gameObject.GetComponent<Boss>().Damaged(damage);
            }
            else
            {
                EnemyController EC = other.gameObject.GetComponent<EnemyController>();
                EC.Damaged(damage);

                Vector3 KBdirVec = new Vector3(EC.transform.position.x - transform.position.x, EC.transform.position.y - transform.position.y, 0f);
                EC.Knockback(KBdirVec);
            }

            
        }
        //Destroy(gameObject);
    }
}
