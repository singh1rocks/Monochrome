﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Vector2 target;
    public Transform transform;
    public float health = 5f;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public float bulletTime; //minimum delay between each bullet fired
    public float bulletTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        health = 5f;
        //init bullet values
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target = new Vector2(transform.position.x + Input.GetAxisRaw("Horizontal") * speed, transform.position.y + Input.GetAxisRaw("Vertical") * speed);
        transform.position = Vector2.MoveTowards(transform.position, target, Mathf.Infinity);

        //shoot bullet
        if (Input.GetMouseButton(0) && bulletTimeCounter >= bulletTime)
        {
            Instantiate(bulletPrefab);
            bulletTimeCounter = 0;
        }
        bulletTimeCounter += Time.deltaTime;

        if (health<=0)
        {
            Debug.Log("Game Over");
        }
    }
}