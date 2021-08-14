using UnityEngine;
using UnityEngine.UI;

public class EXPPlayerBarController : MonoBehaviour
{
    // GameObject with full EXP bar
    public GameObject expBarComplete;

    // Current value fill of EXP bar
    private float currfillValue;

    // Player GameObject
    GameObject player;

    // Player current EXP Text 
    public Text EXPText;

    // Start is called before the first frame update
    private void Start()
    {
        // Find the GameObject of the Player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Fills the bar according to the player's current EXP in relation to his EXP to level up
        currfillValue = player.GetComponent<PlayerController>().exp / (float)(player.GetComponent<PlayerController>().expToLevelUp);
        expBarComplete.GetComponent<Image>().fillAmount = currfillValue;
        // Show the value of player current EXP
        EXPText.text = player.GetComponent<PlayerController>().exp + "/" + player.GetComponent<PlayerController>().expToLevelUp;
    }
}
