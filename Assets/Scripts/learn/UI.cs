using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text gameOverText;
    public Text scoreText;
    private void Awake() {
        gameOverText.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.onGameOver.AddListener(()=> gameOverText.gameObject.SetActive(true));
        GameManager.Instance.onUpdateScore.AddListener((score)=> scoreText.text = "Score: "+ score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
