using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PortraitCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHover = false;
    

    public int id = -1;
    public string unitName;
    public string unitDamage;
    public string unitResistance;
    public string unitInitiative;
    public string unitTrait;

    GameManager gameManager;
    Outline outline;
    Color outlineColor;

    // 두 클릭 사이의 최대 허용 시간
    public float doubleClickTime = 0.5f;

    // 마지막 클릭이 발생한 시간
    private float lastClickTime = 0f;

    void Awake()
    {
        outline = GetComponent<Outline>();
        outlineColor = outline.effectColor;
    }

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (isHover)
        {
            outline.enabled = true;
            outline.effectColor = outlineColor;
            gameManager.battleManager.idHoverOnCard = id;


            // 마우스 왼쪽 버튼 클릭을 감지
            if (Input.GetMouseButtonDown(0))
            {
                // 현재 시간
                float timeSinceLastClick = Time.time - lastClickTime;

                // 두 번째 클릭이 doubleClickTime 내에 발생했는지 확인
                if (timeSinceLastClick <= doubleClickTime)
                {
                    // 더블클릭으로 간주
                    OnDoubleClick();
                }

                // 마지막 클릭 시간 업데이트
                lastClickTime = Time.time;
            }

            if (Input.GetMouseButtonDown(1))
            {
                gameManager.battleManager.showInfoWindow("", "", unitInitiative, "", "");
            }
        }
        else if (!isHover)
        {
            if (gameManager.battleManager.idHoverOnCharacter == id)
            {
                outline.enabled = true;
                outline.effectColor = Color.black;
            }
            else
            {
                outline.enabled = false;
            }
        }
    }

    void OnDoubleClick()
    {
        gameManager.battleManager.idDoubleClick = id;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;

        gameManager.battleManager.idHoverOnCard = -1;
    }
}
