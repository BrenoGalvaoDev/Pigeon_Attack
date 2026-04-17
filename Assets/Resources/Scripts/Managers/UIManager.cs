using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] PlayerLife playerLife;

    [Header("UI Elements")]
    [SerializeField] Text ScoreTxt;
    [SerializeField] Text MaxScoreTxt;
    [SerializeField] Text CoinTxt;

    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject PausePanel;

    [Header("Life Image System")]
    [SerializeField] Image[] maxLifeImages;
    [SerializeField] Image[] fullLifeImages;


    #region DATA

    #endregion

    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region EVENTS

    private void OnEnable()
    {
        playerManager.OnCollectedCoin += UpdateCoinTxt;
        playerManager.OnGameOver += () => GameOverPanel.SetActive(true);
        playerManager.OnPauseGame += (bool value) => PausePanel.SetActive(value);

        playerLife.OnHealthChanged += UpdateLifeImages;
    }
    private void OnDisable()
    {
        playerManager.OnCollectedCoin -= UpdateCoinTxt;
        playerManager.OnGameOver -= () => GameOverPanel.SetActive(true);
        playerManager.OnPauseGame -= (bool value) => PausePanel.SetActive(value);

        playerLife.OnHealthChanged -= UpdateLifeImages;
    }

    #endregion

    #region UI UPDATE

    public void UpdateScoreTxt(float currentScore, float maxScore)
    {
        ScoreTxt.text = currentScore.ToString();
        MaxScoreTxt.text = maxScore.ToString();
    }

    void UpdateCoinTxt(int coinAmount)
    {
        CoinTxt.text = coinAmount.ToString();
    }

    #endregion

    #region Life System
    public void UpdateLifeImages(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < maxLifeImages.Length; i++)
        {
            maxLifeImages[i].enabled = i < maxHealth;
            fullLifeImages[i].enabled = i < currentHealth;
        }
    }
    #endregion
}
