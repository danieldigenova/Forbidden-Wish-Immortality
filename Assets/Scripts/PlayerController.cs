using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Status Points (Persistent)
    public int statusPointsLife;
    public int statusPointsShield;
    public int statusPointsAttack;
    public int pointsToSpend;
    //Experience Points (Persistent)
    public float playerTotalExperience;
    public int level;
    public int playerExperience;

    //Damage system variables
    public float playerLife;
    public float playerShield;
    public float playerAttack;

    public float mov_speed;
    public float attack_speed;
    public float jump_power;

    //state variables
    public bool isJumping;
    public bool isAttacking = false;

    private float cooldownAttack;

    private Rigidbody2D rb;
    private Animator animator;

    public Transform swordRange;
    public float attackRange;
    public LayerMask enemies;

    public Text LevelText;

    private bool isDead = false;

    void Start()
    {

        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            level = data.level;
            statusPointsLife = data.statusPointsLife;
            statusPointsAttack = data.statusPointsAttack;
            statusPointsShield = data.statusPointsShield;
            pointsToSpend = data.pointsToSpend;
        }
        else
        {
            level = 1;
            statusPointsLife = 0;
            statusPointsAttack = 0;
            statusPointsShield =0;
            pointsToSpend = 0;
        }
        updateStatus();
        updateLevelText();
        playerExperience = 0;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        if(!isDead){
            Move();
            Jump();
            Attack();
        }
        if (playerExperience>=20)
        {
            levelUp();
            pointsToSpend += 5;
            playerExperience = 0;
        }

        // Update life UI
        //life_text.text = "Player Life: " + GameController.instance.PlayerLife;
    }

    void levelUp()
    {
        level += 1;
        updateStatus();
        updateLevelText();
    }

    void updateStatus()
    {
        playerLife = 100 + (0.1f* statusPointsLife);
        playerAttack = 15 + (0.1f * statusPointsAttack);
        playerShield = 110 + (0.1f * statusPointsShield);
    }
    public void updateStatusPoints(int attack, int defense, int life, int pointsToSpend)
    {
        statusPointsLife = life;
        statusPointsAttack= attack;
        statusPointsShield = defense;
        this.pointsToSpend = pointsToSpend;
    }

    void updateLevelText()
    {
        LevelText.text = "Lvl. " + level;
    }

    private void Move()
    {

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * mov_speed;
        if(Input.GetAxis("Horizontal") > 0)
        {
            animator.SetBool("Walk", true);
            transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            animator.SetBool("Walk", true);
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        } 
        else
        {
            animator.SetBool("Walk", false);
        }
        
    }
    private void Jump()
    {
        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector2(0, jump_power), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            animator.SetBool("Jump", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = true;
            animator.SetBool("Jump", true);
        }
    }

    private void Attack()
    {
        if (isAttacking)
        {
            cooldownAttack += Time.deltaTime;
            if (cooldownAttack > 1/attack_speed)
            {
                isAttacking = false;
            }   
        } 
        else
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                int type_attack = Random.Range(1, 4);
                animator.SetTrigger("Attack" + type_attack);

                isAttacking = true;
                cooldownAttack = 0.0f;

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordRange.position, attackRange, enemies);

                foreach(Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<EnemyController>().TakeDamage(playerAttack);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (swordRange == null)
            return;

        Gizmos.DrawWireSphere(swordRange.position, attackRange);
    }


    public void TakeDamage(float damage)
    {
        playerLife -= damage;
        if (playerLife < 0)
            playerLife = 0;

        if (playerLife > 0)
        {
            animator.SetTrigger("TakeDamage");
        }

        else if (playerLife <= damage && !isDead)
        {
            animator.SetBool("Dead", true);
            animator.SetTrigger("TakeDamage");
            isDead = true;
            StartCoroutine(Die());            
        }

    }

    private IEnumerator Die()
    {
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(2);

        GameController.instance.ShowGameOver();
    }

}
