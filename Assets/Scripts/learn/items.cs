using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class items : MonoBehaviour
{
    [SerializeField] private int score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            GameManager.Instance.UpdateScore(score);
            GameManager.Instance.RandomSpawnItems(transform);
            Destroy(gameObject);
        }
    }
}
