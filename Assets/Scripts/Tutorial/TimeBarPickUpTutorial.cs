using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBarPickUpTutorial : Tutorial {

    public GameObject timebarUI;
    public TimeManager timeManager;
    public PickUp pickUp;
    public int timesToPick;

    int timesPicked;

	public override void Execute ()
    {
        timebarUI.SetActive(true);
        pickUp.gameObject.SetActive(true);
        timeManager.gameObject.SetActive(true);
        pickUp.OnPickUp += OnPickUp;
        timesToPick = 3;
        timesPicked = 0;
	}
	
	// Update is called once per frame
	void OnPickUp ()
    {
        timesPicked++;

        if (timesPicked >= timesToPick)
        {
            pickUp.OnPickUp -= OnPickUp;
            base.TutorialCompleted();
            pickUp.gameObject.SetActive(false);
            timeManager.gameObject.SetActive(false);
            timebarUI.SetActive(false);
        }

	}
}
