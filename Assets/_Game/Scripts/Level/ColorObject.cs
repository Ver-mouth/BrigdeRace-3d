using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : GameUnit
{
    [SerializeField] private SkinnedMeshRenderer renderSkinned;
    [SerializeField] private MeshRenderer render;
    [SerializeField] private ColorData colorData;

    public ColorType colorType;


    public void ChangeColor(ColorType colorType)
    {
        this.colorType = colorType;
        if (CompareTag("Character") == true)
        {
            renderSkinned.material = colorData.GetColorMat(colorType);
        }
        else
        {
            render.material = colorData.GetColorMat(colorType);
        }

    }
    
}
