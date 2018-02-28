using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables (private)
    [SerializeField] [Range(1.0f, 20.0f)] private float m_speed = 5.0f;
    [SerializeField] [Range(100.0f, 1000.0f)] private float m_jumpForce = 500.0f;
    [SerializeField] private bool m_enableDoubleJump = false;
    [SerializeField] [Range(1.0f, 10.0f)] private float m_fallMultiplier = 5.0f;
    [SerializeField] [Range(1.0f, 10.0f)] private float m_jumpMultiplier = 5.0f;
    [SerializeField] private PlayerController m_oppositePlayerController = null;
    [SerializeField] private Vector3 m_worldOffset;
    [SerializeField] private Transform groundCheckRight;
    [SerializeField] private Transform groundCheckLeft;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private LayerMask whatIsGround;
    private Rigidbody2D m_rigidbody2D;
    private Animator m_animator;
    private bool m_isActive = false;
    private bool m_facingLeft = false;
    private bool m_doubleJump = false;
    private bool m_isGrounded = false;
    private bool m_isFlipping = false;

    #endregion

    #region Properties (public)
    public Rigidbody2D Rigidbody2D { get { return m_rigidbody2D; } }
    public bool IsGrounded { get { return m_isGrounded; } }
    public bool DoubleJump {get { return m_doubleJump; } }
    [SerializeField] private string m_horizontal;

    #endregion

    #region Unity event functions



    // Use this for initialization
    void Start()
    {

        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isFlipping)
        {
            if (m_isActive)
            {
                ActiveUpdate();
            }
            else
            {
                InactiveUpdate();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!m_isFlipping)
        {
            if (m_isActive)
            {
                ActiveFixedUpdate();
            }
            else
            {
                InactiveFixedUpdate();
            }
        }

    }



    #endregion

    #region Methods

    private void ActiveUpdate()
    {
        if ((m_isGrounded || ((!m_doubleJump && !m_oppositePlayerController.DoubleJump)&& m_enableDoubleJump)) && Input.GetButtonDown("Jump"))
        {
            m_animator.SetBool("Ground", false);
            Vector2 velocity = m_rigidbody2D.velocity;
            velocity.y = 0.0f;
            m_rigidbody2D.velocity = velocity;
            m_rigidbody2D.AddForce(Vector2.up * m_jumpForce);

            if (!m_doubleJump && !m_isGrounded)
            {
                m_doubleJump = true;
            }
        }
    }

    private void ActiveFixedUpdate()
    {
        if (m_isGrounded)
            m_doubleJump = false;
        m_isGrounded = Physics2D.OverlapCircle(groundCheckRight.position, groundRadius, whatIsGround);
        if (!m_isGrounded)
            m_isGrounded = Physics2D.OverlapCircle(groundCheckLeft.position, groundRadius, whatIsGround);
        m_animator.SetBool("Ground", m_isGrounded);

        m_animator.SetFloat("vSpeed", m_rigidbody2D.velocity.y);

        if (m_rigidbody2D.velocity.y < 0.0f)
        {
            m_rigidbody2D.velocity += (Vector2.up * Physics2D.gravity.y) * (m_fallMultiplier - 1.0f) * Time.deltaTime;
        }
        else if (m_rigidbody2D.velocity.y > 0.0f)
        {
            m_rigidbody2D.velocity += (Vector2.up * Physics2D.gravity.y) * (m_jumpMultiplier - 1.0f) * Time.deltaTime;
        }

        //float move = CrossPlatformInputManager.GetAxis("Horizontal");
        float move = Input.GetAxis(m_horizontal);


        m_rigidbody2D.velocity = new Vector2(move * m_speed, m_rigidbody2D.velocity.y);

        m_animator.SetFloat("Speed", Mathf.Abs(m_rigidbody2D.velocity.x));

        if (move > 0 && m_facingLeft)
            {
                Flip();
                m_oppositePlayerController.Flip();
            }
        else if (move < 0 && !m_facingLeft)
            {
                Flip();
                m_oppositePlayerController.Flip();
            }
    }

    private void InactiveUpdate()
    {

    }

    private void InactiveFixedUpdate()
    {
        if (m_isGrounded)
            m_doubleJump = false;
        m_isGrounded = Physics2D.OverlapCircle(groundCheckRight.position, groundRadius, whatIsGround);
        if (!m_isGrounded)
            m_isGrounded = Physics2D.OverlapCircle(groundCheckLeft.position, groundRadius, whatIsGround);
        transform.position = m_oppositePlayerController.transform.position + m_worldOffset;
        m_animator.SetFloat("Speed", Mathf.Abs(m_oppositePlayerController.Rigidbody2D.velocity.x));
        m_animator.SetBool("Ground", m_oppositePlayerController.IsGrounded);
        m_animator.SetFloat("vSpeed", m_oppositePlayerController.Rigidbody2D.velocity.y);
    }

    public void Flip()
    {
        m_facingLeft = !m_facingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void SetActive(bool active)
    {
        m_isActive = active;
    }

    public IEnumerator PauseDuringFlip(float duration)
    {
        m_isFlipping = true;
        float t = 0.0f;
        Vector3 curVelocity = m_oppositePlayerController.Rigidbody2D.velocity;
        m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        while (t < duration)
        {
            yield return null;
            t += Time.deltaTime;
        }
        m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_rigidbody2D.velocity = curVelocity;
        m_isFlipping = false;
    }

    #endregion
}
