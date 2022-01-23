using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnAnts : MonoBehaviour
{
    public Transform ant;
    public int AntAmount;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < AntAmount; i++) {
            Instantiate(ant);
        }
    }
}
