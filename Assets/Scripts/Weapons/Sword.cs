using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float swingLifeTime;
    public float damage;
    public float magnitude;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //destroy bullet
        if (swingLifeTime <= 0)
        {
            Destroy(gameObject);
        }
        swingLifeTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().Damaged(damage);

            //knockback
            //Vector3 KBdirVec = new Vector3(other.gameObject.transform.position.x - transform.position.x, other.gameObject.transform.position.y - transform.position.y, 0f);
            //other.gameObject.GetComponent<EnemyController>().Knockback(KBdirVec);
        }

        //reflect bullet

        if (other.gameObject.name == "PizzaBulletPrefab(Clone)")
        {

            Vector3 dir = other.gameObject.transform.up;
            dir.x = -dir.x;
            dir.y = -dir.y;
            other.gameObject.GetComponent<Rigidbody>().AddForce(dir * magnitude, ForceMode.VelocityChange);
            other.gameObject.transform.up = -other.gameObject.transform.up;
            other.gameObject.tag = "Reflected Bullet";
        }
        else if (other.gameObject.tag == "EnemyBullet")
        {
            other.gameObject.GetComponent<EnemyBullet>().dirVec.x = -other.gameObject.GetComponent<EnemyBullet>().dirVec.x;
            other.gameObject.GetComponent<EnemyBullet>().dirVec.y = -other.gameObject.GetComponent<EnemyBullet>().dirVec.y;
            other.gameObject.transform.forward = -other.gameObject.transform.forward;
            other.gameObject.tag = "Reflected Bullet";
        }
    }
}
