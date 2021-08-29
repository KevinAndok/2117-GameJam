using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public enum WallType { basic, glass, rotating, trampoline };
    public WallType type;

    public float rotationSpeed = 1;
    public float destroyTime = .1f;
    public float trampolineForce = 5;

    Coroutine trampolineRoutine = null;

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
            if (trampolineRoutine == null)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity += Vector2.up * trampolineForce;
                trampolineRoutine = StartCoroutine(Trampoline());
            }
        }
    }

    IEnumerator Trampoline()
    {
        yield return new WaitForSeconds(.1f);
        trampolineRoutine = null;
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
