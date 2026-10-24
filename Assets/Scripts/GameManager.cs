using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject menuPanel;          
    public TextMeshProUGUI titleText;     
    public GameObject resumeButton;       
    private bool isPaused = false;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (isGameOver) return;
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; 
        titleText.text = "PAUSED";
        resumeButton.SetActive(true); 
        menuPanel.SetActive(true);    
    }


    public void Resume()
    {
        StartCoroutine(ResumeRoutine());
    }
    private IEnumerator ResumeRoutine()
    {
        menuPanel.SetActive(false); 
        yield return null;          
        isPaused = false;
        Time.timeScale = 1f;        
    }

    public void Restart()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenuScene");
    }

    public void GameOver(bool isWin)
    {
        if (isGameOver) return;
        isGameOver = true;
        StartCoroutine(ShowGameOverRoutine(isWin));
    }

    private IEnumerator ShowGameOverRoutine(bool isWin)
    {
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0f; 
        if(isWin)
        {
            titleText.text = "VICTORY";
        }
        else
        {
            titleText.text = "YOU DIED";
        }
        resumeButton.SetActive(false); 
        menuPanel.SetActive(true);   
    }
}