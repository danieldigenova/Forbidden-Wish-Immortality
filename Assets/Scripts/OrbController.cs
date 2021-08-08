using UnityEngine;

/*
 * Script to control orb collection and experience gain
 **/
public class OrbController : MonoBehaviour
{
    GameObject player;
    GameObject gameController;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject of the Player
        player = GameObject.FindGameObjectWithTag("Player");

        // Find the GameController
        gameController = GameObject.Find("GameController");
    }

    // Function to collect orbs when the player touches them
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            // Increases player experience
            //player.GetComponent<PlayerController>().playerExperience += 20;
            player.GetComponent<PlayerController>().exp += gameController.GetComponent<GameController>().level * 2 ;
            // Destroy the orbs when collecting
            Destroy(gameObject);
        }
    }
}
