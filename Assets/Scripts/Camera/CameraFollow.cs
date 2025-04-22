using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform _target; // Посилання на трансформ Кента
    [SerializeField] private float _smoothSpeed = 0.3f; // Швидкість плавності руху камери
    [SerializeField] private Vector3 _offset = new Vector3(0, 0, -10); // Зміщення камери від цілі

    [Header("Bounds")]
    [SerializeField] private bool _useBounds = false; // Чи використовувати межі
    [SerializeField] private float _minX, _maxX, _minY, _maxY; // Межі руху камери

    private Vector3 _velocity = Vector3.zero;
    private float _smoothTime = 0.3f;

    private void FixedUpdate()
    {
        if (_target == null) return;

        // Обчислюємо бажану позицію камери
        Vector3 targetPosition = _target.position + _offset;

        // Застосовуємо межі, якщо вони увімкнені
        if (_useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, _minX, _maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, _minY, _maxY);
        }

        // Використовуємо SmoothDamp замість Lerp для більш плавного руху
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
    }

    // Метод для встановлення меж камери
    public void SetBounds(float minX, float maxX, float minY, float maxY)
    {
        _minX = minX;
        _maxX = maxX;
        _minY = minY;
        _maxY = maxY;
        _useBounds = true;
    }
}