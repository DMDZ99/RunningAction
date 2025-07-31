using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerBaseMovement : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private Collider2D slideCollider;

    private Animator animator;
    private Collider2D runnerCollider;
    private int jumpCount = 0;

    private bool isGrounded; // ���� ��Ҵ��� Ȯ���ϴ� ����
    private bool isJumping; // ������ �ϰ� �ִ��� Ȯ���ϴ� ����

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        runnerCollider = GetComponent<BoxCollider2D>(); //��ũ��Ʈ�� �ִ� ������Ʈ�� �ڶ��̴��� �޾ƿ´�.
    }

    // Update is called once per frame
    void Update()
    {
        // ������ ���� ����
        RunnerMovementMethod();

        //���� ����
        RunnerJumpMethod();

        // �����̵� ����
        RunnerSlideMethod();
    }

    private void FixedUpdate() //�������� 0.2�� ���� �����Ѵ�.
    {
        // ���� ��Ҵ��� Ȯ���ϴ� ��� raycast, ������ �󿡼��� Ray�� �׷��ִ� �Լ�

        Debug.DrawRay(rigidbody2D.position, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigidbody2D.position, Vector3.down, 2.0f, platformLayer);

        if (rayHit.collider != null && rigidbody2D.velocity.y < 0)
        {
            //Debug.Log(rayHit.collider.name);
            //Debug.Log(rayHit.distance);

            if (rayHit.distance < 1.05f) // rayHit�� �ð� ���� �ִ� ���ǽ�
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && isJumping)
        {
            animator.SetBool("isJump", false);
            isJumping = false;
            jumpCount = 0;
        }

    }

    private void RunnerMovementMethod()
    {
        Vector3 baseForwardSpeed = rigidbody2D.velocity;
        baseForwardSpeed.x = forwardSpeed;

        rigidbody2D.velocity = baseForwardSpeed;
    }

    private void RunnerJumpMethod()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Mathf.Abs(rigidbody2D.velocity.y) < 0.01f) // Mathf.Abs ���밪�� ���� �� ����Ѵ�
            {
                jumpCount = 1;
            }
            else
            {
                jumpCount++;
            }

            if (jumpCount <= 2)
            {
                animator.SetBool("isJump", true);
                rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                isJumping = true; // ������ �����ϸ� bool �� isJumping�� ��
            }

            // rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse); �⺻ ���� ����
            // ForceMode2D.Impulse �� �� �������� ������ �����ϴ� �ڵ�
            // && rigidbody2D.velocity.y == 0 ���� ������ �����ϱ����� ���ǽ� (������ �����ߴ�)
            // && Mathf.Approximately(rigidbody2D.velocity.y, 0) ���� ������ �����ϱ����� ���ǽ� (������ �����ߴ�)
        }
    }

    private void RunnerSlideMethod()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("isSlide", true);
            slideCollider.enabled = true; // component�� ���� Ű���ϴ� �ڵ�.
            runnerCollider.enabled = false;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("isSlide", false);
            slideCollider.enabled = false;
            runnerCollider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log(collision.tag);

        }
    }
}
