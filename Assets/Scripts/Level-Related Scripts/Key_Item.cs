using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Item : MonoBehaviour
{
    // Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            GameVariables.keyCount += 2;
            Destroy(gameObject);
            Debug.Log(GameVariables.keyCount);
        }
    }
}
