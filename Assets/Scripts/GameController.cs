using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject gameOver;
    public static GameController instance;


    public int PlayerTotalLife;
    public int PlayerTotalShield;
    public int PlayerTotalAttack;
    public int PlayerTotalExperience;

    public int PlayerLife;
    public int PlayerShield;
    public int PlayerAttack;
    public int PlayerExperience;

    void Start()
    {
        instance = this;
        this.PlayerLife = this.PlayerTotalLife;
        this.PlayerShield = this.PlayerTotalShield;
        this.PlayerAttack = this.PlayerTotalAttack;
    }

    // Update is called once per frame
    void Update()
    {
        
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
