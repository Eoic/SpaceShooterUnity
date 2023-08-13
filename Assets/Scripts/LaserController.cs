using UnityEngine;

public class LaserController : MonoBehaviour {
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _yAxisOffset = 6f;
    
    private void Update() {
        transform.Translate(_speed * Time.deltaTime * Vector3.up);

        if (transform.position.y >= _yAxisOffset) {
            Destroy(gameObject);
        }
    }
}
