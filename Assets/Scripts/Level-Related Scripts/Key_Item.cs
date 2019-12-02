using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Item : MonoBehaviour
{

    public GameObject UI;

    // Update is called once per frame
    private void Start()
    {
        UI.SetActive(false);
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player" && gameObject.name == "0")
        {
            GameVariables.key_0 += 1;
            Destroy(gameObject);
            UI.SetActive(true);
            Debug.Log(GameVariables.key_0);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "1")
        {
            GameVariables.key_1 += 1;
            Destroy(gameObject);
            UI.SetActive(true);
            Debug.Log(GameVariables.key_1);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "2")
        {
            GameVariables.key_2 += 1;
            Destroy(gameObject);
            UI.SetActive(true);
            Debug.Log(GameVariables.key_2);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "3")
        {
            GameVariables.key_3 += 1;
            Destroy(gameObject);
            UI.SetActive(true);
            Debug.Log(GameVariables.key_3);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "4")
        {
            GameVariables.key_4 += 1;
            Destroy(gameObject);
            UI.SetActive(true);
            Debug.Log(GameVariables.key_4);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "5")
        {
            GameVariables.key_5 += 1;
            Destroy(gameObject);
            UI.SetActive(true);
            Debug.Log(GameVariables.key_5);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "6")
        {
            GameVariables.key_6 += 2;
            Destroy(gameObject);
            UI.SetActive(true);
            Debug.Log(GameVariables.key_6);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "7")
        {
            GameVariables.key_7 += 1;
            Destroy(gameObject);
            Debug.Log(GameVariables.key_7);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "8")
        {
            GameVariables.key_8 += 1;
            Destroy(gameObject);
            UI.SetActive(true);
            Debug.Log(GameVariables.key_8);
        }

    }
}
