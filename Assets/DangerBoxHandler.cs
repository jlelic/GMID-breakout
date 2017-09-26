using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DangerBoxHandler : BoxHandler {
    public override void OnBoxBroken()
    {
        var balls = GameObject.FindGameObjectsWithTag("Ball");
        var message = "";

        switch (Random.Range(0,3))
        {
            case 0:
                for (int i = 0; i < balls.Length; i++)
                {
                    balls[i].GetComponent<BallMovement>().SpeedUpBall();
                }
                message = "FAST BALL";
                break;
            case 1:
                for (int i = 0; i < balls.Length; i++)
                {
                    balls[i].GetComponent<BallMovement>().ShrinkBall();
                }
                message = "SMALL BALL";
                break;
            case 2:
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>().ShrinkPlayer();
                message = "SMALL PADDLE";
                break;
        }
        ShowMessage(message, new Color(0.8f, 0, 0, 1));
    }
}
