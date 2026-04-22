using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/ColorData", order = 1)]
public class ColorData : ScriptableObject
{
    //tạo mảng material để lưu màu
    [SerializeField] private Material[] colorMats;

    // hàm lấy material theo colorType
    public Material GetColorMat(ColorType colorType)
    {
        return colorMats[(int)colorType];
    }
}
