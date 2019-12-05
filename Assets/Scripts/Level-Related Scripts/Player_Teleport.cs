using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Teleport : MonoBehaviour
{

    public float x;
    public float y;
    private float z = 0.81f;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(FindObjectOfType<FadeWhenChangingFloors>().FadeAndMovePlayerTransform(new Vector3(x, y, z)));
        }
    }
}
