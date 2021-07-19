using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject gameOver;
    public static GameController instance;
    public GameObject StatsMenu;
    
    public GameObject player;
    public GameObject Enemy;
    private GameObject enemyInstance;
    public Text levelShow;

    public int level;

    void Start()
    {
        instance = this;
        level = 1;
        enemyInstance = Instantiate(Enemy);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyInstance == null)
        {
            if (player.GetComponent<Transform>().position.x > 3)
            {
                SaveSystem.SavePlayer(player.GetComponent<PlayerController>());
                nextLevel();
                enemyInstance = Instantiate(Enemy);
            }
        }
        if (Input.GetKey(KeyCode.Escape))
        {

        }
    }

    void nextLevel()
    {
        level += 1;
        levelShow.text = "Fase " + level;
        RestartPosition();
        new WaitForSecondsRealtime(0.5f);
    }

    void RestartPosition()
    {
        Vector2 position = player.GetComponent<Transform>().position;
        position.x = -2.2f;
        player.GetComponent<Transform>().position = position;
    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MenuInicial");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
