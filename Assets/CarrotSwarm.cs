using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotSwarm : MonoBehaviour
{
    public int swarmPopulation;
    public GameObject carrotPrefab;
    public float spawnRange;

    private void OnEnable()
    {
        for (int i = 0; i < swarmPopulation; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-spawnRange / 2, spawnRange / 2), Random.Range(-spawnRange / 2, spawnRange / 2), 0f);
            Instantiate(carrotPrefab, spawnPos, Quaternion.identity);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
