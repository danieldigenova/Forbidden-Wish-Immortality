using UnityEngine;
using UnityEngine.UI;

public class HealthEnemyBarController : MonoBehaviour
{
    public GameObject heartBarComplete;

    private float currfillValue;

    void Update()
    {
        currfillValue = (float)GameController.instance.EnemyLife / GameController.instance.EnemyTotalLife;
        heartBarComplete.GetComponent<Image>().fillAmount = currfillValue;
    }
}
