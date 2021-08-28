using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 100;
    public float gravityScale = 5;

    bool shot;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!shot)
        {
            Vector2 mouse = Input.mousePosition;
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            Vector2 dir = mouse - (Vector2)transform.position;
            dir = dir.normalized;

            transform.parent.right = mouse - (Vector2)transform.parent.position;

            if (Input.GetMouseButtonDown(0))
            {
                rb.AddForce(dir * speed, ForceMode2D.Impulse);
                rb.gravityScale = gravityScale;
                shot = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
