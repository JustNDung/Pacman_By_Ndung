using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    public Transform inside;
    public Transform outside;
    private void OnEnable() {
        StopAllCoroutines(); 
    }
    private void OnDisable() {
        if (this.gameObject.activeSelf) {
            StartCoroutine(ExitTransition());
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (this.enabled && other.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            this.ghost.movement.SetDirection(-this.ghost.movement.direction);
        }
    }

    private IEnumerator ExitTransition() {
        this.ghost.movement.SetDirection(Vector2.up, true);
        this.ghost.movement.rigidbody2D.bodyType = RigidbodyType2D.Kinematic; 
        this.ghost.movement.enabled = false;

        Vector3 position = transform.position;
        
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration) {
            Vector3 newPosition = Vector3.Lerp(position, this.inside.position, elapsed / duration);
            newPosition.z = position.z;
            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < duration) {
            Vector3 newPosition = Vector3.Lerp(this.inside.position, this.outside.position, elapsed / duration);
            newPosition.z = position.z;
            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        this.ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0f), true);
        this.ghost.movement.rigidbody2D.bodyType = RigidbodyType2D.Dynamic; 
        this.ghost.movement.enabled = true;
    }   


}
