using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;
    public bool eaten { get; private set; }

    public override void Enable(float duration)
    {
        base.Enable(duration);

        this.body.enabled = false;
        this.eyes.enabled = false;
        this.blue.enabled = true;
        this.white.enabled = false;

        Invoke(nameof(Flash), duration / 2.0f);

    }

    public override void Disable()
    {
        base.Disable();

        this.body.enabled = true;
        this.eyes.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false; 
    }

    private void Flash() {
        if (!this.eaten) {
           this.blue.enabled = false;
           this.white.enabled = true;
           this.white.GetComponent<AnimatedSprite>().Restart();
        }

    }
    private void OnEnable()
    {
        this.ghost.movement.speedMultiplier = 0.5f;
        this.eaten = false;
    }

    private void OnDisable()
    {
        this.ghost.movement.speedMultiplier = 1.0f;
        this.eaten = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman")) {
            if (this.enabled) {
                Eaten();
            }
        }
    }

    private void Eaten() {
        this.eaten = true;
        
        Vector3 position = this.ghost.home.inside.position;
        position.z = this.ghost.transform.position.z;
        this.ghost.transform.position = position;

        this.ghost.home.Enable(this.duration);

        this.body.enabled = false;
        this.eyes.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled)
        {
            // Kiểm tra nếu không có hướng khả dụng
            if (node.availableDirections.Count == 0) {
                return;
            }

            Vector2 bestDirection = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // Tính toán vị trí mới
                Vector2 newPosition = (Vector2)this.transform.position + availableDirection;

                // Tính khoảng cách bình phương tới mục tiêu
                float distance = (this.ghost.target.position - (Vector3)newPosition).sqrMagnitude;

                // Cập nhật hướng tốt nhất nếu tìm được khoảng cách ngắn hơn
                if (distance > maxDistance)
                {
                    bestDirection = availableDirection;
                    maxDistance = distance;
                }
                else if (distance == maxDistance)
                {
                    // Thêm logic ưu tiên nếu khoảng cách bằng nhau (tùy chọn)
                    if (availableDirection == Vector2.up || bestDirection == Vector2.zero)
                    {
                        bestDirection = availableDirection;
                    }
                }
            }

            // Đặt hướng di chuyển cho ghost
            this.ghost.movement.SetDirection(bestDirection);
        }
    }
}
