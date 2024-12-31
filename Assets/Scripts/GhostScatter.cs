using UnityEngine;

public class GhostScatter : GhostBehavior
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

            // Chọn hướng ngẫu nhiên, tránh hướng ngược lại
            int index = Random.Range(0, node.availableDirections.Count);
            Vector2 chosenDirection = node.availableDirections[index];

            while (chosenDirection == -this.ghost.movement.direction && node.availableDirections.Count > 1) {
                index = Random.Range(0, node.availableDirections.Count);
                chosenDirection = node.availableDirections[index];
            }

            // Đặt hướng di chuyển
            this.ghost.movement.SetDirection(chosenDirection);
        }
    }

    private void OnDisable()
    {
        // Kích hoạt chế độ chase khi scatter bị vô hiệu hóa
        this.ghost.chase.Enable();
        Debug.Log("Scatter disabled");
    }
}
