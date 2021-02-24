using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Alpha : MonoBehaviour
{
    public string word = null;
    public SpriteRenderer sr = null;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.DOLocalMoveY(0.25f, 1).SetLoops(-1, LoopType.Yoyo);
    }

    public void End()
    {
        Destroy(gameObject);
    }
}
