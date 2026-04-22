using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform tfCam;
    [SerializeField] Transform tfPlayer;
    [SerializeField] Vector3 offset;

    private void LateUpdate()
    {
        //set vị trí camera theo vị trí player + offset, lerp để làm mượt di chuyển
        tfCam.position = Vector3.Lerp(tfCam.position, tfPlayer.position + offset, Time.deltaTime * 5f);
    }
}
