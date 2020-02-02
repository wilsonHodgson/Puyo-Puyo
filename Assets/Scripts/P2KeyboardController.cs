using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2KeyboardController : MonoBehaviour
{
    public GameObject player_object;
    private GameMaster player;
    private PuyoController controller;
    // Update is called once per frame
    void Start () {
        if (player_object != null) {
            controller = player_object.GetComponent<PuyoController>();
            player = player_object.GetComponent<GameMaster>();
        }
    }
    void Update () {
        if (player.gameStatus==GameMaster.GameStatus.PuyoFalling)
        {

            if (Input.GetKeyUp(KeyCode.A) && (!controller.havingObstacle(0, (int)player.controlMainPuyo.getPosition().x, (int)player.controlMainPuyo.getPosition().y) &&
                                                       !controller.havingObstacle(0, (int)player.controlSubPuyo.getPosition().x, (int)player.controlSubPuyo.getPosition().y)))
            {
                controller.puyoLeft(true);
            }
            if (Input.GetKeyUp(KeyCode.D) && (!controller.havingObstacle(1, (int)player.controlMainPuyo.getPosition().x, (int)player.controlMainPuyo.getPosition().y) &&
                                                       !controller.havingObstacle(1, (int)player.controlSubPuyo.getPosition().x, (int)player.controlSubPuyo.getPosition().y)))
            {
                controller.puyoRight(true);
            }
            if (Input.GetKeyUp(KeyCode.S) && (!controller.reachBottom((int)player.controlMainPuyo.getPosition().x, (int)player.controlMainPuyo.getPosition().y) &&
                                                       !controller.reachBottom((int)player.controlSubPuyo.getPosition().x, (int)player.controlSubPuyo.getPosition().y)))
            {
                controller.puyoDown(true);
            }
            //counterclockwise
            if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.W))
            {
                controller.puyoCounterclockwise();
            }
            //clockwise
            if (Input.GetKeyUp(KeyCode.X))
            {
                controller.puyoClockwise();
            }
            //Hold
            /*if (Input.GetKeyUp(KeyCode.Space))
            {
                controller.hold();
            }*/
        }
    }
}
