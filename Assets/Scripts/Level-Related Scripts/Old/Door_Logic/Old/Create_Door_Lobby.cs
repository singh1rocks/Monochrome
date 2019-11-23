using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Door_Lobby : MonoBehaviour
{
    public GameObject Door_0;
    public GameObject Door_1;
    public GameObject Door_2;
    public GameObject Door_3;

    // Start is called before the first frame update
    void Start()
    {
        Door_0.SetActive(false);
        Door_1.SetActive(false);
        Door_2.SetActive(false);
        Door_3.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        Door_0.SetActive(true);
        Door_1.SetActive(true);
        Door_2.SetActive(true);
        Door_3.SetActive(true);
    }
}
