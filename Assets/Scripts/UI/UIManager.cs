using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public UI_Inventory ui_Inventory;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        ui_Inventory.gameObject.SetActive(true);        //���� ������Ʈ�� �����Ҵ��� ���� �ִٲ���
        ui_Inventory.gameObject.SetActive(false);
    }

    public Transform objectUIs;
    public GameObject objectUI;

    public void OnInventory(InputAction.CallbackContext context)
    {
        ui_Inventory.gameObject.SetActive(!ui_Inventory.gameObject.activeSelf);
        Cursor.lockState = ui_Inventory.gameObject.activeSelf? CursorLockMode.None : CursorLockMode.Locked;
    }
}
