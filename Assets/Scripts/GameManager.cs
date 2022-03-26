using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int Bamboo;
    [SerializeField]
    private TextMeshProUGUI BambooText, TimeRemainingText, GameEndingText;

    private float TimeRemaining;

    private int WinState = 0;

    public GameObject RestartButton, QuitButton;

    [SerializeField]
    private AudioSource EatingSoundFX;

    public int GetWinState()
    {
        return WinState;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayEatingSoundFX()
    {
        EatingSoundFX.Play();
    }

    public void ChangeBamboo(int value)
    {
        Bamboo += value;
        if ( Bamboo > 4 )
        {
            WinState = 1;
        }
        UpdateGUI();
    }

    public void UpdateGUI()
    {
        BambooText.text = ($"Bamboo: {Bamboo}");
        if ( WinState == 1 )
            GameEndingText.text = "You win!";
    }
    void Start()
    {
        BambooText.text = "Bamboo: 0";
        TimeRemaining = 30f;
        GameEndingText.text = "";
        RestartButton.SetActive(false);
        QuitButton.SetActive(false);
    }

    void Update()
    {
        if ( WinState == 0 )
        {
            TimeRemaining -= Time.deltaTime;
            int TimeRemainingInt = (int) TimeRemaining;
            TimeRemainingText.text = $"Time remaining: {TimeRemainingInt.ToString()}";
        }
        if ( WinState == 0 && TimeRemaining <= 0 )
        {
            WinState = -1;
            GameEndingText.text = "You lose!";
        }
        if ( WinState != 0 )
        {
            RestartButton.SetActive(true);
            QuitButton.SetActive(true);
        }
        
    }
}
