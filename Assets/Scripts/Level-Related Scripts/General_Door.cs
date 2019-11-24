using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_Door : MonoBehaviour
{

    public Animation Door_Open;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Door_Open.Play();
            Destroy(gameObject.GetComponent<BoxCollider>());
        }
    }
}
