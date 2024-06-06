using GeekplaySchool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private bool onMobile;
    [SerializeField] private GameObject imageRu, imageTr, imageEn;
    [SerializeField] private AudioSource audioSource;
    private void Start()
    {
        Geekplay.Instance.ShowInterstitialAd();
        if (Geekplay.Instance.language == "ru")
        {
            imageRu.SetActive(true);
            imageTr.SetActive(false);
            imageEn.SetActive(false);
        }
        else if (Geekplay.Instance.language == "en")
        {
            imageRu.SetActive(false);
            imageTr.SetActive(false);
            imageEn.SetActive(true);
        }
        else if (Geekplay.Instance.language == "tr")
        {
            imageRu.SetActive(false);
            imageTr.SetActive(true);
            imageEn.SetActive(false);
        }
        if (Geekplay.Instance.mobile)
        {
            onMobile = true;
        }
        else
        {
            onMobile = false;
        }
    }
    public void PressedPlayButton()
    {
        audioSource.Play();
        if (onMobile)
        {
            SceneManager.LoadScene("GameSceneMobile");
        }
        else
        {
            SceneManager.LoadScene("GameScenePC");
        }

    }
}
