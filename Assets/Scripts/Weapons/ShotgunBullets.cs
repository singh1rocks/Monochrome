using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullets : MonoBehaviour
{
    public float bulletLifeTime;
    public float damage;
    public float rotationRate;

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (bulletLifeTime <= 0)
        {
            Destroy(gameObject);
        }
        bulletLifeTime -= Time.deltaTime;

        /*
        //spin
        Quaternion myRotation = Quaternion.identity;
        myRotation.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + rotationRate);
        transform.rotation = myRotation;
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            if (other.gameObject.GetComponent<EnemyController>())
            {
                other.gameObject.GetComponent<EnemyController>().Damaged(damage);
                Destroy(gameObject);
            }
            else if (other.gameObject.GetComponent<Boss>())
            {
                other.gameObject.GetComponent<Boss>().Damaged(damage);
                Destroy(gameObject);
            }
        }
        if (other.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
