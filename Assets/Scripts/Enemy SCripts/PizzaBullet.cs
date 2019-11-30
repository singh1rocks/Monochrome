using UnityEngine;

public class PizzaBullet : MonoBehaviour
{
    public float damage;
    PlayerMovement player;
    public float lifetime;
    public Rigidbody rb;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        if (lifetime <= 0f)
        {
            lifetime = 5f;
        }
       rb = GetComponent<Rigidbody>();
    }

    private void Update()
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
            player.DamagePlayer(damage);
        }

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
