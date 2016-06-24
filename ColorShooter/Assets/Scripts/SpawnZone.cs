using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class SpawnZone : MonoBehaviour
{
    BoxCollider2D spawnArea;

    void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();
    }

    public Vector2 GetPointInsideArea()
    {
        Vector3 min = spawnArea.bounds.min;
        Vector3 max = spawnArea.bounds.max;

        float xRand = Random.value;
        float yRand = Random.value;

        return new Vector2( (1.0f - xRand) * min.x + xRand * max.x, 
                            (1.0f - yRand) * min.y + yRand * max.y);
    }

}
