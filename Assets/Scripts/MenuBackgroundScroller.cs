using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MenuBackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 1f;
    public Vector2 scrollDirection = new Vector2(1f, 1f);

    private Vector3 startPosition;
    //private float mapWidth;
    //private float mapHeight;
    private Vector3 worldMin;
    private Vector3 worldMax;
    private float scrollChangeTimer = 0f;
    private bool isScrollDirChange = false;

    [SerializeField] private Tilemap tilemap;

    void Start()
    {
        startPosition = transform.position;

        worldMin = tilemap.transform.TransformPoint(tilemap.localBounds.min);
        worldMax = tilemap.transform.TransformPoint(tilemap.localBounds.max);
    }

    
    void Update()
    {
        Vector3 newPosition = transform.position + (Vector3)(scrollDirection.normalized * scrollSpeed * Time.deltaTime);

        if ((Mathf.Abs(transform.position.x) > Mathf.Max(worldMin.x, worldMax.x) - 10 || Mathf.Abs(transform.position.y) > Mathf.Max(worldMin.y, worldMax.y) - 10) && scrollChangeTimer <= 0)
        {
            Vector2 tempScrollDir = scrollDirection;
            if (Mathf.Abs(transform.position.x) > Mathf.Max(worldMin.x, worldMax.x) - 10)
            {
                if (scrollDirection.x <= 0)
                {
                    tempScrollDir.x = Random.Range(0f, 1f);
                }
                else
                {
                    tempScrollDir.x = Random.Range(-1f, 0f);
                }
                tempScrollDir.y = Random.Range(-1f, 1f);
            }
            else
            {
                if (scrollDirection.y <= 0)
                {
                    tempScrollDir.y = Random.Range(0f, 1f);
                }
                else
                {
                    tempScrollDir.y = Random.Range(-1f, 0f);
                }
                tempScrollDir.x = Random.Range(-1f, 1f);
            }
          
            scrollDirection = tempScrollDir;
            isScrollDirChange = true;
            scrollChangeTimer= 2f;
        }

        if(isScrollDirChange)
        {
            scrollChangeTimer -= Time.deltaTime;
        }
        transform.position = newPosition;
    }
}