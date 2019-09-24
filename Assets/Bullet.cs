using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlayerMovement player;

    public Camera cam;
    public float speed;
    Vector2 target;
    Vector2 dirVec;

    public float bulletLifeTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        transform.position = player.GetComponent<Transform>().position;
        cam = Camera.main;
        target = cam.ScreenToWorldPoint(Input.mousePosition);
        dirVec = new Vector2(target.x - transform.position.x, target.y - transform.position.y);

        //point at target
        Vector2 direction = new Vector2(dirVec.x - transform.position.x, dirVec.y - transform.position.y);
        transform.right = dirVec;

        //TODO: initialize bullet life time
        bulletLifeTime = 10;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + dirVec, speed * Time.deltaTime);

        //orientate bullet
        Vector2 direction = new Vector2(dirVec.x - transform.position.x, dirVec.y - transform.position.y);
        transform.right = dirVec;

        //destroy bullet
        if (bulletLifeTime <= 0)
        {
            Destroy(gameObject);
        }
        bulletLifeTime -= Time.deltaTime;
    }
}
