using UnityEngine;

public class GhostChase : GhostBehavior
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            // Kiểm tra nếu không có hướng khả dụng
            if (node.availableDirections.Count == 0) {
                return;
            }

            Vector2 bestDirection = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // Tính toán vị trí mới
                Vector2 newPosition = (Vector2)this.transform.position + availableDirection;

                // Tính khoảng cách bình phương tới mục tiêu
                float distance = (this.ghost.target.position - (Vector3)newPosition).sqrMagnitude;

                // Cập nhật hướng tốt nhất nếu tìm được khoảng cách ngắn hơn
                if (distance < minDistance)
                {
                    bestDirection = availableDirection;
                    minDistance = distance;
                }
                else if (distance == minDistance)
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

    private void OnDisable()
    {
        // Kích hoạt chế độ scatter khi chase bị vô hiệu hóa
        this.ghost.scatter.Enable();
    }
}
