using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Logic_Lockers : MonoBehaviour
{

    public GameObject Enemy_0;
    public GameObject Enemy_1;

    public GameObject Door_0;
    public GameObject Door_1;

    public List<GameObject> Enemies = new List<GameObject>();

    public List<GameObject> Doors = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Enemies.Add(Enemy_0);
        Enemies.Add(Enemy_1);

        Doors.Add(Door_0);
        Doors.Add(Door_1);
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
            GameObject.Destroy(Doors[0]);
            GameObject.Destroy(Doors[1]);
            Doors.RemoveAt(0);
            Doors.RemoveAt(1);
        }
    }
}
