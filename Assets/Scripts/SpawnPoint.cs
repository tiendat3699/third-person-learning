using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.AddSpawnPoint(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
