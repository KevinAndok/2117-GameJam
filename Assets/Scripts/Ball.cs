using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 100;
    public float gravityScale = 5;

    bool shot;
    Rigidbody2D rb;

    Transform robot;
    Transform hand;
    Transform cam;

    private void Awake()
    {
        cam = Camera.main.transform;
        hand = transform.parent;
        robot = hand.parent;
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

            if (mouse.x > robot.position.x)
            {
                robot.localScale = new Vector3(-1, 1, 1);
                hand.right = (mouse - (Vector2)transform.parent.position);
            }
            else
            {
                robot.localScale = new Vector3(1, 1, 1);
                hand.right = -(mouse - (Vector2)transform.parent.position);
            }

            if (Input.GetMouseButtonDown(0))
            {
                rb.AddForce(dir * speed, ForceMode2D.Impulse);
                rb.gravityScale = gravityScale;
                shot = true;
                GameController.startTimer.Invoke();
            }
        }
        else
        {
            var moveTo = Vector3.Lerp(cam.position, transform.localPosition, 1);
            moveTo.z = cam.position.z;
            Camera.main.transform.position = moveTo;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "finish") 
        {
            GameController.LevelComplete();
        }
    }
}
