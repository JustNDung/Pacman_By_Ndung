using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PacmanMovement : MonoBehaviour
{
    public float speed = 8.0f;
    public float speedMultiplier = 1.0f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;
    public new Rigidbody2D rigidbody2D { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }
    private void Awake()
    {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        this.startingPosition = transform.position;
    }

    private void Start() {
        ResetState();
    }

    public void ResetState() {
        this.speedMultiplier = 1.0f;
        this.direction = this.initialDirection;
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startingPosition;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        this.enabled = true;
    }

    private void Update() {
        if (this.nextDirection != Vector2.zero) {
            SetDirection(this.nextDirection);
        }
    }

    private void FixedUpdate() {
        Vector2 position = this.rigidbody2D.position;
        Vector2 translation = this.direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;
        this.rigidbody2D.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 direction, bool forced = false) {
        if (!Occupied(direction) || forced) {
            this.direction = direction;
            this.nextDirection = Vector2.zero;
        } else {
            this.nextDirection = direction;
        }
    }

    public bool Occupied(Vector2 direction) {
        RaycastHit2D hit = Physics2D.BoxCast(
            this.transform.position, Vector2.one * 0.75f, 
            0f, direction, 1.5f, this.obstacleLayer
        );
        return hit.collider != null;
    }
}
