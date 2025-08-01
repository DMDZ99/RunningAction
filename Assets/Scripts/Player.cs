using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    //public event Action OnHitObstacle;
    public void EnableControls() => GameManager.Instance.controlsEnabled = true;
    public void DisableControls() => GameManager.Instance.controlsEnabled = false;
    public void ResetState()
    {
        GameManager.Instance.jumpCount = 0;
        GameManager.Instance.isJumping = false;
        GameManager.Instance.animator.SetBool("isJump", false);
        GameManager.Instance.animator.SetBool("isSlide", false);
        GameManager.Instance.slideCollider.enabled = false;
        GameManager.Instance.runnerCollider.enabled = true;
    }

    public void Update()
    {
        if (!GameManager.Instance.controlsEnabled) return;
        // 앞으로 가는 로직
        GameManager.Instance.RunnerMovementMethod();

        //점프 로직
        GameManager.Instance.RunnerJumpMethod();

        // 슬라이드 로직
        GameManager.Instance.RunnerSlideMethod();
    }

    private void FixedUpdate()//고정으로 0.2초 마다 실행한다.
    {
        // 땅에 닿았는지 확인하는 방법 raycast, 에디터 상에서만 Ray를 그려주는 함수
        Debug.DrawRay(GetComponent<Rigidbody2D>().position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(GetComponent<Rigidbody2D>().position, Vector3.down, 2.0f, GameManager.Instance.platformLayer);

        if (rayHit.collider != null && GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            GameManager.Instance.isGrounded = rayHit.distance < 1.05f;
        }
        else
        {
            GameManager.Instance.isGrounded = false;
        }

        if (GameManager.Instance.isGrounded && GameManager.Instance.isJumping)
        {
            GameManager.Instance.animator.SetBool("isJump", false);
            GameManager.Instance.isJumping = false;
            GameManager.Instance.jumpCount = 0;
        }
    }

    // 장애물 충돌 시 구현된는 애니메이션
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // damage 시작점 무적 판단 (무적 시 리턴)
        if (GameManager.Instance.isInvincible == true)
            return;
        // 무적이라는 기능은 없다 지금 메소드에서는 isInvincible가 참일 때 충돌하는 코드를 건너 뛰어서 return 하게 만든거다.

        if (collision.CompareTag("Obstacle"))
        {
            
            if (collision.CompareTag("Obstacle"))
            {
                // HP 깎이는 메소드 추가해야 함
                GameManager.Instance.TakeDamage(10);

                GameManager.Instance.SpriteDamageMethod();
                GameManager.Instance.Invoke("SpriteResetMethod", GameManager.Instance.invincibleTime);

                //animator.SetTrigger("isDamege");
                // bool은 실행과 끝을 다 체크해야 할때, trigger는 실행만 할 때 (돌아가는 transition을 부착해야한다)
                // condition이 없으면 애니메이션이 끝나고 바로 다음동작 실행

                // Color color = playerOriginSprite.color;
                //color.a = 0.5f;
                //playerOriginSprite.color = color;
                // 색갈+무적
                // Invoke로 다시돌아가는 로직 구현 (시간턴을 주는 코드)
                // Invoke의 사용법 [ Invoke("함수이름", 딜레이_초); ] 이제 알파 값을 조절하는 함수를 구현해야한다.
            }
        }

        if (collision.CompareTag("Coin"))
        {
            
            if (collision.CompareTag("Coin"))
            {
                Destroy(collision.gameObject);
                // 점수가 늘어나는 메소드 추가해야함
            }
        }

        if (collision.CompareTag("RushItem"))
        {

            if (collision.CompareTag("RushItem"))
            {
                Destroy(collision.gameObject);
                GameManager.Instance.StartSuperRushMethod();
                GameManager.Instance.Invoke("EndSuperRushMethod", GameManager.Instance.invincibleTime = 5);
            }
        }

    }
}
