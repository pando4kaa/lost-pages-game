using UnityEngine;
using Cinemachine;

public class MapTransation : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBounds;
    CinemachineConfiner confiner;
    [SerializeField] Direction direction;
    [SerializeField] float additivePos = 2f;

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Kent"))
        {
            confiner.m_BoundingShape2D = mapBounds;
            UpdateKentPosition(collision.gameObject);

            MapController_Manual.Instance?.HighlightArea(mapBounds.name);
            MapController_Dynamic.Instance?.UpdateCurrentArea(mapBounds.name);
        }
    }

    private void UpdateKentPosition(GameObject kent)
    {
        Vector3 kentPosition = kent.transform.position;
        switch (direction)
        {
            case Direction.Up:
                kentPosition.y += additivePos;
                break;
            case Direction.Down:
                kentPosition.y -= additivePos;
                break;
            case Direction.Left:
                kentPosition.x += additivePos;
                break;
            case Direction.Right:
                kentPosition.x -= additivePos;
                break;
        }
        kent.transform.position = kentPosition;
    }
}
