using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBullet : MonoBehaviour
{
    public PlayerMovement player;

    public Camera cam;
    public float speed;
    public Rigidbody rb;
    [SerializeField] Vector2 target;
    [SerializeField] Vector2 dirVec;

    public float bulletLifeTime;
    public float damage;


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

        //point at target
        //Vector2 direction = new Vector2(dirVec.x - transform.position.x, dirVec.y - transform.position.y);
        //transform.right = dirVec;

        speed = 4f;
        bulletLifeTime = .5f;
        damage = .25f;
        transform.forward = dirVec;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = dirVec * speed;
        transform.up = dirVec;

        //destroy bullet
        if (bulletLifeTime <= 0)
        {
            Destroy(gameObject);
        }
        bulletLifeTime -= Time.deltaTime;
    }
}
