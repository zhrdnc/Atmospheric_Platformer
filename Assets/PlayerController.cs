using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Ghost Settings")]
    public GameObject ghostPrefab; 
    public float ghostDelay = 0.05f; 

    [Header("Movement Settings")]
    public float speed = 400f; 
    public float jumpForce = 5f;

    [Header("Dash Settings")]
    public float dashForce = 100f; 
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;
    private bool hasDashedInAir = false; 

    [Header("Jump Logic")]
    private bool isGrounded;
    private int jumpCount = 0; 
    public int maxJumps = 2;   
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    [SerializeField] private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if (isDashing) return; 

     
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        
        if (isGrounded)
        {
           
            jumpCount = 0; 
            hasDashedInAir = false; 
            animator.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            if (isGrounded || !hasDashedInAir)
            {
                StartCoroutine(Dash());
            }
        }

        float xInput = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(xInput * speed * Time.deltaTime, rb.velocity.y);

        if (xInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (xInput < 0) transform.localScale = new Vector3(-1, 1, 1);

        animator.SetBool("isRunning", xInput != 0);

        
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
                Jump();
        }
    }

     void Jump()
    {
    rb.velocity = new Vector2(rb.velocity.x, 0); 
    
    rb.velocity = new Vector2(rb.velocity.x, jumpForce); 
    jumpCount++;
    animator.SetBool("isJumping", true);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        if (!isGrounded) hasDashedInAir = true;
        
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);

        float timer = 0;
        while (timer < dashTime)
        {
            GameObject currentGhost = Instantiate(ghostPrefab, transform.position, transform.rotation);
            SpriteRenderer ghostSR = currentGhost.GetComponent<SpriteRenderer>();
            ghostSR.sprite = GetComponent<SpriteRenderer>().sprite;
            currentGhost.transform.localScale = transform.localScale;
            ghostSR.color = new Color(0.5f, 0.5f, 1f, 0.6f); 

            rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);

            timer += ghostDelay;
            yield return new WaitForSeconds(ghostDelay);
        }

   
        rb.gravityScale = originalGravity;

        rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y); 
        
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}