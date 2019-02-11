using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashTutorial : Tutorial {

    public int dashAmount = 3;
    int dashTimes;

    public override void Execute()
    {
        dashTimes = 0;
        Player.OnPlayerDash += DashCount;
    }

    // Use this for initialization
    void DashCount ()
    {
        dashTimes++;	
        if(dashTimes >= dashAmount)
        {
            Player.OnPlayerDash -= DashCount;
            base.TutorialCompleted();
            gameObject.SetActive(false);
            return;
        }
	}
}
