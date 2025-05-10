using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject bgInitial, bgScenario;
    public GameObject uiInitial, uiGameOver;
    public Transform livesPanel;
    public GameObject zombieSpawner;
    public PlayerController player;

    private bool isPlaying;

    void Start()
    {
        ShowInitial();
    }

    void Update()
    {
        // Inicia ao clicar/tocar em qualquer lugar
        if (!isPlaying && Input.anyKeyDown)
            StartGame();
    }

    void ShowInitial()
    {
        isPlaying = false;
        bgInitial.SetActive(true);
        uiInitial.SetActive(true);
        bgScenario.SetActive(false);
        uiGameOver.SetActive(false);
        livesPanel.gameObject.SetActive(false);
        zombieSpawner.SetActive(false);
    }

    void StartGame()
    {
        isPlaying = true;
        bgInitial.SetActive(false);
        uiInitial.SetActive(false);
        bgScenario.SetActive(true);
        livesPanel.gameObject.SetActive(true);
        livesPanel.GetComponent<LivesController>().ResetLives();
        zombieSpawner.SetActive(true);
        player.Init(this);
    }

    public void PlayerDead()
    {
        // Para lógica de zumbis
        zombieSpawner.SetActive(false);
        player.Die();
        StartCoroutine(ShowGameOver());
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(2f);
        uiGameOver.SetActive(true);
        yield return new WaitForSeconds(3f);
        ShowInitial();
        player.ResetPlayer();
    }
}
