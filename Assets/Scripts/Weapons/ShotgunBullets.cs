using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullets : MonoBehaviour
{
    public float bulletLifeTime;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
            Destroy(gameObject);
        }
    }
}
