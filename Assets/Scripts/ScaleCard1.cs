using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ScaleCard1 : MonoBehaviour
{
    [SerializeField] private ÑompareScript _ñompareScript;
    private Vector2 smollScale;
    private Vector2 originalScale;
    public bool StartScaling = false;
    public bool GetSmoller = false;
    private bool readyToPlay;

    [SerializeField] private Ease easeToBig;
    [SerializeField] private Ease easeToSmoll;

    void Start()
    {
        smollScale = new Vector2(0f, 0f);
        originalScale = transform.localScale;      
        
    }
    public void Update()
    {
        

        if (StartScaling)
        {
            StartScaling = false;
            transform.DOScale(originalScale, 1f).SetEase(easeToBig);
            StartCoroutine(WaitToPlay());
        }

        if (GetSmoller)
        {
            GetSmoller = false;
            transform.DOScale(smollScale, 1f).SetEase(easeToSmoll);
            StartCoroutine(WaitToSmoll());

        }
        if (transform.localScale.x == smollScale.x)
        {
            StartScaling = true;
        }
        if (readyToPlay)
        {
            readyToPlay = false;
            _ñompareScript.card1Button.interactable = true;
            _ñompareScript.card2Button.interactable = true;
        }
    }
    public IEnumerator WaitToPlay()
    {
        yield return new WaitForSecondsRealtime(1f);
        readyToPlay = true;
    }
    public IEnumerator WaitToSmoll()
    {
        yield return new WaitForSecondsRealtime(1f);
        _ñompareScript.Card1GetSmoled = true;
    }
}
