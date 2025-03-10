using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUI : MonoBehaviour
{
    public BaseItemData data;
    public Transform targetObj;
    Camera _camera;

    public TextMeshProUGUI nameText;
    public RectTransform nameBoxRectTransform;
    public TextMeshProUGUI descriptionText;
    public RectTransform descriptionBoxRectTransform;
    bool sizeFit = false;

    public Image crossHair;
    private void Update()
    {
        UpdateDescriptionBackgroundSize();
        UpdateNameBackgroundSize();
    }
    private void LateUpdate()
    {
        transform.position = _camera.WorldToScreenPoint(targetObj.position);
    }
    public void Init()
    {
        _camera = Camera.main;
        data = targetObj.GetComponent<BaseItem>().baseItemData;
        nameText.text = $"[{data.objectName}]";
        descriptionText.text = data.description;

        descriptionBoxRectTransform.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        
    }
    void UpdateNameBackgroundSize()
    {
        nameBoxRectTransform.sizeDelta = nameText.rectTransform.sizeDelta + new Vector2(10,10);
    }
    void UpdateDescriptionBackgroundSize()
    {
        descriptionBoxRectTransform.sizeDelta = descriptionText.rectTransform.sizeDelta + new Vector2(10, 10);
    }

    public void DescriptionActive(bool active)
    {
        descriptionBoxRectTransform.gameObject.SetActive(active);
    }
}
