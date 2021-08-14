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

    // Maximum level reached
    public int max_level;

    // Previous and Next Buttons
    public Button previous_button;
    public Button next_button;

    // Grounds
    public GameObject[] Grounds;
    private GameObject groundInstance;

    // Backgrounds
    public GameObject[] BackgroundGroups;
    private GameObject backgroundGroupInstance;  

    // Start is called before the first frame update
    void Start()
    {
        // Set Game controller instance
        instance = this;

        // Load the game that was saved
        GameData data = SaveSystem.LoadGame();
        // If there is no saved game, then create a date with your information
        if (data != null)
        {
            // Current level
            level = data.level;

            // Max level reached
            max_level = data.max_level;

            // Displays the previous and next level buttons according to the current level
            if (max_level > level)
            {
                next_button.gameObject.SetActive(true);
            } 
            else
            {
                next_button.gameObject.SetActive(false);
            }
            if (level > 1)
            {
                previous_button.gameObject.SetActive(true);
            }
            else
            {
                previous_button.gameObject.SetActive(false);
            }
        }
        else
        {
            // Initial level when you don't have a saved file
            level = 1;
            max_level = 1;

            // Does not display the previous and next buttons
            previous_button.gameObject.SetActive(false);
            next_button.gameObject.SetActive(false);
        }

        // Modify the text of the level
        levelShow.text = "Fase " + level;

        // Instantiate one of the 4 available enemy prefabs
        instantiateEnemy();

        // Leave the scale that time passes at one
        Time.timeScale = 1f;

        // Instantiate an Background and ground
        instantiateBackgroundGroup();
        instantiateGround();
    }

    // Update is called once per frame
    void Update()
    {
        // If the enemy in the level has already been defeated, if you lean against the wall on the right, it moves to the next level
        if (enemyInstance == null)
        {
            if (player.GetComponent<Transform>().position.x > 2.97f)
            {
                // Save current player
                SaveSystem.SavePlayer(player.GetComponent<PlayerController>());
                
                // Go to the next level
                nextLevel();

                // Save current game
                SaveSystem.SaveGame(this);

                // Instantiate one of the 4 available enemy prefabs in the next level
                int i = Random.Range(0, 4);
                enemyInstance = Instantiate(Enemies[i]);                
            }
        }
    }

    // Function to access the next level of a stage already reached by the button
    public void goNextLevel()
    {
        // Destroy current enemy
        Destroy(GameObject.FindGameObjectWithTag("Enemy"));

        // Save current player
        SaveSystem.SavePlayer(player.GetComponent<PlayerController>());

        // Go to the next level
        nextLevel();

        // Save current game
        SaveSystem.SaveGame(this);

        // Instantiate one of the 4 available enemy prefabs in the next level
        int i = Random.Range(0, 4);
        enemyInstance = Instantiate(Enemies[i]);

    }

    // Function to call the next level
    void nextLevel()
    {
        // Increases the level
        level += 1;

        // Increases the max level and disable or enable the previous and next buttons
        if (level >= max_level)
        {
            max_level = level;
            previous_button.gameObject.SetActive(true);
            next_button.gameObject.SetActive(false);
        } 
        else
        {
            previous_button.gameObject.SetActive(true);
            next_button.gameObject.SetActive(true);
        }            

        // Modify the text of the level
        levelShow.text = "Fase " + level;

        // Instantiate an Background and ground
        instantiateBackgroundGroup();
        instantiateGround();

        // Restart Position of Player
        RestartPosition();
        new WaitForSecondsRealtime(0.5f);
    }

    // Function to access the previous level of a stage already reached by the button
    public void goPreviousLevel()
    {
        // Destroy current enemy
        Destroy(GameObject.FindGameObjectWithTag("Enemy"));

        // Save current player
        SaveSystem.SavePlayer(player.GetComponent<PlayerController>());

        // Go to the previous level
        previousLevel();

        // Save current game
        SaveSystem.SaveGame(this);

        // Instantiate one of the 4 available enemy prefabs in the next level
        int i = Random.Range(0, 4);
        enemyInstance = Instantiate(Enemies[i]);
    }

    // Function to call the previous level
    void previousLevel()
    {
        // Decreases the level
        level -= 1;

        // Disable or enable the previous and next buttons
        if (level == 1)
        {
            previous_button.gameObject.SetActive(false);
            next_button.gameObject.SetActive(true);
        }
        else
        {
            previous_button.gameObject.SetActive(true);
            next_button.gameObject.SetActive(true);
        }

        // Modify the text of the level
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

    // Function to start new game
    public void NewGame()
    {
        // Initial level
        level = 1;

        // Modify the text of the level
        levelShow.text = "Fase " + level;

        // Go to the game scene
        SceneManager.LoadScene("Game");
    }


    // Function that instantiate one of the 4 available enemy prefabs
    private void instantiateEnemy()
    {
        // Instantiate one of the 4 available enemy prefabs
        int i = Random.Range(0, 4);
        enemyInstance = Instantiate(Enemies[i]);
    }

    // Function that instantiate one of the 4 available ground
    private void instantiateGround()
    {
        // Disables all ground and activates the draw
        if (groundInstance != null){
            groundInstance.SetActive(false);
        }
        int i = Random.Range(0, 4);
        groundInstance = Grounds[i];
        groundInstance.SetActive(true);
    }

    // Function that instantiate one of the 4 available backgrounds
    private void instantiateBackgroundGroup()
    {
        // Disables all backgrounds and activates the draw
        if (backgroundGroupInstance != null){
            backgroundGroupInstance.SetActive(false);
        }
        int i = Random.Range(0, 4);
        backgroundGroupInstance = BackgroundGroups[i];
        backgroundGroupInstance.SetActive(true);
    }
}
