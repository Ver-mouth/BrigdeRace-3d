using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelText : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;


    private void OnEnable()
    {
        int level = PlayerPrefs.GetInt("Level", 0) + 1; 
        levelText.text = "Level " + level.ToString();
    }
}
