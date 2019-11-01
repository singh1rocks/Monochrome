using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionRadius;
    public float knockbackDistance;
    Rigidbody rb;
    public float explosionForce;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0f, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(1f, 0.92f, 0.016f, 0.3f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
