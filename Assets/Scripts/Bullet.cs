using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlayerMovement player;

    public Camera cam;
    public float speed;
    public Rigidbody rb;
    [SerializeField]Vector2 target;
    [SerializeField]Vector2 dirVec;

    public float bulletLifeTime;
    public float damage;
    public bool canPierceEnemies;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        cam = Camera.main;
        var mousePos = Input.mousePosition;
        mousePos.z = 5;
        target = cam.ScreenToWorldPoint(mousePos);
        dirVec = new Vector3(target.x - transform.position.x, target.y - transform.position.y, 0f);
        dirVec.Normalize();
        rb = GetComponent<Rigidbody>();

        //orientate bullet
        Vector2 direction = new Vector2(dirVec.x - transform.position.x, dirVec.y - transform.position.y);
        transform.up = dirVec;

        bulletLifeTime = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = dirVec * speed;

        //orientate bullet
        Vector2 direction = new Vector2(dirVec.x - transform.position.x, dirVec.y - transform.position.y);
        transform.up = dirVec;

        //destroy bullet
        if (bulletLifeTime <= 0)
        {
            Destroy(gameObject);
        }
        bulletLifeTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().health -= damage;
            if (!canPierceEnemies)
            {
                Destroy(gameObject);

            }
        }

        if (other.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
