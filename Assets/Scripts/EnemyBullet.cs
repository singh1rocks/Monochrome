using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public PlayerMovement player;

    public Camera cam;
    public float speed;
    [SerializeField] Vector2 target;
    [SerializeField] Vector2 dirVec;

    public float bulletLifeTime;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        target = player.transform.position;
        dirVec = new Vector2(target.x - transform.position.x, target.y - transform.position.y);

        //point at target
        //Vector2 direction = new Vector2(dirVec.x - transform.position.x, dirVec.y - transform.position.y);
        //transform.right = dirVec;

        //TODO: initialize bullet life time
        speed = 0.4f;
        bulletLifeTime = 10f;
        damage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + dirVec, speed * Time.deltaTime);

        //orientate bullet
        //Vector2 direction = new Vector2(dirVec.x - transform.position.x, dirVec.y - transform.position.y);
        //transform.right = dirVec

        //destroy bullet
        if (bulletLifeTime <= 0)
        {
            Destroy(gameObject);
        }
        bulletLifeTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.GetComponent<PlayerMovement>().health -= damage;
            Debug.Log("Bullet hit player");
            Destroy(gameObject);
        }
    }
}
