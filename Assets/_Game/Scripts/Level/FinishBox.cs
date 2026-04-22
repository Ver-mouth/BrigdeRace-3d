using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        Character character = other.GetComponent<Character>();
        if (character != null)
        {
            
            LevelManager.Instance.OnFinishGame();
            if (character is Player)
            {
                
                UIManager.Instance.OpenUI<Victory>();
            }
            else
            {
                UIManager.Instance.OpenUI<Defeat>();
            }

            UIManager.Instance.CloseUI<GamePlay>();
            GameManager.Instance.ChangeState(GameState.Pause);

            character.ChangeAnim("Dance");
            character.transform.eulerAngles = Vector3.up * 180;
            character.OnInit();
        }
    }
}
