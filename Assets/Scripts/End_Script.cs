using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Script : MonoBehaviour
{

    public GameObject end_sprite;
    public GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        end_sprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (boss == null)
        {
            end_sprite.SetActive(true);
        }
    }
}
