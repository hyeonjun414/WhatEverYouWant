using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager: MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static WordManager instance;

    public static WordManager Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(this.gameObject);
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public List<string> WordList = new List<string>();

    private void Start()
    {
        WordList.Add("APPLE");
        WordList.Add("WATER");
    }

    public string SelectWord()
    {
        string str = WordList[Random.Range(0, WordList.Count)];
        return str;
    }
}
