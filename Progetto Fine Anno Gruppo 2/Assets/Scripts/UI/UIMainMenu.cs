using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSub;
using UnityEngine.SceneManagement;

public class UIMainMenu : Menu
{
    public override void Open()
    {
        base.Open();
        Time.timeScale = 1;
    }

    public override void Close()
    {
        base.Close();
    }
}
