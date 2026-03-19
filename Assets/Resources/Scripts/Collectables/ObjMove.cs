using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMove : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float timeToLive = 5f;

    public PlayerManager playerManager;

    float t;

    bool isPaused;

    private void Awake()
    {
        playerManager = GameManager.Instance._playerManager;
    }

    private void OnEnable()
    {
        playerManager.OnPauseGame += (bool value) => isPaused = value;
        playerManager.OnGameOver += () => isPaused = true;
    }
    private void OnDisable()
    {
        playerManager.OnPauseGame -= (bool value) => isPaused = value;
        playerManager.OnGameOver -= () => isPaused = true;
    }

    void Update()
    {
        if (isPaused) return;

        t += Time.deltaTime;

        transform.position += Vector3.down * speed * Time.deltaTime;

        if (t >= timeToLive)
        {
            t = 0f;
            gameObject.SetActive(false);
        }
    }

    private void OnBecameInvisible()
    {
        t = 0f;
        
        gameObject.SetActive(false);
    }
}
