using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // observe around own position as much as observation radius.
    // trace the enemy.

    // 거리가 가까워지면 플레이어의 x 좌표로 서서히 x좌표 이동.

    GameObject player = GameObject.FindGameObjectWithTag("Player");

    [SerializeField]
    Vector2 radius;

    private void FixedUpdate()
    {
        while(true)
        {
            Movement2d p = player.GetComponent<Movement2d>();
            Vector2 distance = p.transform.position - transform.position;

            //if (distance < radius )
            {

            }
        }
    }

}
