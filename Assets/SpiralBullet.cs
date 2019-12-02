using UnityEngine;

public class SpiralBullet : MonoBehaviour
{
    public float damage;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
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
