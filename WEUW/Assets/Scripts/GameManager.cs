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
    public GameObject wordUI;
    public GameObject answer;
    public int selectCount = 0;
    public Sprite question;
    public GameObject pos;
    public GameObject wordPrefab;

    public List<GameObject> wordObjectList;
    public Transform[] randomPos;
    public Alpha[] wordObject;
    public Image[] selectWordImage;
    public Image[] answerWordImage;
    public Sprite[] sprites;
    public Dictionary<string, Sprite> dictionary = new Dictionary<string, Sprite>();


    public int score = 0;
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
        randomPos = pos.GetComponentsInChildren<Transform>();
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

    [Button]
    void GenerateWord()
    {
        keyword = WordManager.Instance.SelectWord();
        if (wordObjectList != null)
        {
            for (int i = 0; i < wordObjectList.Count; i++)
                Destroy(wordObjectList[i].gameObject);
            wordObjectList.Clear();
        }

        for (int i = 1; i < randomPos.Length; i++)
        {
            GameObject word = Instantiate(wordPrefab, randomPos[i].position, Quaternion.identity);
            wordObjectList.Add(word);
        }
        int count = 0;
        for(int i = 0; i< 2; i++)
        {
            string alpha = WordManager.Instance.WordList[i];
            for(int j = 0; j<alpha.Length; j++)
            {
                Alpha word = wordObjectList[count].GetComponent<Alpha>();
                word.word = alpha[j].ToString();
                for (int wordspriteidx = 27; j < sprites.Length; wordspriteidx++)
                {
                    if (("W_" + word.word.ToString() + " (UnityEngine.Sprite)") == sprites[wordspriteidx].ToString())
                    {
                        word.sr.sprite = sprites[wordspriteidx];
                        break;
                    }
                }
                count++;
            }
        }

        for(int i = 0; i<10; i++)
        {
            int num = Random.Range(0, wordObjectList.Count);
            Vector3 pos = wordObjectList[i].transform.position;
            wordObjectList[i].transform.position = wordObjectList[num].transform.position;
            wordObjectList[num].transform.position = pos;

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
        int answerCount=0;
        for(int i = 0; i<selectWord.Length; i++)
        {
            if(selectWord[i] == keyword[i])
            {
                answerWordImage[i].sprite = selectWordImage[i].sprite;
                answerCount++;
            }
            else
            {
                answerWordImage[i].sprite = question;
            }
            selectWordImage[i].sprite = question;

        }
        if(answerCount == 5)
        {
            score++;
            UIManager.Instance.UpdateScore(score);
            GenerateWord();
            ResetAnswerWord();
        }
        selectWord = null;
        selectCount = 0;
    }

    void ResetAnswerWord()
    {
        for (int i = 0; i < answerWordImage.Length; i++)
            answerWordImage[i].sprite = question;
    }


    #endregion
}
