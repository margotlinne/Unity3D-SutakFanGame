using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConvoManager : MonoBehaviour
{
    public static ConvoManager instance;
    GameManager gameManager;

    [HideInInspector]
    public bool clickToTalk;
    [HideInInspector]
    public Transform target;

    public bool isTalking = false;

    public GameObject convoCanvas;
    public TextMeshProUGUI convoTxt;
    public TextMeshProUGUI npcNameTxt;

    private int count = 0;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this.gameObject); }
    }

    void Start()
    {
        gameManager = GameManager.instance;

    }

    void Update()
    {
        if (isTalking)
        {
            if (target.gameObject.tag == "Amtak")
            {
                npcNameTxt.text = "Amtak";
            }


            if (count == 0 || Input.GetKeyDown(KeyCode.Space))
            {
                convoCanvas.SetActive(true);
                Debug.Log(count);
                nextConvo("Amtak", count);
                Debug.Log("next" + count);
            }
        }
        else
        {
            convoCanvas.SetActive(false);
        }
    }

    void nextConvo(string name, int num)
    {
        Debug.Log("함수 호출");
        if (name == "Amtak")
        {
            if (!gameManager.acceptedQuest && !gameManager.firstConvoDone)
            {
                Debug.Log("첫 번째 대화 페이즈");
                // 첫 번째 페이즈에서 대화 갯수
                if (num < gameManager.convoData.datas.FirstTalk.Length)
                {
                    convoTxt.text = gameManager.convoData.datas.FirstTalk[num].convo;
                    count++;
                }
                else 
                {
                    isTalking = false;
                    gameManager.firstConvoDone = true;
                    // 매개변수 이름과 같아서 함수 밖의 count값이 조정되는 게 아니었음..
                    count = 0;
                    Debug.Log("카운트 값 0으로 리셋");
                }
            }
            else if (gameManager.firstConvoDone && !gameManager.acceptedQuest)
            {
                Debug.Log("두 번째 대화 페이즈");
                // 두 번째 페이즈에서 대화 갯수
                if (num < gameManager.convoData.datas.SecondTalk.Length)
                {
                    convoTxt.text = gameManager.convoData.datas.SecondTalk[num].convo;
                    count++;
                }
                else
                {
                    isTalking = false;
                    gameManager.acceptedQuest = true;
                    count = 0;
                    Debug.Log("카운트 값 0으로 리셋");
                }
            }
            else if (gameManager.getReward)
            {
                Debug.Log("세 번째 대화 페이즈");
                if (num < gameManager.convoData.datas.RewardTalk.Length)
                {
                    convoTxt.text = gameManager.convoData.datas.RewardTalk[num].convo;
                    count++;
                }
                else
                {
                    isTalking = false;
                    gameManager.getReward = false;
                    count = 0;
                    Debug.Log("카운트 값 0으로 리셋");
                }
            }
        }
    }
}
