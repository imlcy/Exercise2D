using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    void Start()
    {
        GetComponent<Arrow>();
    }
    // aim
    private float radius = 10;
    private float angle;
    private float aimSpeed = 0.01f;

    void Update()
    {
        angle += Time.deltaTime * aimSpeed;
        if (angle < 360)
        {
            var rad = Mathf.Rad2Deg * angle;
            var x = radius * Mathf.Sin(rad);
            var y = radius * Mathf.Cos(rad);
            transform.position += new Vector3(x, y, 0f);
            transform.rotation = Quaternion.Euler(0f, 0f, angle * -1);
        }
        else
        {
            angle = 0;
        }
    }
}
