using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickControlref : MonoBehaviour
{

    [SerializeField] private float magnitude;
    private Vector3 screen;
    public static Vector3 direct;
    private Vector3 MousePos => Input.mousePosition - screen / 2;
    public RectTransform rtfJoystickBG;
    public RectTransform rtfJoystickControl;
    private Vector3 startPoint;
    private Vector3 updatePoint;

    public GameObject joystickPanel;
    // Start is called before the first frame update
    void Awake()
    {
        screen.x = Screen.width;
        screen.y = Screen.height;
        direct = Vector3.zero;

        joystickPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPoint = MousePos;
            rtfJoystickBG.anchoredPosition = startPoint;
            joystickPanel.SetActive(true);
        }    
        if (Input.GetMouseButton(0))
        {
            updatePoint = MousePos;

            rtfJoystickControl.anchoredPosition = Vector3.ClampMagnitude((updatePoint - startPoint),magnitude) + startPoint ;
            direct = (updatePoint - startPoint).normalized;
            direct.z = direct.y;
            direct.y = 0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            joystickPanel.SetActive(false);
        }
    }

    private void OnDisable()
    {
        direct = Vector3.zero;
        rtfJoystickControl.anchoredPosition = startPoint;
    }
}
