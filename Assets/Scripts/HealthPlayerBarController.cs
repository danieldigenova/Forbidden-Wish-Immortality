using UnityEngine;
using UnityEngine.UI;

public class HealthPlayerBarController : MonoBehaviour
{
	public GameObject heartBarComplete;
	
	private float currfillValue;

    GameObject[] players;
    GameObject player;

    public Text LifeText;

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        player = players[0];
    }

    void Update()
    {
        currfillValue = player.GetComponent<PlayerController>().playerLife / (100 + (0.1f * player.GetComponent<PlayerController>().statusPointsLife));
        heartBarComplete.GetComponent<Image>().fillAmount = currfillValue;
        LifeText.text = player.GetComponent<PlayerController>().playerLife + "/" + (100 + (0.1f * player.GetComponent<PlayerController>().statusPointsLife));
    }
}