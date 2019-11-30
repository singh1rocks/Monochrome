using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float swingLifeTime;
    public float damage;
    private Transform reflect; 

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
        if (other.gameObject.tag == "EnemyBullet")
        {
            reflect.position = Vector3.Reflect(other.gameObject.transform.position, Vector3.forward);
        }
    }
}
