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
        bulletLifeTime = 100f;
        damage = 1;
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
}
