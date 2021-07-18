using UnityEngine;
using UnityEngine.UI;

public class HealthEnemyBarController : MonoBehaviour
{

    public Slider healthbar;
    public Gradient healthBarCollor;
    public Image healthBarFill;
    
    void setMaxHealth(float maxHealth)
    {
        healthbar.maxValue = maxHealth;
        healthbar.value = maxHealth;

        healthBarFill.color = healthBarCollor.Evaluate(1f);
    }

    void setHealth(float health)
    {
        healthbar.value = health;

        healthBarFill.color = healthBarCollor.Evaluate(healthbar.normalizedValue);
    }
}
