using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    MaxHealth,
    Heal,
    Coin
}

public class Collectables : MonoBehaviour
{
    CollectableType type;


    [Header("Components")]
    [Tooltip("index 0 = MaxHealth, index 1 = Heal, index 2 = Coin"),
        SerializeField] GameObject[] Objects;

    [Header("Settings")]
    [SerializeField] int maxHealthValue = 1;
    [SerializeField] int healValue = 1;
    [SerializeField] int coinValue = 1;

    #region API

    public void SetType(CollectableType t)
    {
        type = t;

        foreach (var obj in Objects) { obj.SetActive(false); }

        switch (type)
        {
            case CollectableType.MaxHealth:
                Objects[0].SetActive(true);
                break;
            case CollectableType.Heal:
                Objects[1].SetActive(true);
                break;
            case CollectableType.Coin:
                Objects[2].SetActive(true);
                break;
        }

    }

    #endregion

    #region Collider

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager p = collision.gameObject.GetComponent<PlayerManager>();
            if(p == null)
            {
                gameObject.SetActive(false);
                return;
            }

            int value = type switch
            {
                CollectableType.MaxHealth => maxHealthValue,
                CollectableType.Heal => healValue,
                CollectableType.Coin => coinValue,
                _ => 0
            };

            p.CollectionObject(type, value);

            gameObject.SetActive(false);
        }
    }
    #endregion
}
