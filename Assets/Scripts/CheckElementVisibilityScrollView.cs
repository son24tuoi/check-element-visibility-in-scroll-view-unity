using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CheckElementVisibilityScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform elementRectTransform;
    public bool noCheck = false;

    [SerializeField] private UnityEvent onShowEvent;
    [SerializeField] private UnityEvent onHiddenBelowEvent;
    [SerializeField] private UnityEvent onHiddenAboveEvent;

    private Vector3[] elementCorners = new Vector3[4];
    private Rect elementRect;
    private bool isElementHidden;
    private bool isHidden;

    private Vector2 viewPortLocalPos;
    private Vector2 targetLocalPos;
    private Vector3 newTargetLocalPos;
    private Vector3 dis;

    private void Start()
    {
        Check();
    }

    public void Check()
    {
        if (noCheck) return;
        if (!elementRectTransform) return;

        // Kiem tra hien thi
        isElementHidden = IsElementHidden();

        if (isElementHidden)
        {
            // Debug.Log("Hide");
            Hide();
        }
        else
        {
            // Debug.Log("Show");
            onShowEvent?.Invoke();
        }
    }

    private bool IsElementHidden()
    {
        elementRectTransform.GetWorldCorners(elementCorners);
        elementRect = new Rect(elementCorners[0], elementCorners[2] - elementCorners[0]);

        isHidden = !RectTransformUtility.RectangleContainsScreenPoint(scrollRect.viewport, elementRect.min) ||
                    !RectTransformUtility.RectangleContainsScreenPoint(scrollRect.viewport, elementRect.max);

        return isHidden;
    }

    public void Hide()
    {
        viewPortLocalPos = scrollRect.viewport.localPosition;
        targetLocalPos = elementRectTransform.localPosition;

        newTargetLocalPos.x = 0 - (viewPortLocalPos.x + targetLocalPos.x) + (scrollRect.viewport.rect.width / 2) - (elementRectTransform.rect.width / 2);
        newTargetLocalPos.y = 0 - (viewPortLocalPos.y + targetLocalPos.y) + (scrollRect.viewport.rect.height / 2) - (elementRectTransform.rect.height / 2);
        newTargetLocalPos.z = 0;

        dis = newTargetLocalPos - scrollRect.content.localPosition;

        // Debug.Log($"dis: {dis}");

        if (dis.y > 0)
        {
            // Debug.Log("Down");
            onHiddenBelowEvent?.Invoke();
        }
        else if (dis.y < 0)
        {
            // Debug.Log("Up");
            onHiddenAboveEvent?.Invoke();
        }
    }
}
