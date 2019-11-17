using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private Vector3 target;
    private Transform grenade_t;
    private Rigidbody grenade_rb;
    private Transform player_t;
    [SerializeField] private float grenadeDelay; //time from grenade landing, to detonating
    private float grenadeDelayCounter;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float launchSpeed;
    private Vector3 dirVec;

    // Start is called before the first frame update
    void Start()
    {
        grenade_t = GetComponent<Transform>();
        grenade_rb = GetComponent<Rigidbody>();
        player_t = GameObject.FindWithTag("Player").transform;
        grenadeDelayCounter = 0;
        target = player_t.gameObject.GetComponent<ShootBullet>().target;
        dirVec = player_t.gameObject.GetComponent<ShootBullet>().dirVec;
        SetFireVel();
    }

    // Update is called once per frame
    void Update()
    {
        if (grenadeDelayCounter >= grenadeDelay)
        {
            //detonate grenade
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        grenadeDelayCounter += Time.deltaTime;
    }

    public void SetFireVel()
    {
        grenade_rb.velocity = new Vector3(dirVec.x * launchSpeed, dirVec.y * launchSpeed, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //detonate grenade of has landed and has hit grenade even if delay time has not elapsed
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
