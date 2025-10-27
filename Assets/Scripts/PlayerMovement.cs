using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float jumpForce = 5f; // (1-2) 점프 힘 (Inspector에서 조절 가능)
    public float runSpeedMultiplier = 1.5f; // (1-2) 달리기 속도 배율
    

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    // --- 추가된 변수 ---
    private float m_JumpCooldown = 0.5f; // (1-2) 점프 쿨다운 (1초에 최대 2번)
    private float m_LastJumpTime = -0.5f; // 마지막 점프 시간 (시작 시 점프 가능하도록)

    // 입력값 처리를 위한 플래그
    private bool m_JumpInput;
    private bool m_IsRunningInput;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Input은 Update에서 감지하는 것이 더 정확합니다.
    void Update()
    {
        // (1-2, 1-3) Space 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_JumpInput = true;
        }

        // (1-2, 1-3) Left Shift 키 입력 감지
        m_IsRunningInput = Input.GetKey(KeyCode.LeftShift);
    }

   

    void FixedUpdate()
    {
        // (1-2) w, s, a, d 및 방향키 입력
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        // (1-3) Idle / Walk 상태 구분
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        // (1-3) Run 상태 구분
        bool isRunning = isWalking && m_IsRunningInput;
        m_Animator.SetBool("IsRunning", isRunning);

        // 회전 로직 (기존과 동일)
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        // (1-2, 1-3) 점프 로직 처리
        HandleJumping();
    }

    // (1-2, 1-3) 점프 로직을 처리하는 별도 함수
    private void HandleJumping()
    {
        // 점프 입력이 들어왔는지 확인
        if (m_JumpInput)
        {
            // (1-2) 점프 쿨다운 (1초에 2번 = 0.5초 쿨다운)
            bool canJump = (Time.time - m_LastJumpTime) >= m_JumpCooldown;

            // (1-2) 이동 중 점프 가능 (isWalking 여부와 관계없이 쿨다운만 체크)
            if (canJump)
            {
                // (1-3) Jump 애니메이션 트리거 실행
                m_Animator.SetTrigger("Jump");

                // (1-2) 위쪽 방향으로 점프 (물리 적용)
                m_Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                // 마지막 점프 시간 기록
                m_LastJumpTime = Time.time;
            }

            // 입력 플래그 리셋
            m_JumpInput = false;
        }
    }

    void OnAnimatorMove()
    {
        // (1-2) 루트 모션으로 인한 프레임당 이동량
        float speed = m_Animator.deltaPosition.magnitude;

        // (1-2) Left Shift를 누른 상태(IsRunning)일 때 1.5배 속도 적용
        if (m_Animator.GetBool("IsRunning"))
        {
            speed *= runSpeedMultiplier;
        }

        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * speed);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}