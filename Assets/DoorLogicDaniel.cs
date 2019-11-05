using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogicDaniel : MonoBehaviour
{
    private BoxCollider enemyCountTrigger;
    private bool hasScannedEnemies;
    public List<GameObject> enemiesList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hasScannedEnemies)
        {
            //if ()
            {

            }
        }
        hasScannedEnemies = true;
    }
}
