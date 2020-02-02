using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1KeyboardController : MonoBehaviour
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

            if (Input.GetKeyDown(KeyCode.LeftArrow) && (!controller.havingObstacle(0, (int)player.controlMainPuyo.getPosition().x, (int)player.controlMainPuyo.getPosition().y) &&
                                                       !controller.havingObstacle(0, (int)player.controlSubPuyo.getPosition().x, (int)player.controlSubPuyo.getPosition().y)))
            {
                FindObjectOfType<AudioManager>().Play("move");
                controller.puyoLeft(true);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && (!controller.havingObstacle(1, (int)player.controlMainPuyo.getPosition().x, (int)player.controlMainPuyo.getPosition().y) &&
                                                       !controller.havingObstacle(1, (int)player.controlSubPuyo.getPosition().x, (int)player.controlSubPuyo.getPosition().y)))
            {
                FindObjectOfType<AudioManager>().Play("move");
                controller.puyoRight(true);
            }
            if (Input.GetKey(KeyCode.DownArrow) && (!controller.reachBottom((int)player.controlMainPuyo.getPosition().x, (int)player.controlMainPuyo.getPosition().y) &&
                                                       !controller.reachBottom((int)player.controlSubPuyo.getPosition().x, (int)player.controlSubPuyo.getPosition().y)))
            {
                FindObjectOfType<AudioManager>().Play("move");
                controller.puyoDown(true);
            }
            //counterclockwise
            if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                controller.puyoCounterclockwise();
                FindObjectOfType<AudioManager>().Play("rotate");
            }
            //clockwise
            if (Input.GetKeyUp(KeyCode.X))
            {
                controller.puyoClockwise();
                FindObjectOfType<AudioManager>().Play("rotate");
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                //test key
                FindObjectOfType<AudioManager>().Play("gameOver");
                player.gameOverObj.SetActive(true);
                player.gameStatus = GameMaster.GameStatus.GamePause;
                FindObjectOfType<AudioManager>().StopPlaying("music");
            }
        }
    }
}
