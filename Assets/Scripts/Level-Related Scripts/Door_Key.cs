using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Key : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player" && gameObject.name == "Door_0" && GameVariables.key_0 >0)
        {
            GameVariables.key_0 --;
            Destroy(gameObject);
            Debug.Log(GameVariables.key_0);  
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "Door_1" && GameVariables.key_1 > 0)
        {
            GameVariables.key_1--;
            Destroy(gameObject);
            Debug.Log(GameVariables.key_1);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "Door_2" && GameVariables.key_2 > 0)
        {
            GameVariables.key_2--;
            Destroy(gameObject);
            Debug.Log(GameVariables.key_2);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "Door_3" && GameVariables.key_3 > 0)
        {
            GameVariables.key_3--;
            Destroy(gameObject);
            Debug.Log(GameVariables.key_3);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "Door_4" && GameVariables.key_4 > 0)
        {
            GameVariables.key_4--;
            Destroy(gameObject);
            Debug.Log(GameVariables.key_4);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "Door_5" && GameVariables.key_5 > 0)
        {
            GameVariables.key_5--;
            Destroy(gameObject);
            Debug.Log(GameVariables.key_5);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "Door_6" || gameObject.name == "Door_6_1" && GameVariables.key_6 > 0)
        {
            GameVariables.key_6--;
            Destroy(gameObject);
            Debug.Log(GameVariables.key_6);
        }

        if (collider.gameObject.name == "Player" && gameObject.name == "Door_7" && GameVariables.key_7 > 0)
        {
            GameVariables.key_7--;
            Destroy(gameObject);
            Debug.Log(GameVariables.key_7);
        }
    }
}
