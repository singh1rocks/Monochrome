using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<GameObject> enemyList;
    public List<Vector3> enemyPositions;
    public DoorState thisRoomState;
    public GameObject entranceDoor;
    public GameObject exitDoor;
    public GameObject otherDoor_0;

    public GameObject Table_D;

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

        int enemiesAlive = 0;

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].gameObject.GetComponent<EnemyController>().health > 0)
            {
                enemiesAlive++;
            }
        }

        if (thisRoomState == DoorState.notEntered)
        {
            entranceDoor.gameObject.SetActive(false);
            exitDoor.gameObject.SetActive(false);
            if (otherDoor_0.gameObject != null)
            {
                otherDoor_0.gameObject.SetActive(false);
            }
        }

        if (enemiesAlive == 0 && thisRoomState == DoorState.entered)
        {
            //open doors after all enemies are killed
            exitDoor.gameObject.SetActive(false);
            entranceDoor.gameObject.SetActive(false);
            if (otherDoor_0.gameObject != null)
            {
                otherDoor_0.gameObject.SetActive(false);
            }
            thisRoomState = DoorState.canExit;

            if (gameObject.name == "Trigger_Final")
            {
                Destroy(Table_D);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && thisRoomState == DoorState.notEntered)
        {
            thisRoomState = DoorState.entered;

            //close doors to trap player inside
            entranceDoor.gameObject.SetActive(true);
            exitDoor.gameObject.SetActive(true);
            if (otherDoor_0.gameObject != null)
            {
                otherDoor_0.gameObject.SetActive(false);
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                Debug.Log("set enemy active");
                enemyList[i].gameObject.SetActive(true);
            }
        }

        if (other.gameObject.tag == "Enemy")
        {
            if (!enemyList.Contains(other.gameObject))
            {
                enemyList.Add(other.gameObject);
                enemyPositions.Add(other.gameObject.transform.position);
                other.gameObject.SetActive(false);
            }
        }
    }
    
    public void RespawnEnemies()
    {
        for (int i=0; i<enemyList.Count; i++)
        {
            if (enemyList[i].gameObject.GetComponent<EnemyController>().health <= 0)
            {
                EnemyController thisEnemy = enemyList[i].gameObject.GetComponent<EnemyController>();
                thisEnemy.health = thisEnemy.maxHealth;
            }
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            EnemyController thisEnemy = enemyList[i].gameObject.GetComponent<EnemyController>();
            thisEnemy.transform.position = enemyPositions[i];
            if (thisEnemy.FSCoroutine != null && thisEnemy.FSCoroutineRunning)
            {
                StopCoroutine(thisEnemy.FSCoroutine);
                thisEnemy.FSCoroutineRunning = false;
            }
            thisEnemy.spriteRend.color = new Color(thisEnemy.spriteRend.color.r, thisEnemy.spriteRend.color.g, thisEnemy.spriteRend.color.b, 1f);
            enemyList[i].gameObject.SetActive(false);
        }

        thisRoomState = DoorState.notEntered;
    }
}
