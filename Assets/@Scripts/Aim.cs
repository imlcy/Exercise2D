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

    private Transform ep; // ep�� private���� ����

    void Start()
    {
        // �÷��̾� ã��
        player = GameObject.FindGameObjectWithTag("Player");

        // dir�� �����ϱ� ���� �͵�
        _pivot = this.transform.position;
        ep = transform.Find("Aim/EndPoint");

        if (ep == null)
        {
            Debug.LogError("EndPoint�� ã�� �� �����ϴ�! ��θ� Ȯ���ϼ���.");
        }
    }

    void Update()
    {
        // �Ѹ�ó�� �����̱�
        transform.position = player.transform.position;
        transform.Rotate(0, 0, 250f * Time.deltaTime);

        _pivot = this.transform.position;

        if (ep != null) // ep�� null�� �ƴ� ���� ����
        {
            _endPoint = ep.position;
            dir = _endPoint - _pivot; // ���� static ������ ���� ����
        }
        else
        {
            Debug.LogWarning("ep�� null�Դϴ�. _endPoint�� ������ �� �����ϴ�.");
        }
    }
}