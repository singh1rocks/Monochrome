using UnityEngine;

public class SpiralBullet : MonoBehaviour
{
    public float damage;
    private GameObject player;
    public float lifetime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && gameObject.tag == "EnemyBullet")
        {
            player.GetComponent<PlayerMovement>().DamagePlayer(damage);
            //Debug.Log("Bullet hit player");
        }

        if (collision.tag == "Obstacle" || collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
