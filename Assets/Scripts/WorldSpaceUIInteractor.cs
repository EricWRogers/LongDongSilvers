using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class WorldSpaceUIInteractor : MonoBehaviour
{
    public float interactDistance = 3f;

    void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Screen.width / 2, Screen.height / 2)
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (Vector3.Distance(transform.position, result.worldPosition) > interactDistance) continue;

            var button = result.gameObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
                break;
            }
        }
    }
}