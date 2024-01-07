using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTutorial : Bomb
{
    private TutorialController _tutorialController;

    new private void Awake()
    {
        base.Awake();
        _tutorialController = GameObject.FindObjectOfType<TutorialController>();
    }
    new private void Start()
    {
        base.Start();
        _timer.SetInitialTime(120);
    }

    public override void DisplayInteractMsg()
    {
        base.DisplayInteractMsg();
        _tutorialController.OnDefusalSeqRead();
    }

    protected override void DefuseBomb()
    {
        base.DefuseBomb();
        _tutorialController.OnBombDefused();

    }
}
