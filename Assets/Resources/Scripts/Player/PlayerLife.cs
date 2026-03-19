using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public Action OnPlayerDeath;
    public Action OnPlayerTakeHit;
    public Action <int, int> OnHealthChanged;

    [Header("Player Life Settings")]
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    [SerializeField] float invincibilityTime = 1f;

    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

    private void Start()
    {

    }

    #region EXTERNAL CALLS

    public void SetMaxHeath(int value)
    {
        maxHealth += value;
        currentHealth = maxHealth;
    }


    #endregion

    #region Collsion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            TakeHit(1);
        }
    }

    #endregion

    #region Hit & Death

    public void TakeHit(int damage)
    {
        currentHealth -= damage;

        if (currentHealth > 0)
        {
            OnPlayerTakeHit?.Invoke();
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            StartCoroutine(Invincibility());
        }
        else
        {
            Death();
        }
    }

    public void Death()
    {
        currentHealth = 0;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnPlayerDeath?.Invoke();
    }

    #endregion

    #region Heal

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void UpgradeMaxHeath()
    {
        maxHealth++;
        currentHealth = maxHealth;
    }
    #endregion

    #region Coroutines

    IEnumerator Invincibility()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        yield return new WaitForSeconds(invincibilityTime);
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    #endregion
}
