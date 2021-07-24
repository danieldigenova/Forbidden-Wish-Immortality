using UnityEngine;
using UnityEngine.UI;

/*
 * Script to control the stats menu
 **/
public class StatsMenuController : MonoBehaviour
{
    // Variable to establish whether the game is paused
    public static bool gameIsPaused = false;

    // Pause Menu UI
    public GameObject pauseMenuIU;

    // Player Game Object
    public GameObject player;
    
    // Plus Buttons
    public Button attackPlus;
    public Button lifePlus;
    public Button defensePlus;

    // Stats Texts
    public Text attackText;
    public Text defenseText;
    public Text lifeText;
    public Text PointsText;

    // Stats Points
    public int attackPoints;
    public int defensePoints;
    public int lifePoints;
    public int pointsToSpend;

    // Update is called once per frame
    void Update()
    {
        // Pause the game if you press ESC
        // Return if pressed again
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    // Function to pause the game
    void Pause()
    {
        // Load player stats points
        LoadStats();
        // Update UI with new stats
        updateIU();

        // Show the Pause Menu
        pauseMenuIU.SetActive(true);

        // Leave the scale that time passes at zero
        Time.timeScale = 0f;
        // Set the game to paused
        gameIsPaused = true;
    }

    // Function to load player stats points
    void LoadStats()
    {
        attackPoints = player.GetComponent<PlayerController>().statusPointsAttack;
        defensePoints = player.GetComponent<PlayerController>().statusPointsShield;
        lifePoints = player.GetComponent<PlayerController>().statusPointsLife;
        pointsToSpend = player.GetComponent<PlayerController>().pointsToSpend;
    }

    // Function that updates UI texts with stats points
    void updateIU()
    {
        attackText.text = "Atack Points: " + attackPoints;
        defenseText.text = "Defense Points: " + defensePoints;
        lifeText.text = "Life Points: " + lifePoints;
        PointsText.text = "Total Points: " + pointsToSpend;
    }

    // Function to save player progress stats
    void saveStats()
    {
        SaveSystem.SavePlayer(player.GetComponent<PlayerController>());
    }

    // Function to resume the game
    void Resume()
    {
        // No longer show the Pause menu UI
        pauseMenuIU.SetActive(false);
        // Leave the scale that time passes at one
        Time.timeScale = 1f;
        // Resume the game
        gameIsPaused = false;
    }

    // Function that adds a point to spend for the attack attribute
    public void addAtack()
    {
        // Check if you have points to spend
        if (pointsToSpend>0) {
            // Add point to attack
            attackPoints += 1;
            // Remove a point to spend
            pointsToSpend -= 1;
            // Update UI with new stats
            updateIU();
            // Update player stats according to status points
            player.GetComponent<PlayerController>().updateStatusPoints(attackPoints, defensePoints, lifePoints, pointsToSpend);
            // Save player progress stats
            saveStats();
        }
    }

    // Function that adds a point to spend for the defense attribute
    public void addDefense()
    {
        // Check if you have points to spend
        if (pointsToSpend > 0)
        {
            // Add point to defense
            defensePoints += 1;
            // Remove a point to spend
            pointsToSpend -= 1;
            // Update UI with new stats
            updateIU();
            // Update player stats according to status points
            player.GetComponent<PlayerController>().updateStatusPoints(attackPoints, defensePoints, lifePoints, pointsToSpend);
            // Save player progress stats
            saveStats();
        }
    }

    // Function that adds a point to spend for the life attribute
    public void addLife()
    {
        // Check if you have points to spend
        if (pointsToSpend > 0)
        {
            // Add point to life
            lifePoints += 1;
            // Remove a point to spend
            pointsToSpend -= 1;
            // Update UI with new stats
            updateIU();
            // Update player stats according to status points
            player.GetComponent<PlayerController>().updateStatusPoints(attackPoints, defensePoints, lifePoints, pointsToSpend);
            // Save player progress stats
            saveStats();
        }
    }
}
