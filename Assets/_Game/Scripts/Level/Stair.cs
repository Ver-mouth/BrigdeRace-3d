using UnityEngine;

public class Stair : ColorObject
{
    public bool isBuilt = false;
    public ColorType ownerColor; // màu của người xây cầu này

    private void Start()
    {
        ChangeColor(ColorType.White);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu có Player hoặc Bot đi lên
        var unit = other.GetComponent<Bot>();
        if (unit == null) return;

        // Nếu cầu chưa xây và nhân vật có gạch
        if (!isBuilt && unit.BrickCount > 0)
        {
            isBuilt = true;
            ownerColor = unit.colorType;
            unit.RemoveBrick(); // trừ 1 viên gạch
            ChangeColor(unit.colorType); // đổi màu bậc cầu
        }
        else if (!isBuilt && unit.BrickCount <= 0)
        {
            // Không có gạch => dừng lại (không cho đi tiếp)
            unit.StopMove();
        }
    }
}
