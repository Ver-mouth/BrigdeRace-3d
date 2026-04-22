using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Player : Character
{
    [SerializeField] private float speed;
    [SerializeField] private Transform tfPlayer;





    void Update()
    {
        // di chuyển bằng JoyStick
        if (GameManager.Instance.IsState(GameState.Gameplay))
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 nextPoint = JoyStickControl.direct * speed * Time.deltaTime + tfPlayer.transform.position;
                if (CanMove(nextPoint))
                {
                    tfPlayer.transform.position = CheckGround(nextPoint);
                }
                if (JoyStickControl.direct != Vector3.zero)
                {
                    tfSkin.forward = JoyStickControl.direct;

                }
                ChangeAnim("Run");
            }

            if (Input.GetMouseButtonUp(0))
            {
                ChangeAnim("Idle");
            }
        }


    }


}




