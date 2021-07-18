using UnityEngine;
using UnityEngine.UI;

public class HealthPlayerBarController : MonoBehaviour
{
	public GameObject heartBarComplete;
	
	private float currfillValue;

    void Update()
    {
        currfillValue = (float)GameController.instance.PlayerLife / GameController.instance.PlayerTotalLife;
        heartBarComplete.GetComponent<Image>().fillAmount = currfillValue;
    }
}