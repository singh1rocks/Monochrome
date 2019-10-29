using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionRadius;
    public float knockbackDistance;

    // Start is called before the first frame update
    void Start()
    {
        ExplosionDamageAndKnockback(transform.position, explosionRadius);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ExplosionDamageAndKnockback(Vector2 center, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        foreach (Collider2D k in hitColliders)
        {
            if (k.gameObject.tag == "Enemy")
            {
                EnemyController EC = k.gameObject.GetComponent<EnemyController>();
                EC.health -= (float)1.5;
                EC.isBeingKnockedBack = true;
                Vector3 knockbackDirVec = new Vector3(k.transform.position.x - gameObject.transform.position.x, k.transform.position.y - gameObject.transform.position.y, k.transform.position.z);
                knockbackDirVec.Normalize();
                Vector3 target = EC.transform.position + (knockbackDirVec * knockbackDistance);
                StartCoroutine(EC.Knockback(target));

            }
            else if (k.gameObject.tag == "Player")
            {
                /*
                k.gameObject.GetComponent<PlayerMovement>().health -= (float)0.75;
                Vector2 knockbackDirVec = new Vector2(k.transform.position.x - gameObject.transform.position.x, k.transform.position.y - gameObject.transform.position.y);
                gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirVec);
                Debug.Log("hit but kamikaze explosion, dirvec:" + knockbackDirVec.x + " " + knockbackDirVec.y);
                */
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(1f, 0.92f, 0.016f, 0.3f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
