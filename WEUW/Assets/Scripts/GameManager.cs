using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region 싱글톤
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

    #region 변수
    public string selectWord = null;
    public string keyword;
    public GameObject wordUI;
    public GameObject answer;
    public int selectCount = 0;
    public Sprite question;
    public GameObject pos;
    public GameObject wordPrefab;
    public GameObject PlayerPrefab;

    public List<GameObject> wordObjectList;
    public Transform[] randomPos;
    public Alpha[] wordObject;
    public Image[] selectWordImage;
    public Image[] answerWordImage;
    public Sprite[] sprites;
    public Dictionary<string, Sprite> dictionary = new Dictionary<string, Sprite>();


    public int score = 0;
    #endregion

    // 주기적으로 자동 실행되는 포톤 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            // 로컬오브젝트라면 쓰기 부분이 실행
            // 네트워크를 통해 score 값 보내기
            stream.SendNext(score);
        }
        else
        {
            // 리모트 오브젝트라면 읽기 부분이 실행됨
            // 네트워크를 통해 score값 받기
            score = (int)stream.ReceiveNext();
            // 동기화하여 받은 점수를 UI로 표시
            UIManager.Instance.UpdateScore(score);
        }
    }
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

        // 플레이어 생성 랜덤 위치 지정
        Vector3 randompos = new Vector3(Random.Range(-1, 2), 1f, 0f);
        // 네트워크 상의 모들 클라이언트에서 생성
        PhotonNetwork.Instantiate(PlayerPrefab.name, randompos, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    #region 메서드
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
            GameObject word = PhotonNetwork.Instantiate(wordPrefab.name, randomPos[i].position, Quaternion.identity);
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
