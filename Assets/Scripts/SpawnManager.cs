using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [SerializeField] private float _spawnFrequency = 5.0f;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Vector2 _worldBoundTopLeft;
    [SerializeField] private Vector2 _worldBoundBottomRight;

    private bool _isSpawning = true;

    public Vector3 WorldBoundTopLeft { get => _worldBoundTopLeft; }
    public Vector3 WorldBoundBottomRight { get => _worldBoundBottomRight; }

    private void Start() {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy() {
        while (_isSpawning) {
            Instantiate(_enemyPrefab, new Vector3(Random.Range(WorldBoundTopLeft.x, WorldBoundBottomRight.x), WorldBoundTopLeft.y, 0), Quaternion.identity, transform);
            yield return new WaitForSeconds(_spawnFrequency);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(WorldBoundTopLeft, new Vector3(WorldBoundBottomRight.x, WorldBoundTopLeft.y));
        Gizmos.DrawLine(new Vector3(WorldBoundBottomRight.x, WorldBoundTopLeft.y), WorldBoundBottomRight);
        Gizmos.DrawLine(WorldBoundBottomRight, new Vector3(WorldBoundTopLeft.x, WorldBoundBottomRight.y));
        Gizmos.DrawLine(new Vector3(WorldBoundTopLeft.x, WorldBoundBottomRight.y), WorldBoundTopLeft);
    }

    public void OnPlayerDestroyed() {
        _isSpawning = false;
    }
}
