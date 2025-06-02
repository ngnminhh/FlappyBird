using System;
using UnityEngine;
using UnityEngine.UI;

public class SCR_UIManager : MonoBehaviour
{
    // Key dùng để kiểm tra lần chơi đầu
    const string FIRST_PLAY_KEY = "HasPlayedBefore";

    [SerializeField] public GameObject PFB_StorePopup;

    [SerializeField] public Button btnStore;

    // Các UI khác cần ẩn khi mở Store
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject highScoreText;
    [SerializeField] private GameObject titleBG;
    [SerializeField] private GameObject pauseBTN;


    public Action ON_STORE_BUTTON_CLICKED;
    public Action ON_STORE_CLOSED;

    void Start()
    {
        PFB_StorePopup.SetActive(false);
    }

    public void ShowStore()
    {
        // Ẩn các UI khác
        panelGameOver?.SetActive(false);
        playButton?.SetActive(false);
        highScoreText?.SetActive(false);
        titleBG.SetActive(false);
        pauseBTN?.SetActive(false);
        ON_STORE_BUTTON_CLICKED?.Invoke();
        PFB_StorePopup.transform.SetAsLastSibling();
        PFB_StorePopup.SetActive(true);
        
    }

    public void HideStore()
    {
        ON_STORE_CLOSED?.Invoke();
        PFB_StorePopup.SetActive(false);

        // Hiện lại các UI khi đóng Store
        titleBG.SetActive(true);
        playButton?.SetActive(true);
        highScoreText?.SetActive(true);

        // Kiểm tra nếu là người mới thì không hiển thị GameOver
        bool isFirstTimePlayer = PlayerPrefs.GetInt("HasPlayedBefore", 0) == 0;

        if (isFirstTimePlayer)
        {
            // Ẩn panelGameOver nếu người mới
            panelGameOver?.SetActive(false);
            Debug.Log("Lần đầu chơi - Không hiển thị GameOver");
        }
        else
        {
            panelGameOver?.SetActive(true);
            Debug.Log("Đã chơi trước đó - Hiện GameOver");
        }
    }



    public void ShowStoreButton()
    {
        btnStore.gameObject.SetActive(true);
    }

    public void HideStoreButton()
    {
        btnStore.gameObject.SetActive(false);
    }
}
