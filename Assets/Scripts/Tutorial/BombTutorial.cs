using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTutorial : Bomb
{
    new private void Start()
    {
        base.Start();
        _timer.SetInitialTime(60);
    }

    public override void DisplayInteractMsg()
    {
        base.DisplayInteractMsg();
        GameObject.FindObjectOfType<TutorialController>().ProceedWithTutorial();
    }
}
