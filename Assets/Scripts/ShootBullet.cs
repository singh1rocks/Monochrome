using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletTime; //minimum delay between each bullet fired
    public float bulletTimeCounter;
    public Transform p_transform;

    // Start is called before the first frame update
    void Start()
    {
        p_transform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //shoot bullet
        if (Input.GetMouseButton(0) && bulletTimeCounter >= bulletTime)
        {
            Instantiate(bulletPrefab, p_transform.position, Quaternion.identity);
            bulletTimeCounter = 0;
        }
        bulletTimeCounter += Time.deltaTime;
    }
}
