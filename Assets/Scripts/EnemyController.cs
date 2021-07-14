using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Idle,
    Follow,
    Die,
    Attack
};

public class EnemyController : MonoBehaviour
{
    private int life = 100;

    public Text life_text;

    public Animator animator;
    public GameObject player;
    public float mov_speed;
    public float vision_range;
    public float attack_range;
    public int attack_damage;
    public float cooldownAttack;
    private float timer_attack = 0;

    public Transform swordRange;

    public LayerMask players;

    public EnemyState currState = EnemyState.Idle;

    void Start()
    {

    }

    void Update()
    {
        // Update life UI
        life_text.text = "Enemy Life: " + life;

        if (player == null)
        {
            currState = EnemyState.Idle;
        }

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

    public void CheckPlayer()
    {
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= vision_range)
        {
            currState = EnemyState.Follow;
        }
    }

    public void TakeDamage(int damage)
    {
        life = Mathf.Clamp(life - damage, 0, 100);
        if (life > 0)
        {
            animator.SetTrigger("TakeDamage");
        }       

        else if (life <= damage && currState != EnemyState.Die)
        {
            animator.SetBool("Dead", true);
            animator.SetTrigger("TakeDamage");
            currState = EnemyState.Die;
            StartCoroutine(Die());
        }

    }
    private IEnumerator Die()
    {
        //animator.SetBool("Dead", true);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        yield return null;
        
    }

    private void Follow()
    {
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= attack_range + 0.5f)
        {
            currState = EnemyState.Attack;
            animator.SetBool("Walk", false);
        }
        else if (player != null && Vector3.Distance(transform.position, player.transform.position) > vision_range)
        {
            currState = EnemyState.Idle;
            animator.SetBool("Walk", false);
        }
        else
        {
            animator.SetBool("Walk", true);
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, 0), mov_speed * Time.deltaTime);
            if(transform.position.x >= player.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            } 
            else
            {
                transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }
        }
        
    }

    private void Attack()
    {
        if (player != null && Vector3.Distance(transform.position, player.transform.position) > attack_range + 0.5f)
        {
            currState = EnemyState.Follow;
        }

        timer_attack += Time.deltaTime;

        if (cooldownAttack < timer_attack)
        {
            animator.SetTrigger("Attack");

            timer_attack = 0.0f;

            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(swordRange.position, attack_range, players);

            foreach (Collider2D play in hitPlayers)
            {
                play.GetComponent<PlayerController>().TakeDamage(attack_damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (swordRange == null)
            return;

        Gizmos.DrawWireSphere(swordRange.position, attack_range);
    }
}
