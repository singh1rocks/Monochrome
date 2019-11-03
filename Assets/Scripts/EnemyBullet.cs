using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public PlayerMovement player;
    public Rigidbody rb;

    public Camera cam;
    public float speed;
    [SerializeField] private Vector2 target;
    [SerializeField] private Vector2 dirVec;
    [SerializeField] private Vector3 moveVec;

    public float bulletLifeTime;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        moveVec = new Vector3(dirVec.x, dirVec.y, 0);
        rb.velocity = moveVec;

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
