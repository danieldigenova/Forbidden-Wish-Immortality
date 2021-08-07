using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Enemy's current state types
public enum EnemyState
{
    Idle,
    Follow,
    Die,
    Attack
};


/**
 * Script for controlling an AI for melee enemies
 */
public class EnemyController : MonoBehaviour
{
    // Base Stats
    public float mov_speed;
    public float vision_range;
    public float attack_range;
    public float enemyAttack;
    public float enemyLife;

    // Attack Cooldown
    public float cooldownAttack;
    private float timer_attack = 0;

    // Enemy Level
    public int enemyLevel;
    public Text levelText;

    // Enemy animator
    public Animator animator;

    // Player GameObjects
    public GameObject player;

    // Midpoint of melee attack range
    public Transform meleeRange;

    // Player layer
    public LayerMask players;

    // Enemy current state
    public EnemyState currState = EnemyState.Idle;

    // Prefabs of the Orbs of experience
    public GameObject orbGreenPrefab;
    public GameObject orbBluePrefab;

    // Health bar
    public Slider healthbar;
    public Gradient healthBarCollor;
    public Image healthBarFill;

    // Start is called before the first frame update
    void Start()
    {
        // Get and show the enemy's level
        enemyLevel = GameController.instance.level;
        levelText.text = "lvl. " + enemyLevel;

        // Set the attributes of enemies according to their level.
        this.enemyLife = (100 + 0.5f * enemyLevel);
        this.enemyAttack = 10 + enemyLevel;
        setMaxHealth(enemyLife);

        // Find the GameObject of the Player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if there is a player in the scene. If not, it is in idle state
        if (player.GetComponent<PlayerController>().isDead)
        {
            currState = EnemyState.Idle;
        }

        // Perform an action depending on the enemy's current state
        switch (currState)
        {
            case (EnemyState.Idle):
                CheckPlayer();
                break;
            case (EnemyState.Follow):
                Follow();
                break;
            case (EnemyState.Die):
                break;
            case (EnemyState.Attack):
                Attack();
                break;
        }
    }

    // Function to see if there is a player in the enemy's view range
    public void CheckPlayer()
    {
        // If there is a player in the enemy's sight range, then the enemy goes to Follow state.
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= vision_range)
        {
            // Set the current state to dead
            currState = EnemyState.Follow;
        }
    }

    // Function that deals damage to the enemy according to a value
    public void TakeDamage(float damage)
    {
        // Reduces enemy's health with damage value
        enemyLife -= damage;

        // Sets life at 0 if below that
        if (enemyLife < 0)
            enemyLife = 0;

        // If the hit wasn't fatal, then it reduces the enemy's health and performs the hurt animation
        if (enemyLife > 0)
        {
            // Play the hurt animation
            animator.SetTrigger("TakeDamage");
            // Set new enemy life in health bar
            setHealth(enemyLife);
        }
        // If the hit was fatal, performs a hurt animation, followed by a death animation, and puts it in the die state. After that, call the showOrbs() function.
        else if (enemyLife <= damage && currState != EnemyState.Die)
        {
            // Set new enemy life in health bar
            setHealth(enemyLife);
            // Play the hurt and death animations
            animator.SetBool("Dead", true);
            animator.SetTrigger("TakeDamage");
            // Set the current state to die
            currState = EnemyState.Die;
            // Call the function that shows the orbs
            StartCoroutine(showOrbs());
        }

    }

    // Function that generates experience orbs and destroys the enemy's gameObject
    private IEnumerator showOrbs()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2);

        // Show the experience orbs
        orbGreenPrefab.SetActive(true);
        GameObject orbGreen = Instantiate<GameObject>(orbGreenPrefab);
        orbGreen.transform.position = new Vector3(gameObject.transform.position.x + 0.2f, gameObject.transform.position.y - 0.45f, 0);

        orbBluePrefab.SetActive(true);
        GameObject orbBlue = Instantiate<GameObject>(orbBluePrefab);
        orbBlue.transform.position = new Vector3(gameObject.transform.position.x - 0.2f, gameObject.transform.position.y - 0.45f, 0);

        // Destroys the enemy's gameObject
        Destroy(gameObject);

        yield return null;
    }

    // Function that makes the enemy follow the player
    private void Follow()
    {
        // If the player is in attack range, it changes to the current state for attack
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= attack_range + 0.5f)
        {
            // Set the current state to attack
            currState = EnemyState.Attack;
            // Returns to idle animation
            animator.SetBool("Walk", false);
        }
        // If the player is out of sight, it switches to idle state
        else if (player != null && Vector3.Distance(transform.position, player.transform.position) > vision_range)
        {
            // Set the current state to idle
            currState = EnemyState.Idle;
            // Returns to idle animation
            animator.SetBool("Walk", false);
        }
        // If the player is in vision range then performs the walk animation and follows the player
        else
        {
            // Play walk animation
            animator.SetBool("Walk", true);
            // Moves the enemy towards the player according to their movement speed
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, 0), mov_speed * Time.deltaTime);
            if (transform.position.x >= player.transform.position.x)
            {
                // Turn the angle to face the player
                transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
            else
            {
                // Turn the angle to face the player
                transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }
        }

    }

    // Function that makes the enemy attack the player if he is in his attack range
    private void Attack()
    {
        // If the player is too far away then it switches to the follow state.
        if (player != null && Vector3.Distance(transform.position, player.transform.position) > attack_range + 0.5f)
        {
            // Set the current state to follow
            currState = EnemyState.Follow;
        }

        // Increases attack cooldown timer whenever in this state
        timer_attack += Time.deltaTime;

        // Performs an attack if the timer has passed the attack cooldown
        if (cooldownAttack < timer_attack)
        {
            // Play the attack animation
            animator.SetTrigger("Attack");

            // Reset timer
            timer_attack = 0.0f;

            // Checks all affected players in enemy's circle of attack
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(meleeRange.position, attack_range, players);

            // Damages all players affected by the attack
            foreach (Collider2D play in hitPlayers)
            {
                play.GetComponent<PlayerController>().TakeDamage(enemyAttack);
            }
        }
    }

    // Draw a sphere around the midpoint of melee attack range
    private void OnDrawGizmosSelected()
    {
        // Checks if the point is set
        if (meleeRange == null)
            return;

        // Draw the sphere in midpoint of melee attack range, with radius according to attack range
        Gizmos.DrawWireSphere(meleeRange.position, attack_range);
    }

    // Set the health bar fills according to maxHealth
    void setMaxHealth(float maxHealth)
    {
        // Set the health bar value to maxHealth
        healthbar.maxValue = maxHealth;
        healthbar.value = maxHealth;

        // Set color to green
        healthBarFill.color = healthBarCollor.Evaluate(1f);
    }

    //Set the health bar fills to health value
    void setHealth(float health)
    {
        // Set the health bar value to health
        healthbar.value = health;
        // Set bar fills according to health
        healthBarFill.color = healthBarCollor.Evaluate(healthbar.normalizedValue);
    }

}
