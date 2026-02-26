using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControler : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 20f;

    public Transform groundChecker;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    public GameObject projectilePrefab;
    public Transform FirePoint;
    public int health = 100;
    Rigidbody2D rb;
    Vector2 moveInput;
    bool isGrounded;
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump()
    {
        if (!isGrounded) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }


    void Update()
    {
        // 1. Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundRadius, groundLayer);

        // 2. Update Animator parameters
        anim.SetFloat("speed", Mathf.Abs(moveInput.x));
        anim.SetBool("isGrounded", isGrounded);

        // 3. Flip sprite based on movement direction
        if (moveInput.x > 0)
            transform.localScale = Vector3.one;
        else if (moveInput.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }
    void OnAttack()
    {
        anim.SetTrigger("Attack");
        FireProjectile();
    }

    public void FireProjectile()
    {
        // The "Safety Check"
        if (FirePoint == null)
        {
            Debug.LogError("Hey! You forgot to drag the FirePoint into the Inspector!");
            return;
        }

        GameObject proj = Instantiate(projectilePrefab, FirePoint.position, Quaternion.identity);
        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        proj.GetComponent<ProjectilePlayer>().SetDirection(dir);
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
