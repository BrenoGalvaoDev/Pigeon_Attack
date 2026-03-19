using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesScpawnManager : MonoBehaviour
{
    [SerializeField] Pooling pooling;
    [SerializeField] Collider2D collider2;

    List<GameObject> collectibles = new List<GameObject>();

    private void Start()
    {
        GetList();
    }

    public void SpawnCollectible()
    {
        GameObject collectible = GetCollectible();
        if (collectible != null)
        {
            collectible.transform.position = GetRandomPosition();
            collectible.SetActive(true);
            collectible.GetComponent<Collectables>().SetType((CollectableType)Random.Range(0, 3));
        }
    }

    void GetList()
    {
        collectibles.Clear();

        collectibles = pooling.ObjectPool("collectable");
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
