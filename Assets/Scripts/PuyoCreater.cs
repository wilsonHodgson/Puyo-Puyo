using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoCreater : MonoBehaviour
{
    public GameObject player_object;
    private GameMaster player;
    public GameObject bluePuyo;
    public GameObject greenPuyo;
    public GameObject purplePuyo;
    public GameObject redPuyo;
    public GameObject yellowPuyo;
    public GameObject greyPuyo;

    public static GameObject bluePuyoGameObject;
    public static GameObject greenPuyoGameObject;
    public static GameObject purplePuyoGameObject;
    public static GameObject redPuyoGameObject;
    public static GameObject yellowPuyoGameObject;
    public static GameObject greyPuyoGameObject;

    void Start()
    {
        if (player_object != null){
            player = player_object.GetComponent<GameMaster>();
        }
        bluePuyoGameObject = bluePuyo;
        greenPuyoGameObject = greenPuyo;
        purplePuyoGameObject = purplePuyo;
        redPuyoGameObject = redPuyo;
        yellowPuyoGameObject = yellowPuyo;
        greyPuyoGameObject = greyPuyo;
    }

    public Puyo PuyoCreate(int x, int y) {
        //print("puyo is creating...");
        Puyo puyo = player.puyoGroupObj.AddComponent<Puyo>();
        puyo.setColor(Random.Range(0, 3));
        puyo.setLinkStatus(ImageController.NORMAL);
        GameObject newPuyoObj;
        switch (puyo.getColor()) {
            case 0:
                newPuyoObj = Instantiate(bluePuyoGameObject);
                break;
            case 1:
                newPuyoObj = Instantiate(greenPuyoGameObject);
                break;
            case 2:
                newPuyoObj = Instantiate(purplePuyoGameObject);
                break;
            case 3:
                newPuyoObj = Instantiate(redPuyoGameObject);
                break;
            case 4:
                newPuyoObj = Instantiate(yellowPuyoGameObject);
                break;
            default:
                newPuyoObj = Instantiate(bluePuyoGameObject);
                break;
        }
        newPuyoObj.transform.SetParent(player.puyoGroupObj.transform);
        newPuyoObj.transform.localPosition = new Vector3(x, y, 0);
        newPuyoObj.transform.localScale = new Vector3(1, 1, 1);
        puyo.setPuyoObj(newPuyoObj);

        List<Puyo> puyoList = new List<Puyo>();
        puyoList.Add(puyo);
        puyo.setLinkPuyoList(puyoList);

        return puyo;
    }

    public Puyo TrashCreate(int x, int y){
        Puyo puyo = player.puyoArr[x,y];

        if (puyo != null){
            Destroy(puyo.getPuyoObj());
            puyo = null;
        }
        puyo.setColor(5);
        puyo.setLinkStatus(ImageController.NORMAL);

        GameObject newPuyoObj = Instantiate(greyPuyoGameObject);
        newPuyoObj.transform.localPosition = new Vector3(x, y, 0);
        newPuyoObj.transform.localScale = new Vector3(1, 1, 1);
        puyo.setPuyoObj(newPuyoObj);

        List<Puyo> puyoList = new List<Puyo>();
        puyoList.Add(puyo);
        puyo.setLinkPuyoList(puyoList);

        return puyo;
    }
}
