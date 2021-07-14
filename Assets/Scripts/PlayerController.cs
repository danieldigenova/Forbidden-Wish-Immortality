using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private int life = 100;
    public float mov_speed;
    public float attack_speed;
    public float jump_power;
    public int attack_damage;

    public Text life_text;

    public bool isJumping;
    public bool isAttacking = false;

    private float cooldownAttack;

    private Rigidbody2D rb;
    private Animator animator;

    public Transform swordRange;
    public float attackRange;
    public LayerMask enemies;

    private bool isDead = false;

    void Start()
    {
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

        // Update life UI
        life_text.text = "Player Life: " + life;
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
                    enemy.GetComponent<EnemyController>().TakeDamage(attack_damage);
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


    public void TakeDamage(int damage)
    {
        life = Mathf.Clamp(life - damage, 0, 100);
        if (life > 0)
        {
            animator.SetTrigger("TakeDamage");
        }

        else if (life <= damage && !isDead)
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
        Destroy(gameObject);

    }

}
