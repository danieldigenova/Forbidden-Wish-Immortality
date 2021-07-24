using UnityEngine;

/*
 * Script to control orb collection and experience gain
 **/
public class OrbController : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject of the Player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Function to collect orbs when the player touches them
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            // Increases player experience
            player.GetComponent<PlayerController>().playerExperience += 20;
            // Destroy the orbs when collecting
            Destroy(gameObject);
        }
    }
}
