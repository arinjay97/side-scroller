using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] AudioClip deathSFX;
    AudioSource source;

    [SerializeField] TextMeshProUGUI availableLives;
    [SerializeField] TextMeshProUGUI currentScore;

    void Awake()
    {
        int numberOfSessions = FindObjectsOfType<GameSession>().Length;
        if(numberOfSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }    
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        availableLives.text = "Lives: " + playerLives.ToString();
        currentScore.text = "Score: " + score.ToString();
    }

    public void playerDeath()
    {
        if (playerLives > 1)
        {
            
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int scoreToAdd)
    {
        score += scoreToAdd;
        currentScore.text = "Score: " + score.ToString();
    }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(4);
        Destroy(gameObject);
    }

    void TakeLife()
    {
        playerLives--;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        source.PlayOneShot(deathSFX, 0.3f);
        SceneManager.LoadScene(currentScene);
        availableLives.text = "Lives: " + playerLives.ToString();
    }
}
