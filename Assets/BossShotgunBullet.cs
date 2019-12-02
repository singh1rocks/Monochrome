using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotgunBullet : MonoBehaviour
{
    public float damage;
    public float lifetime;
    private GameObject player;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0f)
        {
            lifetime = 5f;
        }

        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.drag = Random.Range(2f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
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
