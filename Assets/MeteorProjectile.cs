using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorProjectile : MonoBehaviour
{
    public Vector3 target;
    public float speed;
    public bool hasHit;

    public float timeFromHitToDestroy;

    private SpriteRenderer renderer;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        hasHit = false;

        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("hasHit", hasHit);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position == target)
        {
            hasHit = true;
            StartCoroutine(Fade());
        }

        if (hasHit)
        {

            if (timeFromHitToDestroy <= 0f)
            {
                Destroy(gameObject);
            }

            timeFromHitToDestroy -= Time.deltaTime;
        }
    }

    IEnumerator Fade()
    {
        for (float ft = 1f; ft >= 0; ft -= 0.01f)
        {
            Color c = renderer.material.color;
            c.a = ft;
            renderer.material.color = c;
            yield return null;
        }
    }
}
