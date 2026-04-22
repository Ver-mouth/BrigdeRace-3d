using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : ColorObject
{
    [SerializeField] private LayerMask groundLayer, stairLayer;
    [SerializeField] private PlayerBrick PlayerBrickPrefab;
    [SerializeField] private Transform tfBrickHolder;
    [SerializeField] protected Transform tfSkin;
    public Animator anim;
    private string currentAnim;

    private List<PlayerBrick> liPlayerBrick = new List<PlayerBrick>();
    [HideInInspector] public Stage stage;
    public int BrickCount => liPlayerBrick.Count;

    public virtual void OnInit()
    {
        currentAnim = "Idle";
        ClearBrick();
        tfSkin.rotation = Quaternion.identity;
    }

    // kiểm tra điểm tiếp theo có chạm đất không
    public Vector3 CheckGround(Vector3 nextPoint)
    {
        //Raycast bắt va chạm
        RaycastHit hit;
        Vector3 origin = nextPoint + Vector3.up * 0.5f; // nâng raycast lên bắt layer

        //xử lý va chạm
        if (Physics.Raycast(origin, Vector3.down, out hit, 10f, groundLayer))
        {
            //nếu chạm đất thì trả về vị trí chạm
            return hit.point + Vector3.up * 0f;
        }

        return transform.position;
    }

    //kiểm tra điểm tiếp theo có thể di chuyển không
    public bool CanMove(Vector3 nextPoint)
    {
        bool isCanMove = true;
        RaycastHit hit;
        Vector3 origin = nextPoint + Vector3.up * 0.5f; // nâng raycast lên bắt layer
        if (Physics.Raycast(origin, Vector3.down, out hit, 10f, stairLayer))
        {
            //nếu chạm bậc thang thì kiểm tra màu và gạch trên lưng
            Stair stair = hit.collider.GetComponent<Stair>();
            if (stair.colorType != colorType && liPlayerBrick.Count > 0)
            {

                stair.ChangeColor(colorType);
                RemoveBrick();
                stage.NewBrick(colorType);

            }

            if (stair.colorType != colorType && liPlayerBrick.Count == 0 && tfSkin.forward.z > 0)
            {
                isCanMove = false;
            }
        }

        return isCanMove;
    }

    public void AddBrick()
    {
        //tạo gạch vị trí holder với màu player, mỗi gạch cách nhau 0.25f
        PlayerBrick playerBrick = Instantiate(PlayerBrickPrefab, tfBrickHolder);
        playerBrick.ChangeColor(colorType);
        playerBrick.transform.localPosition = Vector3.up * liPlayerBrick.Count * 0.25f;
        liPlayerBrick.Add(playerBrick);
    }

    public void RemoveBrick()
    {
        //xóa gạch cuối cùng trong list nếu có
        if (liPlayerBrick.Count > 0)
        {
            //gán gạch cuối vào biến tạm rồi xóa khỏi list
            PlayerBrick playerBrick = liPlayerBrick[liPlayerBrick.Count - 1];
            liPlayerBrick.RemoveAt(liPlayerBrick.Count - 1);
            Destroy(playerBrick.gameObject);
        }
    }

    public void ClearBrick()
    {
        for (int i = 0; i < liPlayerBrick.Count; i++)
        {
            Destroy(liPlayerBrick[i].gameObject);
            //SimplePool.Despawn(liPlayerBrick[i].gameObject);
        }
        liPlayerBrick.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        //nếu chạm gạch cùng màu thì thêm gạch vào lưng và hủy gạch
        if (other.CompareTag("Brick"))
        {
            Brick brick = other.GetComponent<Brick>();
            if (brick.colorType == colorType)
            {
                brick.OnDespawn();
                AddBrick();
                Destroy(brick.gameObject);

            }
        }


    }

    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            if (currentAnim != animName)
            {
                
                anim.ResetTrigger(currentAnim);
                currentAnim = animName;
                anim.SetTrigger(currentAnim);
            }
        }
    }
}
