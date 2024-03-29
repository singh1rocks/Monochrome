﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaconCrossbow : MonoBehaviour
{
    public GameObject flamebulletPrefab;
    public float bulletTime; //minimum delay between each bullet fired
    public float bulletTimeCounter;
    private PlayerMovement p_controller;
    private Camera cam;
    private Vector3 target;
    private Vector3 dirVec;
    public float bulletOffset;
    private Vector3 mousePos;


    // Start is called before the first frame update
    void Start()
    {
        p_controller = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        cam = Camera.main;
        mousePos = Input.mousePosition;
        mousePos.z = 5;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 5;
        target = cam.ScreenToWorldPoint(mousePos);
        dirVec = new Vector3(target.x - transform.position.x, target.y - transform.position.y, 0f);
        dirVec.Normalize();

        //shoot bullet
        if (Input.GetMouseButton(0) && bulletTimeCounter >= bulletTime)
        {
            Instantiate(flamebulletPrefab, transform.position + dirVec * bulletOffset, Quaternion.identity);
            bulletTimeCounter = 0;
        }
        bulletTimeCounter += Time.deltaTime;
    }
}
