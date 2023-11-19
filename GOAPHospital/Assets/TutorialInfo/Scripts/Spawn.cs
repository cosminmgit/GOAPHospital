using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject patienPrefab;
    public int numberOfPatients;

    private void Start()
    {
        for(int i = 0; i < numberOfPatients; i++)
        {
            Instantiate(patienPrefab, transform.position, transform.rotation);
        }

     //   Invoke("SpawnPatient", 5f);
        Invoke("SpawnPatient", 5f);
    }

    private void SpawnPatient()
    {
        Instantiate(patienPrefab, transform.position, transform.rotation);
        Invoke("SpawnPatient",5f);
    }

}
