using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetObstacleTag : MonoBehaviour
{
    public Transform[] transforms;

    // Start is called before the first frame update
    void Start()
    {
        transforms = GetComponentsInChildren<Transform>();

        foreach (Transform t in transforms)
        {
            t.gameObject.layer = 9;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
