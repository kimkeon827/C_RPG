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
	/// ���콺 ����Ʈ�� ���� ������ ���� ���� ���η� �� �� 1ȸ ȣ��
	/// </summary>
	public void OnPointerEnter(PointerEventData eventData)
	{
		// ������ ������ ������ ��������� ����
		image.color = Color.yellow;
    }

	/// <summary>
	/// ���콺 ����Ʈ�� ���� ������ ���� ������ �������� �� 1ȸ ȣ��
	/// </summary>
	public void OnPointerExit(PointerEventData eventData)
	{
		// ������ ������ ������ �Ͼ������ ����
		image.color = Color.white;
    }

        /// <summary>
        /// ���� ������ ���� ���� ���ο��� ����� ���� �� 1ȸ ȣ��
        /// </summary>
        public void OnDrop(PointerEventData eventData)
	{
		// pointerDrag�� ���� �巡���ϰ� �ִ� ���(=������)
		item = eventData.pointerDrag.GetComponent<DraggableUI>();
        if ( eventData.pointerDrag != null )
		{
            if (rect.GetComponentInChildren<DraggableUI>()) // ���� �ȿ� ������ ���� Ȯ��
			{
				rectTransform = rect;									// ���� rect��ġ ����
                eventData.pointerDrag.transform.SetParent(transform);	// �巡�׵� �������� ��ӽ��Կ� �ڽ����� ����
                eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;

				//previousParent�� ���� item�� �̵��� �θ𽽷� ��ġ ����
                rectTransform.GetChild(0).GetComponent<RectTransform>().position = item.previousParent.GetComponent<RectTransform>().position;
                rectTransform.GetChild(0).transform.SetParent(item.previousParent);


            }
            else
			{
                // �巡���ϰ� �ִ� ����� �θ� ���� ������Ʈ�� �����ϰ�, ��ġ�� ���� ������Ʈ ��ġ�� �����ϰ� ����
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
            }
		}
	}
}

