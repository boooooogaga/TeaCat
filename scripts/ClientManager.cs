using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public int ClientCount = 0;

    public Transform[] SpotPoints;

    public GameObject[] clients;

    private Transform spawnPoint;

    public bool[] FreeSlot = new bool[]{true,true,true};

    public void SpawnClient()
    {
        ClientCount++;
        GameObject newClient = Instantiate(clients[Random.Range(0,clients.Length)], spawnPoint);
        for (int i = 0; i < FreeSlot.Length; i++)
        {
            if (FreeSlot[i] == true) 
            {
                newClient.GetComponent<ClientBeh>().spot = SpotPoints[i];
                newClient.GetComponent<ClientBeh>().Id = i;
                break; 
            }
        }
    }
}
