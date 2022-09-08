using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player") {
            Debug.Log("die");
            GameManager.Instance.GameOver();   
        }
    }
}
