using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Button attackSpeedPlus;
    public Button movSpeedPlus;
    public Button lifePlus;
    public Button defensePlus;
    public Button luckPlus;

    // Stats Texts
    public Text attackText;
    public Text defenseText;
    public Text attackSpeedText;
    public Text movSpeedText;
    public Text lifeText;
    public Text luckText;
    public Text PointsText;

    // Stats Points
    public int attackPoints;
    public int defensePoints;
    public int attackSpeedPoints;
    public int movSpeedPoints;
    public int luckPoints;
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
        attackSpeedPoints = player.GetComponent<PlayerController>().statusPointsAttackSpeed;
        movSpeedPoints = player.GetComponent<PlayerController>().statusPointsMovSpeed;
        lifePoints = player.GetComponent<PlayerController>().statusPointsLife;
        luckPoints = player.GetComponent<PlayerController>().statusPointsLuck;
        pointsToSpend = player.GetComponent<PlayerController>().pointsToSpend;
    }

    // Function that updates UI texts with stats points
    void updateIU()
    {
        attackText.text = "Attack Points: " + attackPoints;
        defenseText.text = "Defense Points: " + defensePoints;
        lifeText.text = "Life Points: " + lifePoints;
        PointsText.text = "Total Points: " + pointsToSpend;

        if (luckPoints >= 200)
        {
            luckText.text = "Luck Points: MAX";
        }
        else
        {
            luckText.text = "Luck Points: " + luckPoints;
        }

        if (movSpeedPoints >= 200)
        {
            movSpeedText.text = "Mov Speed Points: MAX";
        }
        else
        {
            movSpeedText.text = "Mov Speed Points: " + movSpeedPoints;
        }

        if (attackSpeedPoints >= 200)
        {
            attackSpeedText.text = "Attack Speed Points: MAX";
        }
        else
        {
            attackSpeedText.text = "Attack Speed Points: " + attackSpeedPoints;
        }
    }

    // Function to save player progress stats
    void saveStats()
    {
        SaveSystem.SavePlayer(player.GetComponent<PlayerController>());
    }

    // Function to resume the game
    public void Resume()
    {
        // No longer show the Pause menu UI
        pauseMenuIU.SetActive(false);
        // Leave the scale that time passes at one
        Time.timeScale = 1f;
        // Resume the game
        gameIsPaused = false;
    }

    // Quit Game Function
    public void QuitGame()
    {
        Application.Quit();
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
            player.GetComponent<PlayerController>().updateStatusPoints(attackPoints, defensePoints, attackSpeedPoints, movSpeedPoints, lifePoints, pointsToSpend, luckPoints);
            // Save player progress stats
            saveStats();
        }
    }

    public void addAtackSpeed()
    {
        // Check if you have points to spend
        if (pointsToSpend > 0 && attackSpeedPoints<200)
        {
            // Add point to attack
            attackSpeedPoints += 1;
            // Remove a point to spend
            pointsToSpend -= 1;
            // Update UI with new stats
            updateIU();
            // Update player stats according to status points
            player.GetComponent<PlayerController>().updateStatusPoints(attackPoints, defensePoints, attackSpeedPoints, movSpeedPoints, lifePoints, pointsToSpend, luckPoints);
            // Save player progress stats
            saveStats();
        }
    }

    public void addMovSpeed()
    {
        // Check if you have points to spend
        if (pointsToSpend > 0 && movSpeedPoints<200)
        {
            // Add point to attack
            movSpeedPoints += 1;
            // Remove a point to spend
            pointsToSpend -= 1;
            // Update UI with new stats
            updateIU();
            // Update player stats according to status points
            player.GetComponent<PlayerController>().updateStatusPoints(attackPoints, defensePoints, attackSpeedPoints, movSpeedPoints, lifePoints, pointsToSpend, luckPoints);
            // Save player progress stats
            saveStats();
        }
    }
    public void addLuck()
    {
        // Check if you have points to spend
        if (pointsToSpend > 0 && luckPoints < 200)
        {
            // Add point to attack
            luckPoints += 1;
            // Remove a point to spend
            pointsToSpend -= 1;
            // Update UI with new stats
            updateIU();
            // Update player stats according to status points
            player.GetComponent<PlayerController>().updateStatusPoints(attackPoints, defensePoints, attackSpeedPoints, movSpeedPoints, lifePoints, pointsToSpend, luckPoints);
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
            player.GetComponent<PlayerController>().updateStatusPoints(attackPoints, defensePoints, attackSpeedPoints, movSpeedPoints, lifePoints, pointsToSpend, luckPoints);
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
            player.GetComponent<PlayerController>().updateStatusPoints(attackPoints, defensePoints, attackSpeedPoints, movSpeedPoints, lifePoints, pointsToSpend, luckPoints);
            player.GetComponent<PlayerController>().recoverLife(1);
            // Save player progress stats
            saveStats();
        }
    }

    public void returnMenu()
    {
        SceneManager.LoadScene("MenuInicial");
    }
}
