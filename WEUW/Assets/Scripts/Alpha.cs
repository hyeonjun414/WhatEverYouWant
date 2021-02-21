using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Alpha : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMoveY(0.5f, 1).SetLoops(-1, LoopType.Yoyo);
    }
}
