using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    public Selectable defaultSelection;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(null);
    }

    void LateUpdate()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (Input.GetButtonDown(Constants.k_ButtonNameSubmit)
                || Input.GetAxisRaw(Constants.k_AxisNameHorizontal) != 0
                || Input.GetAxisRaw(Constants.k_AxisNameVertical) != 0)
            {
                EventSystem.current.SetSelectedGameObject(defaultSelection.gameObject);
            }
        }
    }
}
