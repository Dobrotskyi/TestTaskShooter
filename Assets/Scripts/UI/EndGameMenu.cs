using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    public static event Action GameHasEnded;
    private enum Status
    {
        Won,
        Lost
    }
    private Status _status;

    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private GameObject _body;

    private List<Health> _enemies = new();
    private int _enemiesKilled = 0;
    private Health _playerHealth;

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Awake()
    {
        _enemies = new List<Health>(FindObjectsOfType<Health>().Where(t => t.CompareTag("Enemy")));
        _playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();

        _playerHealth.ZeroHealth += PlayerLost;
        for (int i = 0; i < _enemies.Count; i++)
            _enemies[i].ZeroHealth += UpdateEnemiesDeadCount;
        if (_body.activeSelf)
            _body.SetActive(false);
    }

    private void OnDestroy()
    {
        _playerHealth.ZeroHealth -= PlayerLost;
        for (int i = 0; i < _enemies.Count; i++)
            _enemies[i].ZeroHealth -= UpdateEnemiesDeadCount;
    }

    private void UpdateEnemiesDeadCount()
    {
        _enemiesKilled++;
        if (_enemiesKilled == _enemies.Count)
        {
            _status = Status.Won;
            ShowGameOverScreen();
        }
    }

    private void PlayerLost()
    {
        _status = Status.Lost;
        ShowGameOverScreen();
    }

    private void ShowGameOverScreen()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        string title = "";
        if (_status == Status.Lost)
            title = "YOU LOST!";
        else
            if (_status == Status.Won)
            title = "YOU WIN!";

        _title.text = title;
        _body.SetActive(true);
        GameHasEnded?.Invoke();
    }
}
