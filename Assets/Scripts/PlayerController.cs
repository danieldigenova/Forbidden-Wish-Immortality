using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
 * Script to control the player
 **/

public class PlayerController : MonoBehaviour
{
    // Status Points (Persistent)
    public int statusPointsLife;
    public int statusPointsShield;
    public int statusPointsAttack;
    public int pointsToSpend;

    // Experience Points (Persistent)
    //public float playerTotalExperience;
    //public int playerExperience;

    // Current level
    public int level;
    public Text LevelText;

    // Current EXP
    public int exp;
    public int expToLevelUp;

    // Current Life
    public float playerLife;

    // Base Stats
    public float playerMaxLife;
    public float playerDefense;
    public float playerAttack;
    public float mov_speed;
    public float attack_speed;
    public float jump_power;
    public float attackRange;

    // State variables
    public bool isJumping = false;
    public bool isAttacking = false;
    public bool isDead = false;

    // Attack Cooldown
    private float cooldownAttack;

    // Player Rigidbody2D
    private Rigidbody2D rb;

    // Player Animator
    private Animator animator;

    // Midpoint of sword attack range
    public Transform swordRange;

    // Enemies layer
    public LayerMask enemies;

    //Sound
    public AudioSource runSource;
    public AudioClip[] runClips;

    public AudioSource jumpSource;
    public AudioClip[] jumpClips;

    public AudioSource swordSource;
    public AudioClip[] swordClips;

    public int flagActiveSound;

    // Start is called before the first frame update
    void Start()
    {
        // Load the player that was saved
        PlayerData data = SaveSystem.LoadPlayer();
        // If there is no saved player, then create a date with your information
        if (data != null)
        {
            level = data.level;
            exp = data.exp;
            playerLife = data.playerLife;
            expToLevelUp = data.expToLevelUp;
            statusPointsLife = data.statusPointsLife;
            statusPointsAttack = data.statusPointsAttack;
            statusPointsShield = data.statusPointsShield;
            pointsToSpend = data.pointsToSpend;

            // if the player has low life, he recovers half his life
            if (playerLife < (100 + (1f * statusPointsLife)) / 2)
            {
                recoverLife((100 + (1f * statusPointsLife)) / 2 - playerLife);
            }
        }
        else
        {
            level = 1;
            exp = 0;
            playerLife = 100;
            playerMaxLife = 100;
            expToLevelUp = 2;
            statusPointsLife = 0;
            statusPointsAttack = 0;
            statusPointsShield = 0;
            pointsToSpend = 0;
        }

        // Update player stats according to attribute points
        updateStatus();
        
        // Update Level Text to current level
        updateLevelText();

        // Set initial experience
        //playerExperience = 0;

        // Get Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Get animator component
        animator = GetComponent<Animator>(); 

        flagActiveSound = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is not dead, check if any action keys were pressed
        if (!isDead){
            Move();
            Jump();
            Attack();
        }

        // Check if the experience has reached the mark for level up
        if(exp >= expToLevelUp)
        {
            // Call the function to level up
            levelUp();
            // Give points to spend
            pointsToSpend += 5;
            // Reset current experience
            exp = 0;
        }

        // Update player stats according to attribute points
        updateStatus();
    }

    // Function for level up
    void levelUp()
    {
        // Increase the level
        level += 1;

        // Reset current EXP
        exp = 0;

        // Set new EXP to Level Up
        // Function based on the experience curve of Dungeons and Dragons
        expToLevelUp = ((5 * ((int) Mathf.Pow(level, 2))) - (5 * level));

        // Recover player life
        playerLife = playerMaxLife;

        // Update Level Text to current level
        updateLevelText();
    }

    // Function to update player stats according to status points
    void updateStatus()
    {
        playerMaxLife = 100 + (1f* statusPointsLife);
        playerAttack = 15 + (0.5f * statusPointsAttack);
        playerDefense = 10 + (0.5f * statusPointsShield);
    }

    public void recoverLife(float value) {
        playerLife += value;
    }

    // Function to update the current status points of each attribute
    public void updateStatusPoints(int attack, int defense, int life, int pointsToSpend)
    {
        statusPointsLife = life;
        statusPointsAttack= attack;
        statusPointsShield = defense;
        this.pointsToSpend = pointsToSpend;
    }

    // Fuction to update Level Text
    void updateLevelText()
    {
        LevelText.text = "Lvl. " + level;
    }

    // Function that moves the player
    private void Move()
    {
        // Moves the player according to horizontal key press
        // The movement speed is according to the movement speed attribute.
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * mov_speed;

        // Move right
        if (Input.GetAxis("Horizontal") > 0)
        {
            // Play walk animation
            animator.SetBool("Walk", true);
            // Turn to the right side
            transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

            flagActiveSound += 1;

            if(!isJumping && flagActiveSound % 6 == 0){
                int randomIndex = Random.Range(0, runClips.Length);
                runSource.PlayOneShot(runClips[randomIndex]);
            }
        }
        // Move left
        else if (Input.GetAxis("Horizontal") < 0)
        {
            // Play walk animation
            animator.SetBool("Walk", true);
            // Turn to the left side
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);

            flagActiveSound += 1;

            if(!isJumping && flagActiveSound % 6 == 0){
                int randomIndex = Random.Range(0, runClips.Length);
                runSource.PlayOneShot(runClips[randomIndex]);
            }
            
        }
        // Stopped
        else
        {
            // Return to idle animation
            animator.SetBool("Walk", false);
        }
        
    }

    // Function for the player to jump
    private void Jump()
    {
        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            // Exerts a force on the player according to the jump force
            rb.AddForce(new Vector2(0, jump_power), ForceMode2D.Impulse);

            int randomIndex = Random.Range(0, jumpClips.Length);
            jumpSource.PlayOneShot(jumpClips[randomIndex]);
        }
    }

    // Function to check if the player is on the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is on the ground
        if (collision.gameObject.tag == "Ground")
        {
            // Return to idle animation
            isJumping = false;
            animator.SetBool("Jump", false);
        }

        if(collision.gameObject.tag == "Enemy")
        {
            if(transform.eulerAngles == new Vector3(0.0f, 180.0f, 0.0f))
            {
                // Repels touching an enemy
                rb.AddForce(new Vector2(-1, 1), ForceMode2D.Impulse);
            } 
            else
            {
                // Repels touching an enemy
                rb.AddForce(new Vector2(1, 1), ForceMode2D.Impulse);
            }
            
        }
    }

    // Function to check if the player is jumping
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player is on the ground
        if (collision.gameObject.tag == "Ground")
        {
            // Play jump animation
            isJumping = true;
            animator.SetBool("Jump", true);
        }
    }

    // Player attack function
    private void Attack()
    {
        // Check if it is in attack cooldown
        if (isAttacking)
        {
            // Check if the attack cooldown has already been exceeded to allow another attack
            cooldownAttack += Time.deltaTime;
            if (cooldownAttack > 1/attack_speed)
            {
                isAttacking = false;
            }   
        }
        // If he is not on attack timeout, he can make a new attack
        else
        {
            // If the Z key is pressed, performs an attack
            if (Input.GetKeyDown(KeyCode.Z))
            {
                int randomIndex = Random.Range(0, swordClips.Length);
                swordSource.PlayOneShot(swordClips[randomIndex]);

                // Draw one of the 3 attack animations to animate
                int type_attack = Random.Range(1, 4);
                animator.SetTrigger("Attack" + type_attack);

                // Enter attack cooldown
                isAttacking = true;
                cooldownAttack = 0.0f;

                // Checks all affected enemies in player's circle of attack
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordRange.position, attackRange, enemies);

                // Damages all enemies affected by the attack
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<EnemyController>().TakeDamage(playerAttack);
                }
            }
        }
    }

    // Draw a sphere around the midpoint of sword attack range
    private void OnDrawGizmosSelected()
    {
        // Checks if the point is set
        if (swordRange == null)
            return;

        // Draw the sphere in midpoint of sword attack range, with radius according to attack range
        Gizmos.DrawWireSphere(swordRange.position, attackRange);
    }

    // Function that deals damage to the player according to a value
    public void TakeDamage(float damage)
    {
        // Reduces player's health with damage value
        //playerLife -= damage;
        playerLife = Mathf.Clamp(playerLife + playerDefense - damage, 0, playerLife);

        // Sets life at 0 if below that
        //if (playerLife < 0)
            //playerLife = 0;

        // If the hit wasn't fatal, then it reduces the player's health and performs the hurt animation
        if (playerLife > 0)
        {
            // Play the hurt animation
            animator.SetTrigger("TakeDamage");
        }
        // If the hit was fatal, performs a hurt animation, followed by a death animation, and puts it in the die state. After that, call the showGameOvers() function.
        else if (playerLife <= damage && !isDead)
        {
            // Play the hurt and death animations
            animator.SetBool("Dead", true);
            animator.SetTrigger("TakeDamage");
            // Set the current state to die
            isDead = true;
            StartCoroutine(showGameOver());            
        }

    }

    // Function to show Game Over UI
    private IEnumerator showGameOver()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2);

        // Show Game Over UI
        GameController.instance.ShowGameOver();
    }

}
