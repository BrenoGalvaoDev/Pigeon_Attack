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

    [Header("UI Elements")]
    [SerializeField] Text ScoreTxt;
    [SerializeField] Text MaxScoreTxt;
    [SerializeField] Text CoinTxt;

    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject PausePanel;


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
    }
    private void OnDisable()
    {
        playerManager.OnCollectedCoin -= UpdateCoinTxt;
        playerManager.OnGameOver -= () => GameOverPanel.SetActive(true);
        playerManager.OnPauseGame -= (bool value) => PausePanel.SetActive(value);
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
}
