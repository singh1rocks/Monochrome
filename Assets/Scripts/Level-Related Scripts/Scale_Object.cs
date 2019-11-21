using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_Object : MonoBehaviour
{
    public List<GameObject> objectList;

    private bool spaceEntered;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spaceEntered == true)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name == "Player")
        {
            spaceEntered = true;
        }
    }
}
