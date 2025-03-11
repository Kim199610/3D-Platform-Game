using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public UI_Inventory ui_Inventory;
    public UI_Game ui_game;

    
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
        ui_Inventory.gameObject.SetActive(true);        //하위 오브젝트들 동적할당을 위해 켯다끄기
        ui_Inventory.gameObject.SetActive(false);
    }

    public Transform objectUIs;
    public GameObject objectUI;

    
}
