using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defeat : UICanvas
{
    public void RetryButton()
    {
        LevelManager.Instance.OnRetry();
        Close(0f);
    }
}
