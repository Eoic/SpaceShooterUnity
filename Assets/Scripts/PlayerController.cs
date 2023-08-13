using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Vector2 _topLeftBound;
    [SerializeField] private Vector2 _bottomRightBound;
    [SerializeField] private bool _wrapAroundX;
    [SerializeField] private bool _wrapAroundY;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private GameObject _tripleShotProjectile;
    [SerializeField] private Vector3 _projectileOffset;
    [SerializeField] private float _fireRate = 0.25f;
    [SerializeField] private int lives = 3;
    private float _nextFire;
    private bool _isTripleShotActive = true;
    private Vector3 _direction;
    private Vector3 _newPosition;
    private SpawnManager _spawnManager;

    private void Start() {
        transform.position = Vector3.zero;
        _nextFire = Time.time;
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    private void Update() {
        ApplyMovement();
        ApplyFiring();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_topLeftBound, new Vector3(_bottomRightBound.x, _topLeftBound.y));
        Gizmos.DrawLine(new Vector3(_bottomRightBound.x, _topLeftBound.y), _bottomRightBound);
        Gizmos.DrawLine(_bottomRightBound, new Vector3(_topLeftBound.x, _bottomRightBound.y));
        Gizmos.DrawLine(new Vector3(_topLeftBound.x, _bottomRightBound.y), _topLeftBound);
    }

    private void ApplyMovement() {
        _direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        if (_direction == Vector3.zero)
            return;

        _newPosition = transform.position + _speed * Time.deltaTime * _direction.normalized;

        if (_wrapAroundX) _newPosition = WrapMovementX(_newPosition);
        else _newPosition.x = Mathf.Clamp(_newPosition.x, _bottomRightBound.x, _topLeftBound.x);

        if (_wrapAroundY) _newPosition = WrapMovementY(_newPosition);
        else _newPosition.y = Mathf.Clamp(_newPosition.y, _bottomRightBound.y, _topLeftBound.y);

        transform.position = _newPosition;
    }

    private Vector3 WrapMovementX(Vector3 targetPosition) {
        if (targetPosition.x >= _topLeftBound.x)
            targetPosition.x = _bottomRightBound.x;
        else if (targetPosition.x <= _bottomRightBound.x)
            targetPosition.x = _topLeftBound.x;

        return targetPosition;
    }

    private Vector3 WrapMovementY(Vector3 targetPosition) {
        if (targetPosition.y >= _topLeftBound.y)
            targetPosition.y = _bottomRightBound.y;
        else if (_newPosition.y <= _bottomRightBound.y)
            targetPosition.y = _topLeftBound.y;

        return targetPosition;
    }

    private void ApplyFiring() {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _nextFire) {
            _nextFire = Time.time + _fireRate;
            var projectile = _isTripleShotActive ? _tripleShotProjectile : _projectile;  
            Instantiate(projectile, transform.position + _projectileOffset, Quaternion.identity);
        }
    }

    public void ApplyDamage() {
        lives--;

        if (lives <= 0) {
            if (_spawnManager != null) {
                _spawnManager.OnPlayerDestroyed();
            } else {
                Debug.LogError("Spawn Manager is not set.");
            }

            Destroy(gameObject);
        }
    }
}
