using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUI : MonoBehaviour
{
    public BaseObjData data;

    public TextMeshProUGUI nameText;
    public RectTransform nameBoxRectTransform;
    public TextMeshProUGUI descriptionText;
    public RectTransform descriptionBoxRectTransform;

    public Image crossHair;

    public void Init()
    {
        nameText.text = $"[{data.objectName}]";
        UpdateNameBackgroundSize();
        descriptionText.text = data.description;
        UpdateDescriptionBackgroundSize();

        descriptionBoxRectTransform.gameObject.SetActive(false);
    }
    void UpdateNameBackgroundSize()
    {
        nameBoxRectTransform.sizeDelta = new Vector2(30 + (data.objectName.Length) * 20, 30);
    }
    void UpdateDescriptionBackgroundSize()
    {
        descriptionBoxRectTransform.sizeDelta = new Vector2(250, ((data.description.Length / 13) + 1) * 20 + 30);
    }

    public void DescriptionActive(bool active)
    {
        descriptionBoxRectTransform.gameObject.SetActive(active);
    }
}
