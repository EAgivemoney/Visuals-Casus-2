using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text descriptionText;
    public RectTransform backgroundRectTransform;

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            Input.mousePosition,
            null,
            out localPoint
        );
        transform.localPosition = localPoint;
    }

    public void ShowTooltip(string description)
    {
        gameObject.SetActive(true);
        descriptionText.text = description;
        Vector2 backgroundSize = new Vector2(descriptionText.preferredWidth + 8f, descriptionText.preferredHeight + 8f);
        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
