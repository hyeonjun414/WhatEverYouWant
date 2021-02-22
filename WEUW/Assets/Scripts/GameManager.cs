using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ
    private static GameManager instance;

    public static GameManager Instance
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

    #region º¯¼ö
    
    public string keyword;
    public SpriteRenderer[] wordObject;

    public Sprite[] sprites;
    public Dictionary<string, Sprite> dictionary = new Dictionary<string, Sprite>();
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("wordimage/keybtn");
        for(int i = 0; i<sprites.Length; i++)
        {
            dictionary[sprites[i].name] = sprites[i]; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    void selectTest()
    {
        keyword = WordManager.Instance.SelectWord();
        for(int i = 0; i<wordObject.Length; i++)
        {
            int j;
            for(j = 27; j<sprites.Length; j++)
            {
                if (("W_" + keyword[i].ToString()+ " (UnityEngine.Sprite)") == sprites[j].ToString())
                    break;
            }
            wordObject[i].sprite = sprites[j];
        }
    }
    
}
