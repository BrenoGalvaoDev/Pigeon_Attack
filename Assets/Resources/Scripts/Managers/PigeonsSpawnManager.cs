using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonsSpawnManager : MonoBehaviour
{
    [SerializeField] Pooling pooling;
    [SerializeField] Collider2D collider2;

    List<GameObject> pigeons = new List<GameObject>();

    int pigeonAmountInScene;

    public Action<int> OnPigeonsAmountChanged;

    private void Start()
    {
        GetList();
    }

    public void SpawnPigeon()
    {
        GameObject pigeon = GetInativePigeon();
        if (pigeon != null)
        {
            pigeon.transform.position = GetRandomPosition();
            pigeon.SetActive(true);
            pigeonAmountInScene++;
            OnPigeonsAmountChanged?.Invoke(pigeonAmountInScene);
        }
    }

    void GetList()
    {
        pigeons.Clear();

        pigeons = pooling.ObjectPool("pigeon");
    }

    GameObject GetInativePigeon()
    {
        return pigeons.Find(obj => !obj.activeInHierarchy);
    }

    GameObject GetAtivePigeon()
    {
        return pigeons.Find(obj => obj.activeInHierarchy);
    }

    Vector2 GetRandomPosition()
    {
        Bounds bounds = collider2.bounds;
        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x); //usando UnityEngine.Random para não gerar error de ambiguidade com System.Random
        float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }

    public bool CanSpawnPigeon()
    {
        return GetInativePigeon() != null;  // Tem pombo inativo no pool
    }

    public void DespawnRandomPigeon()
    {
        GameObject activePigeon = pigeons.Find(obj => obj.activeInHierarchy);
        if (activePigeon != null)
        {
            activePigeon.SetActive(false);
            pigeonAmountInScene--;
            OnPigeonsAmountChanged?.Invoke(pigeonAmountInScene);
        }
    }

}
