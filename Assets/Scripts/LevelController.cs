using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public GameObject player;
    public GameObject rightWall;
    public GameObject leftWall;
    public GameObject Enemy;
    private GameObject enemyInstance;
    public Text levelShow;

    int level;

    private void Start()
    {
        level = 1;
        enemyInstance=Instantiate(Enemy);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyInstance == null)
        {
            if (player.GetComponent<Transform>().position.x>3)
            {
                nextLevel();
                enemyInstance=Instantiate(Enemy);
            }
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
}
