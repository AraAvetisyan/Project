using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SignScale : MonoBehaviour
{
    private Vector2 toScale;
    [SerializeField] private Ease ease;
    

    public void Scale()
    {

        toScale = new Vector2(1f, 1f);
        transform.DOScale(toScale, 1.5f).SetEase(ease);
    }

    
}
