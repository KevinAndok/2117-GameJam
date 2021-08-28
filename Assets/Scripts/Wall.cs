using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public enum WallType { basic, glass, rotating, trampoline };
    public WallType type;

    public float rotationSpeed = 1;
    public float destroyTime = .1f;

    private void FixedUpdate()
    {
        if (type == WallType.rotating)
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == WallType.glass)
        {
            Destroy(gameObject, destroyTime);
        }
        else if (type == WallType.trampoline)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity *= 1.5f;
        }
    }

    private void OnDrawGizmos()
    {
        if (type == WallType.rotating)
        {
            Gizmos.color = Color.blue;
            float circleSize = Mathf.Sqrt(Mathf.Pow(transform.localScale.x / 2, 2) + Mathf.Pow(transform.localScale.y / 2, 2));
            Gizmos.DrawWireSphere(transform.position, circleSize);
        }
    }
}
