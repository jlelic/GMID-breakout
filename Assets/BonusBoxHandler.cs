using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBoxHandler : BoxHandler
{
    public override void OnBoxBroken()
    {
        var balls = GameObject.FindGameObjectsWithTag("Ball");
        var message = "";
        switch (Random.Range(0, 3))
        {
            case 0:
                foreach (GameObject ball in balls)
                {
                    GameObject.Instantiate(ball);
                    GameObject.Instantiate(ball);
                }
                message = "3X BALLS";
                break;
            case 1:
                foreach (GameObject ball in balls)
                {
                    ball.GetComponent<BallMovement>().EnlargeBall();
                }
                message = "LARGE BALL";
                break;
            case 2:
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>().EnlargePlayer();
                message = "LARGE PADDLE";
                break;
        }
        ShowMessage(message, new Color(0,0.8f,0,1));
    }
}
