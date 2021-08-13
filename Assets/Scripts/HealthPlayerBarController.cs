using UnityEngine;
using UnityEngine.UI;

/**
 * Script to control the player's life bar
 */
public class HealthPlayerBarController : MonoBehaviour
{
    // GameObject with full health bar
    public GameObject healthBarComplete;

    // Current value fill of health bar
    private float currfillValue;

    // Player GameObject
    GameObject player;

    // Player current life Text 
    public Text LifeText;

    // Start is called before the first frame update
    private void Start()
    {
        // Find the GameObject of the Player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Fills the bar according to the player's current health in relation to his maximum health
        currfillValue = player.GetComponent<PlayerController>().playerLife / player.GetComponent<PlayerController>().playerMaxLife;
        healthBarComplete.GetComponent<Image>().fillAmount = currfillValue;
        // Show the value of Player current life
        LifeText.text = (int)player.GetComponent<PlayerController>().playerLife + "/" + player.GetComponent<PlayerController>().playerMaxLife;
    }
}