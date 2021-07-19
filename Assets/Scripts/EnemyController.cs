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
    //Base Stats

    public int enemyLevel;
    public Text levelText;

    public Animator animator;
    public GameObject[] player;
    public float mov_speed;
    public float vision_range;
    public float attack_range;
    public float cooldownAttack;
    private float timer_attack = 0;

    public float EnemyAttack;
    public float enemyLife;

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
        enemyLevel = GameController.instance.level;
        levelText.text = "lvl. " + enemyLevel;

        this.enemyLife = (100+ 0.5f*enemyLevel);
        this.EnemyAttack = 10 + enemyLevel;
        setMaxHealth(enemyLife);

        player = GameObject.FindGameObjectsWithTag("Player");
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
        if (player[0] != null && Vector3.Distance(transform.position, player[0].transform.position) <= vision_range)
        {
            currState = EnemyState.Follow;
        }
    }

    public void TakeDamage(float damage)
    {
        enemyLife -=damage;

        if (enemyLife < 0)
            enemyLife = 0;

        if (enemyLife > 0)
        {
            animator.SetTrigger("TakeDamage");
            setHealth(enemyLife);
        }       

        else if (enemyLife <= damage && currState != EnemyState.Die)
        {
            setHealth(enemyLife);
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
        if (player[0] != null && Vector3.Distance(transform.position, player[0].transform.position) <= attack_range + 0.5f)
        {
            currState = EnemyState.Attack;
            animator.SetBool("Walk", false);
        }
        else if (player != null && Vector3.Distance(transform.position, player[0].transform.position) > vision_range)
        {
            currState = EnemyState.Idle;
            animator.SetBool("Walk", false);
        }
        else
        {
            animator.SetBool("Walk", true);
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(player[0].transform.position.x, transform.position.y, 0), mov_speed * Time.deltaTime);
            if(transform.position.x >= player[0].transform.position.x)
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
        if (player[0] != null && Vector3.Distance(transform.position, player[0].transform.position) > attack_range + 0.5f)
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
