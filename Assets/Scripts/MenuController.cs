using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuController : MonoBehaviour
{
    public Button buttonContinuar;

    // Start is called before the first frame update
    void Start()
    {
        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            buttonContinuar.gameObject.SetActive(true);
        }
        else
        {
            buttonContinuar.gameObject.SetActive(false);
        }
    }

    // Function to go to the game scene
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Function to go to the new game scene
    public void NewGame()
    {
        string path = Application.persistentDataPath + "/player.fun";

        File.Delete(path);

        path = Application.persistentDataPath + "/game.fun";

        File.Delete(path);

        SceneManager.LoadScene("Game");
    }

    // Quit Game Function
    public void QuitGame()
    {
        Application.Quit();
    }
}
