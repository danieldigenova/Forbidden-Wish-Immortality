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
    //private int life = 100;

    //public Text life_text;

    public Animator animator;
    public GameObject player;
    public float mov_speed;
    public float vision_range;
    public float attack_range;
    public float cooldownAttack;
    private float timer_attack = 0;

    public int EnemyTotalLife;
    public int EnemyTotalAttack;

    public int EnemyAttack;
    public int enemyLife;

    public Transform swordRange;

    public LayerMask players;

    public EnemyState currState = EnemyState.Idle;

    public GameObject orbGreenPrefab;
    public GameObject orbBluePrefab;
    public Slider healthbar;
    public Gradient healthBarCollor;
    public Image healthBarFill;

    void Start()
    {
        this.enemyLife = this.EnemyTotalLife;
        this.EnemyAttack = this.EnemyTotalAttack;
        setMaxHealth(EnemyTotalLife);
    }

    void Update()
    {
        // Update life UI
        //life_text.text = "Enemy Life: " + life;

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
        enemyLife = Mathf.Clamp(enemyLife - damage, 0, 100);
        setHealth(enemyLife);
        if (enemyLife > 0)
        {
            animator.SetTrigger("TakeDamage");
        }       

        else if (enemyLife <= damage && currState != EnemyState.Die)
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

        orbGreenPrefab.SetActive(true);
        GameObject orbGreen = Instantiate<GameObject>(orbGreenPrefab);
        orbGreen.transform.position = new Vector3(gameObject.transform.position.x + 0.2f, gameObject.transform.position.y - 0.45f, 0); //gameObject.transform.position;

        orbBluePrefab.SetActive(true);
        GameObject orbBlue = Instantiate<GameObject>(orbBluePrefab);
        orbBlue.transform.position = new Vector3(gameObject.transform.position.x - 0.2f, gameObject.transform.position.y - 0.45f, 0); //gameObject.transform.position;

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
                play.GetComponent<PlayerController>().TakeDamage(EnemyAttack);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (swordRange == null)
            return;

        Gizmos.DrawWireSphere(swordRange.position, attack_range);
    }

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
