using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
	private	Image			image;
	private	RectTransform	rect;
	private RectTransform rectTransform;
	public DraggableUI item;
	Vector3 dir;

	public bool hasitem;
    private void Awake()
	{
		image	= GetComponent<Image>();
		rect	= GetComponent<RectTransform>();
	}


	/// <summary>
	/// 마우스 포인트가 현재 아이템 슬롯 영역 내부로 들어갈 때 1회 호출
	/// </summary>
	public void OnPointerEnter(PointerEventData eventData)
	{
		// 아이템 슬롯의 색상을 노란색으로 변경
		image.color = Color.yellow;
    }

	/// <summary>
	/// 마우스 포인트가 현재 아이템 슬롯 영역을 빠져나갈 때 1회 호출
	/// </summary>
	public void OnPointerExit(PointerEventData eventData)
	{
		// 아이템 슬롯의 색상을 하얀색으로 변경
		image.color = Color.white;
    }

        /// <summary>
        /// 현재 아이템 슬롯 영역 내부에서 드롭을 했을 때 1회 호출
        /// </summary>
        public void OnDrop(PointerEventData eventData)
	{
		// pointerDrag는 현재 드래그하고 있는 대상(=아이템)
		item = eventData.pointerDrag.GetComponent<DraggableUI>();
        if ( eventData.pointerDrag != null )
		{
            if (rect.GetComponentInChildren<DraggableUI>()) // 슬롯 안에 아이템 유무 확인
			{
				rectTransform = rect;									// 기존 rect위치 저장
                eventData.pointerDrag.transform.SetParent(transform);	// 드래그된 아이템을 드롭슬롯에 자식으로 설정
                eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;

				//previousParent는 기존 item에 이동전 부모슬롯 위치 저장
                rectTransform.GetChild(0).GetComponent<RectTransform>().position = item.previousParent.GetComponent<RectTransform>().position;
                rectTransform.GetChild(0).transform.SetParent(item.previousParent);


            }
            else
			{
                // 드래그하고 있는 대상의 부모를 현재 오브젝트로 설정하고, 위치를 현재 오브젝트 위치와 동일하게 설정
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
            }
		}
	}
}

