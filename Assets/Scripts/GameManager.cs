using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Text scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject Play_BTN;
    public GameObject gameOver;
    public GameObject pause_BTN;
    public GameObject pausePanel;
    public GameObject titleBG;
    public int DEFAULT_PRICE_PER_REPLAY = 1;
    public GameObject tutorialPanel;
    public GameObject tutorial_BTN;
    public AudioManager audioManager;
    

    public AudioClip sfxDefeatAudioClip;
    private AudioSource defeatSfxSource;

    public static GameManager Instance { get; private set; }

    public int CurrentScore => score;
    private int score;
    private int highScore;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (highScoreText != null)
            highScoreText.gameObject.SetActive(false);
        else
            Debug.LogError("highScoreText is not assigned!");
        if (gameOver != null)
        {
            gameOver.SetActive(false);
            Debug.Log("GameOver UI đã bị ẩn thành công.");
        }
        else
        {                                   
            Debug.LogError("gameOver UI chưa được gán!");
        }
        defeatSfxSource = GetComponent<AudioSource>();
        if (defeatSfxSource == null)
            defeatSfxSource = gameObject.AddComponent<AudioSource>();

        Pause();
        ShowTutorialIfFirstTime();
    }
    private void Start()
    {
        pause_BTN.SetActive(false);

        pausePanel.SetActive(false);
        if (gameOver != null)
        {
            gameOver.SetActive(false);
            Debug.Log("GameOver UI đã bị ẩn trong Start().");
        }
        else
        {
            Debug.LogError("gameOver UI chưa được gán!");
        }
    }
    public void ShowTutorialIfFirstTime()
    {
        if (!PlayerPrefs.HasKey("HasSeenTutorial"))
        {
            if (tutorialPanel != null)
            {
                
                pause_BTN.SetActive(false);
                tutorialPanel.SetActive(true);
                PlayerPrefs.SetInt("HasSeenTutorial", 1);
                PlayerPrefs.Save();

                
            }
            else
            {
                Debug.LogError("tutorialPanel is not assigned!");
            }
        }
        else
        {
            if (tutorialPanel != null)
                tutorialPanel.SetActive(false);
        }
    }
    public void ShowTutorial()
    {
        if (tutorialPanel != null)
        {
           
            tutorialPanel.SetActive(true);
            pause_BTN.SetActive(false);
            tutorial_BTN.SetActive(false);
            Animator[] anims = tutorialPanel.GetComponentsInChildren<Animator>(true);
            foreach (var anim in anims)
            {
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
                anim.enabled = true;
            }
        }

        
    }

  

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
       
        tutorial_BTN.SetActive(true);

    }

    public void Play()
    {
        if (Play_BTN == null || gameOver == null || scoreText == null)
        {
            Debug.LogError("Play_BTN, gameOver hoặc scoreText chưa được gán trong Inspector!");
            return;
        }
        titleBG.SetActive(false);
        score = 0;
        scoreText.text = score.ToString();
        Play_BTN.SetActive(false);
        gameOver.SetActive(false);
        
        tutorialPanel.SetActive(false);
        tutorial_BTN.SetActive(false);
        pausePanel.SetActive(false);
        pause_BTN.SetActive(true);
        if (highScoreText != null)
            highScoreText.gameObject.SetActive(false);

        Time.timeScale = 1f;

        if (player != null)
        {
            player.ResetPlayer(); // Gọi reset player
            player.enabled = true;
        }
        else
        {
            Debug.LogError("player is not assigned!");
        }

        foreach (var bom in FindObjectsOfType<Boms>())
        {
            Destroy(bom.gameObject);
        }

        if (audioManager != null && audioManager.musicAudioSource != null)
            audioManager.musicAudioSource.Play();
    }

    public void ContinueGame()
    {
        if (Play_BTN != null) Play_BTN.SetActive(false);
        if (gameOver != null) gameOver.SetActive(false);
        if (highScoreText != null) highScoreText.gameObject.SetActive(false);
        pause_BTN.SetActive (true);
        Time.timeScale = 1f;

        if (player != null)
        {
            player.ResetPlayer();
            player.enabled = true;
        }

        if (audioManager != null && audioManager.musicAudioSource != null)
            audioManager.musicAudioSource.Play();

    }

    public void Pause()
    {
        Time.timeScale = 0f;
        if (player != null)
            player.enabled = false;
        pausePanel.SetActive(true);
        if (audioManager != null && audioManager.musicAudioSource != null)
            audioManager.musicAudioSource.Stop();
    }
    public void Pause_gameover()
    {

        Time.timeScale = 0f;
        if (player != null)
            player.enabled = false;
        pausePanel.SetActive(false);
        if (audioManager != null && audioManager.musicAudioSource != null)
            audioManager.musicAudioSource.Stop();
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        if (player != null)
            player.enabled = true;
        pausePanel.SetActive(false);
    }

    public void GameOver()
    {
       
        StartCoroutine(GameOverRoutine());
        pausePanel.SetActive(false);
        pause_BTN.SetActive(false);
    }
    public void HomeGame()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
            highScoreText.gameObject.SetActive(true);
        }

        if (gameOver != null) gameOver.SetActive(false);
        if (Play_BTN != null) Play_BTN.SetActive(true);

        pause_BTN.SetActive(false);
        tutorial_BTN.SetActive(true);
        
        pausePanel.SetActive(false);
        titleBG.SetActive(true);
        if (audioManager != null && audioManager.musicAudioSource != null)
            audioManager.musicAudioSource.Play();
    }

    private IEnumerator GameOverRoutine()
    {
        
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
            highScoreText.gameObject.SetActive(true);
        }

        if (gameOver != null) gameOver.SetActive(true);
        if (Play_BTN != null) Play_BTN.SetActive(true);

        if (audioManager != null && audioManager.musicAudioSource != null)
            audioManager.musicAudioSource.Stop();

        if (sfxDefeatAudioClip != null && defeatSfxSource != null)
            defeatSfxSource.PlayOneShot(sfxDefeatAudioClip);

        yield return new WaitForSeconds(1f);
        Pause_gameover();
    }

    public void IncreaseScore()
    {
        score++;
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    
}