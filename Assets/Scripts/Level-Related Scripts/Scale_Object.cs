using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_Object : MonoBehaviour
{
    public List<GameObject> objectList;

    private bool spaceEntered;

    public float height;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name == "Player")
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].transform.localScale -= new Vector3(0.0f, 0.0f, height);
            }

        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].transform.localScale += new Vector3(0.0f, 0.0f, height);
            }
        }
    }
}
