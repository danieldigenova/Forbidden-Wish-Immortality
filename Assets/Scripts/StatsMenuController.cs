using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenuController : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuIU;

    public GameObject player;
    public Button attackPlus;
    public Button lifePlus;
    public Button defensePlus;

    public Text attackText;
    public Text defenseText;
    public Text lifeText;
    public Text PointsText;


    public int attackPoints;
    public int defensePoints;
    public int lifePoints;
    public int pointsToSpend;

    void Update()
    {
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

    void Pause()
    {
        LoadStats();
        updateIU();
        pauseMenuIU.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    void LoadStats()
    {
        attackPoints = player.GetComponent<PlayerController>().statusPointsAttack;
        defensePoints = player.GetComponent<PlayerController>().statusPointsShield;
        lifePoints = player.GetComponent<PlayerController>().statusPointsLife;
        pointsToSpend = player.GetComponent<PlayerController>().pointsToSpend;
    }

    void updateIU()
    {
        attackText.text = "Atack Points: " + attackPoints;
        defenseText.text = "Defense Points: " + defensePoints;
        lifeText.text = "Life Points: " + lifePoints;
        PointsText.text = "Total Points: " + pointsToSpend;
    }

    void saveStats()
    {
        SaveSystem.SavePlayer(player.GetComponent<PlayerController>());
    }

    void Resume()
    {
        pauseMenuIU.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void addAtack()
    {
        if (pointsToSpend>0) {
            attackPoints += 1;
            pointsToSpend -= 1;
            updateIU();
            player.GetComponent<PlayerController>().updateStatusPoints(attackPoints, defensePoints, lifePoints, pointsToSpend);
            saveStats();
        }
    }
    public void addDefense()
    {
        if (pointsToSpend > 0)
        {
            defensePoints += 1;
            pointsToSpend -= 1;
            updateIU();
            player.GetComponent<PlayerController>().updateStatusPoints(attackPoints, defensePoints, lifePoints, pointsToSpend);
            saveStats();
        }
    }
    public void addLife()
    {
        if (pointsToSpend > 0)
        {
            lifePoints += 1;
            pointsToSpend -= 1;
            updateIU();
            player.GetComponent<PlayerController>().updateStatusPoints(attackPoints, defensePoints, lifePoints, pointsToSpend);
            saveStats();
        }
    }
}
