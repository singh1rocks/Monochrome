using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Backup1 : MonoBehaviour
{
    [SerializeField] protected List<GameObject> enemyList;
    [SerializeField] private DoorState thisRoomState;
    public GameObject entranceDoor;
    public GameObject exitDoor;
    public GameObject otherDoor_0;

    public enum DoorState
    {
        notEntered, //player hasnt entered room yet
        entered, //player has entered room, doors close behind him
        canExit //player has killed all enemies and doors open, allows him to exit room
    }

    // Start is called before the first frame update
    void Start()
    {
        thisRoomState = DoorState.notEntered;
        entranceDoor.gameObject.SetActive(false);
        exitDoor.gameObject.SetActive(false);

        if (otherDoor_0.gameObject != null)
        {
            otherDoor_0.gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<enemyList.Count; i++)
        {
            if (enemyList[i] == null)
            {
                enemyList.Remove(enemyList[i]);
            }
        }

        if (enemyList.Count <= 0 && thisRoomState == DoorState.entered)
        {
            //open doors after all enemies are killed
            exitDoor.gameObject.SetActive(false);
            entranceDoor.gameObject.SetActive(false);
            if (otherDoor_0.gameObject != null)
            {
                otherDoor_0.gameObject.SetActive(false);
            }
                
            thisRoomState = DoorState.canExit;
        }

        if (thisRoomState == DoorState.entered)
        {
            //close doors to trap player inside
            entranceDoor.gameObject.SetActive(true);
            exitDoor.gameObject.SetActive(true);
            if (otherDoor_0.gameObject != null)
            {
                otherDoor_0.gameObject.SetActive(true);
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].gameObject.GetComponent<EnemyController>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && thisRoomState == DoorState.notEntered)
        {
            thisRoomState = DoorState.entered;
        }

        if (other.gameObject.tag == "Enemy")
        {
            enemyList.Add(other.gameObject);
            other.gameObject.GetComponent<EnemyController>().enabled = false;
        }
    }
}
