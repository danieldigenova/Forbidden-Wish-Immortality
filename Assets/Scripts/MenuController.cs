using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

/*
 * Menu controller script
 **/
public class MenuController : MonoBehaviour
{
    public Button buttonContinuar;

    // Start is called before the first frame update
    void Start()
    {
        // Load saved game file
        GameData data = SaveSystem.LoadGame();

        // If there is no saved file then it does not display the continue button
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
    // Used for when there is a saved file
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Function to go to the new game scene
    public void NewGame()
    {
        // Delete saved files to start a new game
        string path = Application.persistentDataPath + "/player.fun";
        File.Delete(path);
        path = Application.persistentDataPath + "/game.fun";
        File.Delete(path);

        // Go to the game scene
        SceneManager.LoadScene("Game");
    }

    // Quit Game Function
    public void QuitGame()
    {
        Application.Quit();
    }
}
