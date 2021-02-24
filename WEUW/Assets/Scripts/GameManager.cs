using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public string selectWord = null;
    public string keyword;
    public Alpha[] wordObject;
    public GameObject wordUI;
    public GameObject answer;
    public Image[] selectWordImage;
    public Image[] answerWordImage;
    public int selectCount = 0;

    public Sprite question;

    public Sprite[] sprites;
    public Dictionary<string, Sprite> dictionary = new Dictionary<string, Sprite>();
    #endregion


    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("wordimage/keybtn");
        for(int i = 0; i<sprites.Length; i++)
        {
            dictionary[sprites[i].name] = sprites[i]; 
        }

        selectWordImage = wordUI.GetComponentsInChildren<Image>();
        answerWordImage = answer.GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    #region ¸Þ¼­µå
    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.X)) RemoveOneWord();
        if (Input.GetKeyDown(KeyCode.Z)) SummitAnswer();

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
            wordObject[i].word = keyword[i].ToString();
            wordObject[i].sr.sprite = sprites[j];
        }
    }

    public void WordUIChange(Alpha word)
    {
        selectWord += word.word;
        selectWordImage[selectCount].sprite = word.sr.sprite;
        selectCount++;
    }

    public void RemoveOneWord()
    {
        if (selectWord == null) return;
        if(selectWord.Length == 1)
        {
            selectWord = null;
            selectCount = 0;
            selectWordImage[selectCount].sprite = question;
        }
        else
        {
            int textLength = selectWord.Length;
            selectWord = selectWord.Substring(0, textLength - 1);
            selectCount--;
            selectWordImage[selectCount].sprite = question;
        }
    }

    public void SummitAnswer()
    {
        for(int i = 0; i<selectWord.Length; i++)
        {
            if(selectWord[i] == keyword[i])
            {
                answerWordImage[i].sprite = selectWordImage[i].sprite;
            }
            else
            {
                answerWordImage[i].sprite = question;
            }
            selectWordImage[i].sprite = question;

        }
        selectWord = null;
        selectCount = 0;
    }
    #endregion
}
