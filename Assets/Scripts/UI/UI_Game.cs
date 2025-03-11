using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Game : MonoBehaviour
{
    public delegate void InventoryDelegate(bool isInventoryOn);
    public InventoryDelegate inventoryDelegate;
    public UI_Inventory ui_Inventory;
    private void Awake()
    {
        inventoryDelegate += InventoryOn;
    }
    public void OnInventory(InputAction.CallbackContext context)
    {
        inventoryDelegate?.Invoke(ui_Inventory.gameObject.activeSelf);
    }
    void InventoryOn(bool isInventoryOn)
    {
        ui_Inventory.gameObject.SetActive(!isInventoryOn);
        if (!isInventoryOn)
        {
            Cursor.lockState = CursorLockMode.None;
            GameManager.Instance.Player.controller.isLookable = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            GameManager.Instance.Player.controller.isLookable = true;
        }
    }
}
