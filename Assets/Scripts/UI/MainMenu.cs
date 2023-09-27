using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _winsAmt;
    [SerializeField] private TextMeshProUGUI _lossesAmt;

    private void Awake()
    {
        _winsAmt.text = PlayerWinLossTracker.GetPlayerWins().ToString();
        _lossesAmt.text = PlayerWinLossTracker.GetPlayerLosses().ToString();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
