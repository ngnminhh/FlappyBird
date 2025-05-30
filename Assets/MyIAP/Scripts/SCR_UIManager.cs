using System;
using UnityEngine;
using UnityEngine.UI;

public class SCR_UIManager : MonoBehaviour
{
    [SerializeField] public GameObject PFB_StorePopup;

    [SerializeField] public Button btnStore;

    // Các UI khác cần ẩn khi mở Store
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject highScoreText;

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

        ON_STORE_BUTTON_CLICKED?.Invoke();
        PFB_StorePopup.transform.SetAsLastSibling();
        PFB_StorePopup.SetActive(true);
        
    }

    public void HideStore()
    {
        ON_STORE_CLOSED?.Invoke();
        PFB_StorePopup.SetActive(false);

        // Hiện lại các UI khi đóng Store
        panelGameOver?.SetActive(true);
        playButton?.SetActive(true);
        highScoreText?.SetActive(true);
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
