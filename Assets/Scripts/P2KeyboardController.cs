﻿using System.Collections;
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

            if (Input.GetKeyDown(KeyCode.A) && (!controller.havingObstacle(0, (int)player.controlMainPuyo.getPosition().x, (int)player.controlMainPuyo.getPosition().y) &&
                                                       !controller.havingObstacle(0, (int)player.controlSubPuyo.getPosition().x, (int)player.controlSubPuyo.getPosition().y)))
            {
                FindObjectOfType<AudioManager>().Play("move");
                controller.puyoLeft(true);
            }
            if (Input.GetKeyDown(KeyCode.D) && (!controller.havingObstacle(1, (int)player.controlMainPuyo.getPosition().x, (int)player.controlMainPuyo.getPosition().y) &&
                                                       !controller.havingObstacle(1, (int)player.controlSubPuyo.getPosition().x, (int)player.controlSubPuyo.getPosition().y)))
            {
                FindObjectOfType<AudioManager>().Play("move");
                controller.puyoRight(true);
            }
            if (Input.GetKeyDown(KeyCode.S) && (!controller.reachBottom((int)player.controlMainPuyo.getPosition().x, (int)player.controlMainPuyo.getPosition().y) &&
                                                       !controller.reachBottom((int)player.controlSubPuyo.getPosition().x, (int)player.controlSubPuyo.getPosition().y)))
            {
                FindObjectOfType<AudioManager>().Play("move");
                controller.puyoDown(true);
            }
            //counterclockwise
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.W))
            {
                controller.puyoCounterclockwise();
                FindObjectOfType<AudioManager>().Play("rotate");
            }
            //clockwise
            if (Input.GetKeyDown(KeyCode.Z))
            {
                controller.puyoClockwise();
                FindObjectOfType<AudioManager>().Play("rotate");
            }
        }
    }
}
