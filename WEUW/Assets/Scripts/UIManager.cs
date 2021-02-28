using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static UIManager instance;

    private void Awake()
    {
        if(instance)
        {
            DestroyImmediate(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public Text scoreTxt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScore(int score)
    {
        scoreTxt.text = "SCORE : " + score.ToString();
    }
}
