using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostBehavior initialBehavior;
    public Transform target;
    public int points = 200;

    private void Awake()
    {
        this.movement = GetComponent<Movement>();
        this.home = GetComponent<GhostHome>();
        this.chase = GetComponent<GhostChase>();
        this.frightened = GetComponent<GhostFrightened>();
        this.scatter = GetComponent<GhostScatter>();
        
    }

    private void Start() {
        ResetState();
    }

    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.movement.ResetState();

        this.home.Disable();
        this.chase.Disable();
        this.frightened.Disable();
        
        if (this.home != this.initialBehavior) {
            this.home.Disable();
        }

        if (this.initialBehavior != null) {
            this.initialBehavior.Enable();
        }
    }

     private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman")) {
            if (this.frightened.enabled) {
                FindFirstObjectByType<GameManager>().GhostEaten(this);
            } else {
                FindFirstObjectByType<GameManager>().PacmanEaten();
            }
        }
     }
        
}
