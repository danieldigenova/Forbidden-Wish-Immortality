using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Game controller script
 **/

public class GameController : MonoBehaviour
{
    // Game controller instance
    public static GameController instance;

    // Game Over UI
    public GameObject gameOver;
    
    // Stats Menu UI
    public GameObject StatsMenu;
    
    // Player GameObject
    public GameObject player;

    // Prefabs of Enemies
    public GameObject[] Enemies;

    // Enemy instance
    private GameObject enemyInstance;

    // Current level
    public int level;
    public Text levelShow;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set Game controller instance
        instance = this;

        // Initial level
        level = 1;

        // Instantiate one of the 4 available enemy prefabs
        int i = Random.Range(0, 4);
        enemyInstance = Instantiate(Enemies[i]);
    }

    // Update is called once per frame
    void Update()
    {
        // If the enemy in the level has already been defeated, if you lean against the wall on the right, it moves to the next level
        if (enemyInstance == null)
        {
            if (player.GetComponent<Transform>().position.x > 3)
            {
                // Save current player
                SaveSystem.SavePlayer(player.GetComponent<PlayerController>());
                
                // Go to the next level
                nextLevel();

                // Instantiate one of the 4 available enemy prefabs in the next level
                int i = Random.Range(0, 4);
                enemyInstance = Instantiate(Enemies[i]);
            }
        }
        if (Input.GetKey(KeyCode.Escape))
        {

        }
    }

    // Function to call the new level
    void nextLevel()
    {
        // Increases the level
        level += 1;
        //Modify the text of the level
        levelShow.text = "Fase " + level;

        // Restart Position of Player
        RestartPosition();
        new WaitForSecondsRealtime(0.5f);
    }

    // Function to reset player position
    void RestartPosition()
    {
        // Set in player initial position
        Vector2 position = player.GetComponent<Transform>().position;
        position.x = -2.2f;
        player.GetComponent<Transform>().position = position;
    }

    // Function to show Game Over UI
    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }

    // Function to return to the start menu
    public void RestartGame()
    {
        SceneManager.LoadScene("MenuInicial");
    }

    // Function to go to the game scene
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Quit Game Function
    public void QuitGame()
    {
        Application.Quit();
    }
}
