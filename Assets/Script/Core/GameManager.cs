using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // ���� ---------------------------------------------------------------------------------------

    /// <summary>
    /// �÷��̾�
    /// </summary>
    Player player;

    /// <summary>
    /// ������ �����͸� �����ϴ� �޴���
    /// </summary>
    ItemDataManager itemData;

    C_InventoryUI inventoryUI;

    ItemLogger logger;
    public ItemLogger Logger => logger;

    // ������Ƽ ------------------------------------------------------------------------------------

    /// <summary>
    /// player �б� ���� ������Ƽ.
    /// </summary>
    public Player Player => player;

    /// <summary>
    /// ������ ������ �޴���(�б�����) ������Ƽ
    /// </summary>
    public ItemDataManager ItemData => itemData;

    public C_InventoryUI InvenUI => inventoryUI;

    // �Լ� ---------------------------------------------------------------------------------------

    /// <summary>
    /// ���� �޴����� ���� ��������ų� ���� �ε� �Ǿ��� �� ����� �ʱ�ȭ �Լ�
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();

        itemData = GetComponent<ItemDataManager>();
        player = FindObjectOfType<Player>();
        inventoryUI = FindObjectOfType<C_InventoryUI>();
        logger = FindObjectOfType<ItemLogger>();
    }
}
