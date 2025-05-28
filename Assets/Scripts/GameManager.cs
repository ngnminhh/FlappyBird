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

    private int score;
    private int highScore;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreText != null)
            highScoreText.gameObject.SetActive(false);
        else
            Debug.LogError("highScoreText is not assigned!");

        Pause();
    }

    public void Play()
    {
        // Debug logs để kiểm tra null
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
            player.transform.position = Vector3.zero;
            typeof(Player).GetField("direction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(player, Vector3.zero);
        }
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
            typeof(Player).GetField("direction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(player, Vector3.zero);
        }
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

        const string currencyID = "clam"; // đảm bảo dùng đúng ID từ DB
        int currentClam = DBManager.GetCurrency(currencyID);
        Debug.Log($"Số {currencyID} hiện tại: {currentClam}");

        if (currentClam < DEFAULT_PRICE_PER_REPLAY)
        {
            Debug.Log($"Không đủ {currencyID}, bạn đang có {currentClam}");
            // Nếu có UI popup thông báo, gọi ở đây
            // uIManager.ShowPopupNotEnough();
            return;
        }

        // Trừ 1 clam không ghi DB nếu cần: DBManager.ConsumeCurrencyNoSave(currencyID, DEFAULT_PRICE_PER_REPLAY);
        DBManager.ConsumeCurrency(currencyID, DEFAULT_PRICE_PER_REPLAY);

        int afterDeduct = DBManager.GetCurrency(currencyID);
        Debug.Log($"Đã trừ {DEFAULT_PRICE_PER_REPLAY} {currencyID}, còn lại: {afterDeduct}");

        ContinueGame();
    }
}
