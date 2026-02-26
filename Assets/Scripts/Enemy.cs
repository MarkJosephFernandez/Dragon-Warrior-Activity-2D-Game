using JetBrains.Annotations;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float jumpForce = 10f;

    public float fireRange = 4f;
    public float fireRate = 2f;

    public GameObject projectilePrefab;
    public Transform firePoint;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundRadius = 0.2f;
    public int health = 100;
    Transform player;
    float playerDistance;
    float fireTimer;
    Rigidbody2D rb;
    bool isGrounded;
    Animator anim;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        playerDistance = player.position.x - transform.position.x;
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundRadius,groundLayer);
        anim.SetBool("isGrounded", isGrounded);

        // Face player
        transform.localScale = new Vector3(playerDistance > 0 ? 1 : -1,1,1);

        // Move or stop
        if (Mathf.Abs(playerDistance) > fireRange)
        {
            transform.Translate(Mathf.Sign(playerDistance) * moveSpeed * Time.deltaTime,0,0);
            anim.SetFloat("speed", 1f); // WALK
        }
        else
        {   
            // IDLE
            anim.SetFloat("speed", 0f); 
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                // Attack
                anim.SetTrigger("Attack");
                Fire(playerDistance);
                fireTimer = 0f;
            }
        }
        //Jump randomly
        if (isGrounded && Random.value < 0.005f)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void Fire(float distance)
    {
        GameObject proj = Instantiate(projectilePrefab,firePoint.position,Quaternion.identity);

        ProjectileEnemy p = proj.GetComponent<ProjectileEnemy>();
        if (p != null)
        {
            Vector2 dir = distance > 0 ? Vector2.right : Vector2.left;
            p.SetDirection(dir);
        }
    }
        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }


