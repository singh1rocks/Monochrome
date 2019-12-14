﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerTriggerScript : MonoBehaviour
{
    public float damage;
    public float flameTimeCounter;
    public float flameCounter;

    //Changes the sprite
    public ShootBullet shootBulletScript;
    private Vector3 dirVec;
    public SpriteRenderer spriteRend;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        shootBulletScript = GameObject.FindWithTag("Player").GetComponent<ShootBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        dirVec = shootBulletScript.dirVec;
        //flip sprite for left and right sides
        if (dirVec.x >= 0)
        {
            spriteRend.flipY = false;
        }
        else
        {
            spriteRend.flipY = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && flameTimeCounter >= flameCounter)
        {
            if (other.gameObject.GetComponent<EnemyController>())
            {
                other.gameObject.GetComponent<EnemyController>().Damaged(damage);
            }
            else if (other.gameObject.GetComponent<Boss>())
            {
                other.gameObject.GetComponent<Boss>().Damaged(damage);
            }
            
            flameTimeCounter = 0;
        }

        flameTimeCounter += Time.deltaTime;
    }
}
