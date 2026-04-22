using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;

public class NewStageBox : MonoBehaviour
{
    public Stage stage;
    //tạo 1 hashset để đánh dấu những character đã chạm vào box này
    private HashSet<Character> triggeredCharacters = new HashSet<Character>();

    private void OnTriggerEnter(Collider other)
    {

        Character character = other.GetComponent<Character>();
        if (character != null && !triggeredCharacters.Contains(character))
        {
            triggeredCharacters.Add(character);

            character.stage = stage;
            stage.InitColor(character.colorType);


        }



    }


}
