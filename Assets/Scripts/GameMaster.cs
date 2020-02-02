using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // Create -> Fall -> Arrange -> Link -> Calculate combo
    public enum GameStatus
    {
        GameInitializing, PuyoCreating, PuyoFalling, PuyoArranging, PuyoLinking, ComboCalculating, GamePause, GameOver
    }

    public GameObject singleton;
    private PuyoCreater creater;
    private PuyoController controller;
    public GameObject puyoGroup;
    public float fallingSpeed;
    public GameObject mainPuyoShiny;
    public GameObject gameOver;

    [System.NonSerialized]
    public GameStatus gameStatus;
    [System.NonSerialized]
    public GameObject puyoGroupObj;
    [System.NonSerialized]
    public GameObject mainPuyoShinyObj;
    [System.NonSerialized]
    public GameObject gameOverObj;
    [System.NonSerialized]
    public Puyo[,] puyoArr;
    [System.NonSerialized]
    public Puyo controlMainPuyo;
    [System.NonSerialized]
    public Puyo controlSubPuyo;
    [System.NonSerialized]
    public Queue<Puyo> puyoInventory;
    //0=top, 1=right, 2=down, 3=left
    [System.NonSerialized]
    public int subPuyoDirection = 2;
    [System.NonSerialized]
    public int comboNumber = 0;

//this may be hardcoded play area limits, should check this
    public static int bottomPosition = -176;
    public static int leftPosition = -96;
    public static int rightPosition = 64;

    [System.NonSerialized]
    private bool falling = true;

    // Use this for initialization
    void Start() {
        creater = this.GetComponent<PuyoCreater>();
        controller = this.GetComponent<PuyoController>();
        puyoArr = new Puyo[6, 13];
        puyoGroupObj = puyoGroup;
        gameOverObj = gameOver;
        puyoInventory = new Queue<Puyo>();
        gameStatus = GameStatus.GameInitializing;
        mainPuyoShinyObj = mainPuyoShiny;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameStatus == GameStatus.GameInitializing)
        {
            puyoInventory.Enqueue(creater.PuyoCreate(100, 175));
            puyoInventory.Enqueue(creater.PuyoCreate(100, 143));
            puyoInventory.Enqueue(creater.PuyoCreate(100, 50));
            puyoInventory.Enqueue(creater.PuyoCreate(100, 18));

            gameStatus = GameStatus.PuyoCreating;
        }

        if (gameStatus == GameStatus.PuyoCreating)
        {
            controller.puyoCreate();
            gameStatus = GameStatus.PuyoFalling;
        }

        if (gameStatus == GameStatus.PuyoFalling)
        {
            if (falling)
            {
                StartCoroutine("fallingGap");
                falling = false;
            }
        }

        if (gameStatus == GameStatus.PuyoArranging)
        {
            controller.puyoArrange();
            gameStatus = GameStatus.PuyoLinking;
        }

        if (gameStatus == GameStatus.PuyoLinking)
        {
            controller.resetPuyoStatusAndLinkPuyoList();
            controller.linkSamePuyo();
            gameStatus = GameStatus.ComboCalculating;
        }

        if (gameStatus == GameStatus.ComboCalculating)
        {
            if (controller.readyToEliminatePuyo())
            {
                StartCoroutine("statusChangingGap");
                gameStatus = GameStatus.GamePause;
            }
            else
            {
                gameStatus = GameStatus.PuyoCreating;
            }
        }
    }

    IEnumerator fallingGap() {
        yield return new WaitForSeconds(fallingSpeed);
        //If reach the bottom, create new puyo
        if (controller.reachBottom((int)controlMainPuyo.getPosition().x, (int)controlMainPuyo.getPosition().y) || 
            controller.reachBottom((int)controlSubPuyo.getPosition().x, (int)controlSubPuyo.getPosition().y))
        {
            if (controller.isGameOver())
            {
                gameOverObj.SetActive(true);
                gameStatus = GameStatus.GamePause;
            }
            else
            {
                int mainX = (int)controlMainPuyo.getPosition().x;
                int mainY = (int)controlMainPuyo.getPosition().y;
                int subX = (int)controlSubPuyo.getPosition().x;
                int subY = (int)controlSubPuyo.getPosition().y;
                puyoArr[mainX, mainY] = controlMainPuyo;
                puyoArr[subX, subY] = controlSubPuyo;

                gameStatus = GameStatus.PuyoArranging;
            }
        }
        else
        {
            controller.puyoDown(true);
        }
        falling = true;
    }

    //Before eliminated puyo, wait a while.
    IEnumerator statusChangingGap()
    {
        yield return new WaitForSeconds(0.8f);
        StartCoroutine("showComboImg");
        ImageController.setComboNumber(++comboNumber);
        controller.eliminatePuyo();
        gameStatus = GameStatus.PuyoArranging;
    }

    IEnumerator showComboImg()
    {
        ImageController.comboGameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        ImageController.comboGameObject.SetActive(false);
    }
}
