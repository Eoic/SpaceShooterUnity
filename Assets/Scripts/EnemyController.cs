using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private SpawnManager _spawnManager;

    private void Update() {
        if (transform.position.y <= _spawnManager.WorldBoundBottomRight.y) {
            transform.position = new Vector3(Random.Range(_spawnManager.WorldBoundTopLeft.x, _spawnManager.WorldBoundBottomRight.x), _spawnManager.WorldBoundTopLeft.y);
            return;
        }

        transform.Translate(_speed * Time.deltaTime * Vector3.down);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "Player":
                var player = other.GetComponent<PlayerController>();

                if (player != null) {
                    player.ApplyDamage();
                    Destroy(gameObject);
                }

                break;
            case "Projectile":
                Destroy(other.gameObject);
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
