using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonsSpawnManager : MonoBehaviour
{
    [SerializeField] Pooling pooling;
    [SerializeField] Collider2D collider2;

    List<GameObject> collectibles = new List<GameObject>();

    private void Start()
    {
        GetList();
    }

    public void SpawnPigeon()
    {
        GameObject pigeon = GetCollectible();
        if (pigeon != null)
        {
            pigeon.transform.position = GetRandomPosition();
            pigeon.SetActive(true);
        }
    }

    void GetList()
    {
        collectibles.Clear();

        collectibles = pooling.ObjectPool("pigeon");
    }

    GameObject GetCollectible()
    {
        return collectibles.Find(obj => !obj.activeInHierarchy);
    }

    Vector2 GetRandomPosition()
    {
        Bounds bounds = collider2.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }
}
