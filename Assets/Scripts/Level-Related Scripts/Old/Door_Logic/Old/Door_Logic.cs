using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Logic : MonoBehaviour
{

    public GameObject Enemy_0;
    public GameObject Enemy_1;
    public GameObject Enemy_2;
    public GameObject Enemy_3;
    public GameObject Enemy_4;
    public GameObject Enemy_5;
    public GameObject Enemy_6;
    public GameObject Enemy_7;
    public GameObject Enemy_8;
    public GameObject Enemy_9;
    public GameObject Enemy_10;
    public GameObject Enemy_11;

    public GameObject Door_0;
    public GameObject Door_1;
    public GameObject Door_2;
    public GameObject Door_3;

    public List<GameObject> Enemies = new List<GameObject>();

    public List<GameObject> Doors = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Enemies.Add(Enemy_0);
        Enemies.Add(Enemy_1);
        Enemies.Add(Enemy_2);
        Enemies.Add(Enemy_3);
        Enemies.Add(Enemy_4);
        Enemies.Add(Enemy_5);
        Enemies.Add(Enemy_6);
        Enemies.Add(Enemy_7);
        Enemies.Add(Enemy_8);
        Enemies.Add(Enemy_9);
        Enemies.Add(Enemy_10);
        Enemies.Add(Enemy_11);

        Doors.Add(Door_0);
        Doors.Add(Door_1);
        Doors.Add(Door_2);
        Doors.Add(Door_3);

        Door_0.SetActive(false);
        Door_1.SetActive(false);
        Door_2.SetActive(false);
        Door_3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i<Enemies.Count; i++)
        {
            if (Enemies[i] == null)
            {
                Enemies.RemoveAt(i);
            }
        }

        if(Enemies.Count == 0)
        {
            //GameObject.Destroy(Doors[0]);
            //GameObject.Destroy(Doors[1]);
            //GameObject.Destroy(Doors[2]);
            //GameObject.Destroy(Doors[3]);
            Door_0.SetActive(false);
            Door_1.SetActive(false);
            Door_2.SetActive(false);
            Door_3.SetActive(false);
            //Doors.RemoveAt(0);
            //Doors.RemoveAt(1);
            //Doors.RemoveAt(2);
            //Doors.RemoveAt(3);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Door_0.SetActive(true);
            Door_1.SetActive(true);
            Door_2.SetActive(true);
            Door_3.SetActive(true);
        }
    }
}
