using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 100;
    public float gravityScale = 5;

    public SpriteRenderer powerBar;
    public Sprite[] powerSprites;

    int powerIncrement = 1;
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

            var increment = Camera.main.orthographicSize / 4;
            if (increment * 3 < dir.magnitude)
            {
                powerIncrement = 4;
                powerBar.sprite = powerSprites[3];
            }
            else if (increment * 2 < dir.magnitude)
            {
                powerIncrement = 3;
                powerBar.sprite = powerSprites[2];
            }
            else if (increment < dir.magnitude)
            {
                powerIncrement = 2;
                powerBar.sprite = powerSprites[1];
            }
            else
            {
                powerIncrement = 1;
                powerBar.sprite = powerSprites[0];
            }

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
                rb.AddForce(dir * speed * powerIncrement, ForceMode2D.Impulse);
                rb.gravityScale = gravityScale;
                shot = true;
                GameController.instance.startTimer?.Invoke();
            }
        }
        else
        {
            var moveTo = Vector3.Lerp(cam.position, transform.position, 1);
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
