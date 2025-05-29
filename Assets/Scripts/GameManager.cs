using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniPay;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Text scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject Play_BTN;
    public GameObject gameOver;
    public int DEFAULT_PRICE_PER_REPLAY = 1;

    public GameObject popupNotEnough;
    public GameObject tutorialPanel;
    public AudioManager audioManager;

    public SCR_UIManager uiManager; 

    
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

        // Khởi tạo AudioSource cho defeat sound
        defeatSfxSource = GetComponent<AudioSource>();
        if (defeatSfxSource == null)
            defeatSfxSource = gameObject.AddComponent<AudioSource>();

        Pause();
        ShowTutorialIfFirstTime();
    }

    public void ShowTutorialIfFirstTime()
    {
        if (!PlayerPrefs.HasKey("HasSeenTutorial"))
        {
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(true);
                PlayerPrefs.SetInt("HasSeenTutorial", 1);
                PlayerPrefs.Save();

                if (uiManager != null)
                    uiManager.HideStore();
                else
                    Debug.LogWarning("uiManager is not assigned!");
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
        }
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    public void HidePopupNotEnough()
    {
        if (popupNotEnough != null)
        {
            popupNotEnough.SetActive(false);
        }
    }

    public void ShowPopupNotEnough()
    {
        if (popupNotEnough != null)
        {
            popupNotEnough.SetActive(true);
        }
    }

    public void Play()
    {
        if (Play_BTN == null)
        {
            Debug.LogError("Play_BTN is NULL! Hãy kéo nút Play vào GameManager trong Inspector.");
            return;
        }

        if (gameOver == null)
        {
            Debug.LogError("gameOver is NULL!");
            return;
        }

        score = 0;
        if (scoreText != null)
            scoreText.text = score.ToString();
        else
            Debug.LogError("scoreText is not assigned!");

        Play_BTN.SetActive(false);
        gameOver.SetActive(false);
        if (highScoreText != null)
            highScoreText.gameObject.SetActive(false);

        Time.timeScale = 1f;

        if (player != null)
            player.enabled = true;
        else
            Debug.LogError("player is not assigned!");

        Boms[] boms = FindObjectsOfType<Boms>();
        for (int i = 0; i < boms.Length; i++)
        {
            Destroy(boms[i].gameObject);
        }

        if (player != null)
        {
            typeof(Player).GetField("direction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        }
        if (audioManager != null && audioManager.musicAudioSource != null)
            audioManager.musicAudioSource.Play();

    }

    public void ContinueGame()
    {
        if (Play_BTN != null) Play_BTN.SetActive(false);
        if (gameOver != null) gameOver.SetActive(false);
        if (highScoreText != null) highScoreText.gameObject.SetActive(false);

        Time.timeScale = 1f;
        if (player != null)
        {
            player.enabled = true;
            player.transform.position = Vector3.zero;
            typeof(Player).GetField("direction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
        }
        if (audioManager != null && audioManager.musicAudioSource != null)
            audioManager.musicAudioSource.Play();

    }

    public void Pause()
    {
        Time.timeScale = 0f;
        if (player != null)
            player.enabled = false;
    }

    public void GameOver()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore.ToString();
            highScoreText.gameObject.SetActive(true);
        }

        if (gameOver != null) gameOver.SetActive(true);
        if (Play_BTN != null) Play_BTN.SetActive(true);

        // Dừng nhạc nền
        if (audioManager != null && audioManager.musicAudioSource != null)
            audioManager.musicAudioSource.Stop();
        else
            Debug.LogWarning("AudioManager or musicAudioSource is not assigned!");

        // PHÁT THUA
        if (sfxDefeatAudioClip != null && defeatSfxSource != null)
        {
            defeatSfxSource.PlayOneShot(sfxDefeatAudioClip);
        }
        else
        {
            Debug.LogWarning("Defeat SFX or AudioSource is missing!");
        }

        
        
        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void CheckScore()
    {
        StopAllCoroutines();

        const string currencyID = "clam";
        int currentClam = DBManager.GetCurrency(currencyID);
        Debug.Log($"Số {currencyID} hiện tại: {currentClam}");

        if (currentClam < DEFAULT_PRICE_PER_REPLAY)
        {
            Debug.Log($"Không đủ {currencyID}, bạn đang có {currentClam}");
            ShowPopupNotEnough();
            return;
        }

        DBManager.ConsumeCurrency(currencyID, DEFAULT_PRICE_PER_REPLAY);

        int afterDeduct = DBManager.GetCurrency(currencyID);
        Debug.Log($"Đã trừ {DEFAULT_PRICE_PER_REPLAY} {currencyID}, còn lại: {afterDeduct}");

        ContinueGame();
    }
}
