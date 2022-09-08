using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    private int playerScore;
    [SerializeField] private GameObject[] listItems;
    [HideInInspector] public UnityEvent<int> onUpdateScore;
    [HideInInspector] public UnityEvent onGameOver;
    private bool isGameOver = false;
    private List<Transform> listSpawnPoint;
    

    private void Awake() {
        if(Instance != null && Instance !=this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        listSpawnPoint = new List<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        RandomSpawnItems();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int score) {
        playerScore += score;
        onUpdateScore?.Invoke(playerScore);
    }

    public void AddSpawnPoint(Transform spawnPoint) {
        listSpawnPoint.Add(spawnPoint);
    }

    public void RandomSpawnItems(Transform currentPoint = null) {
        int itemIndex = Random.Range(0, listItems.Length);
        int spawnPointIndex = Random.Range(0, listSpawnPoint.Count);
        if(currentPoint != null && listSpawnPoint.IndexOf(currentPoint) == spawnPointIndex) {
            RandomSpawnItems(currentPoint);
        } else {
            Transform spawnPoint = listSpawnPoint[spawnPointIndex];
            Instantiate(listItems[itemIndex], spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void GameOver() {
        if(!isGameOver) {
            StartCoroutine(waitGameover());
        }
    }

    IEnumerator waitGameover() {
        yield return new WaitForSeconds(0.3f);
        isGameOver = true;
        Time.timeScale = 0;
        onGameOver?.Invoke();
    }
}
