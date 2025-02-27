using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UIElements;

public class Aim : MonoBehaviour
{
    private GameObject player;

    public static Vector3 _pivot;
    public static Vector3 _endPoint;
    public static Vector3 dir;

    private Transform ep; // ep를 private으로 변경

    void Start()
    {
        // 플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player");

        // dir을 설정하기 위한 것들
        _pivot = this.transform.position;
        ep = transform.Find("Aim/EndPoint");

        if (ep == null)
        {
            Debug.LogError("EndPoint를 찾을 수 없습니다! 경로를 확인하세요.");
        }
    }

    void Update()
    {
        // 한몸처럼 움직이기
        transform.position = player.transform.position;
        transform.Rotate(0, 0, 250f * Time.deltaTime);

        _pivot = this.transform.position;

        if (ep != null) // ep가 null이 아닐 때만 실행
        {
            _endPoint = ep.position;
            dir = _endPoint - _pivot; // 전역 static 변수에 방향 저장
        }
        else
        {
            Debug.LogWarning("ep가 null입니다. _endPoint를 갱신할 수 없습니다.");
        }
    }
}