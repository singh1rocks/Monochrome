using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBullet : MonoBehaviour
{
    public float damage;
    PlayerMovement player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.DamagePlayer(damage);
        }

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
