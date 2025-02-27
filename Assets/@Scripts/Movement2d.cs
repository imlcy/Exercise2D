using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class Movement2d : MonoBehaviour
{
    [SerializeField]
    float _speed = 5.0f;

    [SerializeField]
    int _fireSpeed = 30; // 이동 속도 조절

    [SerializeField]
    float jumpForce = 10.0f;

    Arrow arrow;

    private Rigidbody2D _rb;
    private bool isGrounded = true;
    private bool isTimeStopped = false; // 시간 정지 상태 여부

    private float _delayTime = 0.1f;
    private float _lastSpaceTime = 0f;
    private Vector2 _nowPosition;

    private float inputTimeLimit = 5.0f;
    private Coroutine inputCoroutine;

    private Vector3 _targetPosition; // 목표 위치
    private bool isFiring = false; // 발사 중 여부


    Aim aim;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        aim = GetComponent<Aim>();
    }

    private void Update()
    {
        // 공중에서 Space 누르면 시간 정지
        if (!isGrounded && !isTimeStopped && Input.GetKeyDown(KeyCode.Space) && Time.time - _lastSpaceTime > _delayTime)
        {
            StopTime();
            inputCoroutine = StartCoroutine(CheckInputTimeLimit());
        }

        // shot fire
        if (isTimeStopped && Input.GetKeyDown(KeyCode.Space) && Time.time - _lastSpaceTime > _delayTime)
        {
            Fire();
            StopCoroutine(inputCoroutine);
            Debug.Log("stop coroutine");
        }

        // R키 누르면 시간 재개
        if (isTimeStopped && Input.GetKeyDown(KeyCode.R) && Time.time - _lastSpaceTime > _delayTime)
        {
            ResumeTime();
            StopCoroutine(inputCoroutine);
            Debug.Log("stop coroutine");
        }
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        // 땅에있으면서 시간이 멈춘 상태가 아니면 이동 및 점프 가능
        if (!isTimeStopped)
        {
            if (isGrounded)
            {   // move
                _rb.velocity = new Vector2(horizontal * _speed, _rb.velocity.y);

                // 점프 (지상에서만 가능)
                if (Input.GetKey(KeyCode.Space))
                {
                    Debug.Log("jump");
                    _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    isGrounded = false;
                    _lastSpaceTime = Time.time;
                }
            }
            else
            {
                // direction change
                _rb.velocity = new Vector2(horizontal * _speed, _rb.velocity.y);
            }
        }

        // Fire 실행 시 목표 위치로 이동
        if (isFiring)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _fireSpeed * Time.fixedDeltaTime);

            // 목표 위치에 도달하면 멈춤
            if (Vector3.Distance(transform.position, _targetPosition) < 0.01f) // "collision이 감지되면" 이라는 조건 추가하고싶은데..
            {
                isFiring = false;
                ResumeTime();
            }
        }
    }

    // 💡 시간 정지 (공중에 멈춤)
    public void StopTime()
    {
        Debug.Log("Time Stopped!");
        isTimeStopped = true;
        _nowPosition = transform.position; // 현재 위치 저장
        _rb.velocity = Vector2.zero; // 즉시 속도 0
        _rb.gravityScale = 0; // 중력 제거
        _rb.bodyType = RigidbodyType2D.Kinematic; // 물리 엔진 비활성화 (충돌 유지)
        _lastSpaceTime = Time.time;
        Debug.Log(System.DateTime.Now);

        // 현재 위치에 고정
        transform.position = _nowPosition;
    }

    // 발사
    public void Fire()
    {
        Debug.Log("fire!");
        _rb.gravityScale = 0;

        // 그 방향으로 발사하기
        //aim의 pivot과 endpoint의 position을 가져와야함.
        Vector3 dir = Aim.dir; // 방향벡터
        _targetPosition = transform.position + dir.normalized * 5; // 목표 위치 설정  << 여기서 arrow방향대로 날아가게 하면 될듯. 
        Debug.Log(_targetPosition); 
        isFiring = true; // 이동 시작
    }

    // 💡 시간 재개
    public void ResumeTime()
    {
        Debug.Log("Time Resumed!");
        isTimeStopped = false;
        _rb.gravityScale = 6; // 중력 재활성화
        _rb.bodyType = RigidbodyType2D.Dynamic; // 물리 재활성화
        // 마지막 방향을 저장해놨다가 그걸 속도를 곱해서 자연스러운 물리현상 적용.
        
    }

    // 바닥 충돌 감지 (착지 시 자동 시간 재개)
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Stopped Time..
    // 💡 5초 동안 입력 없으면 실행되는 코루틴
    public IEnumerator CheckInputTimeLimit()
    {
        yield return new WaitForSeconds(inputTimeLimit);
        Debug.Log("There is no any input");
        ResumeTime();
    }
}
