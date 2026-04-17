using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;

    [Header("Components")]
    [SerializeField] GameObject droppingsPrefab;
    [SerializeField] int droppingsAmount;
    [SerializeField] Transform droppingsContainer;
    [Space(10)]
    [SerializeField] GameObject pigeonPrefab;
    [SerializeField] int pigeonAmount;
    [SerializeField] Transform pigeonContainer;
    [Space(10)]
    [SerializeField] GameObject collectablePrefab;
    [SerializeField] int collectableAmount;
    [SerializeField] Transform collectableContainer;

    public Dictionary<string, List<GameObject>> poolDictionary = new Dictionary<string, List<GameObject>>();

    void Awake()
    {
        poolDictionary.Clear();

        Pool(droppingsPrefab, "droppings", droppingsAmount, droppingsContainer);
        Pool(pigeonPrefab, "pigeon", pigeonAmount, pigeonContainer);
        Pool(collectablePrefab, "collectable", collectableAmount, collectableContainer);
    }

    void Pool(GameObject obj, string name, int amount, Transform container)
    {
        if(!poolDictionary.ContainsKey(name))
        {
            poolDictionary.Add(name, new List<GameObject>());
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject newObj = Instantiate(obj);
            
            if(newObj.GetComponent<ObjMove>() != null)
            {
                newObj.GetComponent<ObjMove>().playerManager = playerManager;
            }
            if(newObj.GetComponent<Pigeon>() != null)
            {
                newObj.GetComponent<Pigeon>().playerManager = playerManager;
            }
            newObj.transform.SetParent(container);
            poolDictionary[name].Add(newObj);

            newObj.SetActive(false);
        }
    }

    public List<GameObject> ObjectPool(string name)
    {
        return poolDictionary[name];
    }
}
