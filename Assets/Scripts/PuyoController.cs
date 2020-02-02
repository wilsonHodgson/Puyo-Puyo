using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class IntEvent: UnityEvent<int> {} 
public class PuyoController : MonoBehaviour {

    public GameObject singleton;
    public GameObject player_object;
    private GameMaster player;
    private PuyoCreater creater;
    private ImageController image_controller;

    private EventManagerPuyo eventManager;

    void Awake(){
        eventManager = FindObjectOfType<EventManagerPuyo>();
    }

    void onEnable(){
    }
    void Start() {
        eventManager.OnRowDeleteEvent += deleteRows;
        if (singleton != null){
            image_controller = singleton.GetComponent<ImageController>();
        }
        if (player_object != null){
            player = player_object.GetComponent<GameMaster>();
            creater = player_object.GetComponent<PuyoCreater>();
        }

    }
    public void puyoCreate()
    {
        player.controlMainPuyo = player.puyoInventory.Dequeue();
        player.controlSubPuyo = player.puyoInventory.Dequeue();
        player.puyoInventory.ElementAt(1).getPuyoObj().transform.localPosition = new Vector3(100, 175, 0);
        player.puyoInventory.ElementAt(0).getPuyoObj().transform.localPosition = new Vector3(100, 143, 0);
        player.puyoInventory.Enqueue(creater.PuyoCreate(100, 18));
        player.puyoInventory.Enqueue(creater.PuyoCreate(100, 50));
        player.controlMainPuyo.getPuyoObj().transform.localPosition = new Vector3(0, 208, 0);
        player.controlSubPuyo.getPuyoObj().transform.localPosition = new Vector3(0, 240, 0);
        player.controlMainPuyo.setPosition(new Vector2(3, 12));
        player.controlSubPuyo.setPosition(new Vector2(3, 13));
        player.subPuyoDirection = 0;
        player.comboNumber = 0;
        player.mainPuyoShinyObj.transform.localPosition = new Vector3(0, 208, 0);
        player.mainPuyoShinyObj.transform.SetAsLastSibling();
        image_controller.setShinyPuyo(player.controlMainPuyo.getColor());
    }

    public void puyoDown(bool moveShinyPuyo) {
        player.controlMainPuyo.getPuyoObj().transform.localPosition = new Vector3
                    (player.controlMainPuyo.getPuyoObj().transform.localPosition.x, player.controlMainPuyo.getPuyoObj().transform.localPosition.y - 32, player.controlMainPuyo.getPuyoObj().transform.localPosition.z);
        player.controlSubPuyo.getPuyoObj().transform.localPosition = new Vector3
            (player.controlSubPuyo.getPuyoObj().transform.localPosition.x, player.controlSubPuyo.getPuyoObj().transform.localPosition.y - 32, player.controlSubPuyo.getPuyoObj().transform.localPosition.z);
        player.controlMainPuyo.setPosition(new Vector2(player.controlMainPuyo.getPosition().x, player.controlMainPuyo.getPosition().y - 1));
        player.controlSubPuyo.setPosition(new Vector2(player.controlSubPuyo.getPosition().x, player.controlSubPuyo.getPosition().y - 1));
        if (moveShinyPuyo)
        {
            player.mainPuyoShinyObj.transform.localPosition = player.controlMainPuyo.getPuyoObj().transform.localPosition;
        }
    }

    public void puyoLeft(bool moveShinyPuyo) {
        player.controlMainPuyo.getPuyoObj().transform.localPosition = new Vector3
                    (player.controlMainPuyo.getPuyoObj().transform.localPosition.x - 32, player.controlMainPuyo.getPuyoObj().transform.localPosition.y, player.controlMainPuyo.getPuyoObj().transform.localPosition.z);
        player.controlSubPuyo.getPuyoObj().transform.localPosition = new Vector3
            (player.controlSubPuyo.getPuyoObj().transform.localPosition.x - 32, player.controlSubPuyo.getPuyoObj().transform.localPosition.y, player.controlSubPuyo.getPuyoObj().transform.localPosition.z);
        player.controlMainPuyo.setPosition(new Vector2(player.controlMainPuyo.getPosition().x - 1, player.controlMainPuyo.getPosition().y));
        player.controlSubPuyo.setPosition(new Vector2(player.controlSubPuyo.getPosition().x - 1, player.controlSubPuyo.getPosition().y));
        if (moveShinyPuyo)
        {
            player.mainPuyoShinyObj.transform.localPosition = player.controlMainPuyo.getPuyoObj().transform.localPosition;
        }
    }

    public void puyoRight(bool moveShinyPuyo) {
        player.controlMainPuyo.getPuyoObj().transform.localPosition = new Vector3
                    (player.controlMainPuyo.getPuyoObj().transform.localPosition.x + 32, player.controlMainPuyo.getPuyoObj().transform.localPosition.y, player.controlMainPuyo.getPuyoObj().transform.localPosition.z);
        player.controlSubPuyo.getPuyoObj().transform.localPosition = new Vector3
            (player.controlSubPuyo.getPuyoObj().transform.localPosition.x + 32, player.controlSubPuyo.getPuyoObj().transform.localPosition.y, player.controlSubPuyo.getPuyoObj().transform.localPosition.z);
        player.controlMainPuyo.setPosition(new Vector2(player.controlMainPuyo.getPosition().x + 1, player.controlMainPuyo.getPosition().y));
        player.controlSubPuyo.setPosition(new Vector2(player.controlSubPuyo.getPosition().x + 1, player.controlSubPuyo.getPosition().y));
        if (moveShinyPuyo)
        {
            player.mainPuyoShinyObj.transform.localPosition = player.controlMainPuyo.getPuyoObj().transform.localPosition;
        }
    }

    public void puyoCounterclockwise()
    {
        int x = (int)player.controlMainPuyo.getPosition().x;
        int y = (int)player.controlMainPuyo.getPosition().y;
        if (player.subPuyoDirection==0) {
            if ((x==0 || player.puyoArr[x - 1, y] == null) && (x == 5 || player.puyoArr[x + 1, y] == null))
            {
                if (x == 0 || player.puyoArr[x - 1, y] != null)
                {
                    puyoRight(true);
                }
                player.subPuyoDirection = 3;
                subPuyoMoveToLeft();
            }
        }
        else if (player.subPuyoDirection == 1)
        {
            player.subPuyoDirection = 0;
            subPuyoMoveToTop();
        }
        else if (player.subPuyoDirection == 2)
        {
            if ((x == 5 || player.puyoArr[x + 1, y] == null) && (x == 0 || player.puyoArr[x - 1, y] == null))
            {
                if (x == 5 || player.puyoArr[x + 1, y] != null)
                {
                    puyoLeft(true);
                }
                player.subPuyoDirection = 1;
                subPuyoMoveToRight();
            }
        }
        else if (player.subPuyoDirection == 3)
        {
            if (y != 0)
            {
                player.subPuyoDirection = 2;
                subPuyoMoveToDown();
            }
        }

    }

    public void puyoClockwise()
    {
        //soundManager.playSound("rotateSound");
        int x = (int)player.controlMainPuyo.getPosition().x;
        int y = (int)player.controlMainPuyo.getPosition().y;
        if (player.subPuyoDirection == 0)
        {
            if ((x == 5 || player.puyoArr[x + 1, y] == null) && (x == 0 || player.puyoArr[x - 1, y] == null))
            {
                if (x == 5 || player.puyoArr[x + 1, y] != null)
                {
                    puyoLeft(true);
                }
                player.subPuyoDirection = 1;
                subPuyoMoveToRight();
            }
        }
        else if (player.subPuyoDirection == 1)
        {
            if (y != 0)
            {
                player.subPuyoDirection = 2;
                subPuyoMoveToDown();
            }
        }
        else if (player.subPuyoDirection == 2)
        {
            if ((x == 0 || player.puyoArr[x - 1, y] == null) && (x == 5 || player.puyoArr[x + 1, y] == null))
            {
                if (x == 0 || player.puyoArr[x - 1, y] != null)
                {
                    puyoRight(true);
                }
                player.subPuyoDirection = 3;
                subPuyoMoveToLeft();
            }
        }
        else if (player.subPuyoDirection == 3)
        {
            player.subPuyoDirection = 0;
            subPuyoMoveToTop();
        }

    }

    public void puyoArrange()
    {
        player.mainPuyoShinyObj.transform.localPosition = new Vector3(0, 208, 0);
        //If a puyo on the air, then make it fall to bottom.
        for (int y = 1; y < 13; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                if (player.puyoArr[x, y] != null)
                {
                    if (player.puyoArr[x, y - 1] == null)
                    {
                        FindObjectOfType<AudioManager>().Play("placePuyo");
                        GameObject tempPuyo = player.puyoArr[x, y].getPuyoObj();
                        tempPuyo.transform.localPosition = new Vector3(tempPuyo.transform.localPosition.x, tempPuyo.transform.localPosition.y - 32, tempPuyo.transform.localPosition.z);
                        player.puyoArr[x, y - 1] = player.puyoArr[x, y];
                        player.puyoArr[x, y] = null;
                        y = 1;
                        x = -1;
                    }
                }
            }
        }
    }

    public bool reachBottom(int x, int y)
    {
        if (y == 0)
        {
            return true;
        }
        if (player.puyoArr[x, y - 1] != null)
        {
            return true;
        }
        return false;
    }

    //0=left, 1=right
    public bool havingObstacle(int type, int x, int y)
    {
        if (y >= 13)
            return false;

        if (x <= 0 && type==0)
            return true;

        if (x >= 5 && type == 1)
            return true;

        if (type == 0) {
            if (player.puyoArr[x - 1, y] != null)
            {
                return true;
            }
        }
        else {
            if(player.puyoArr[x + 1, y] != null)
            {
                return true;
            }
        }
        return false;
    }

    public void subPuyoMoveToTop() {
        player.controlSubPuyo.getPuyoObj().transform.localPosition = new Vector3
            (player.controlMainPuyo.getPuyoObj().transform.localPosition.x, player.controlMainPuyo.getPuyoObj().transform.localPosition.y + 32, player.controlMainPuyo.getPuyoObj().transform.localPosition.z);
        player.controlSubPuyo.setPosition(new Vector2(player.controlMainPuyo.getPosition().x, player.controlMainPuyo.getPosition().y + 1));
    }

    public void subPuyoMoveToDown()
    {
        player.controlSubPuyo.getPuyoObj().transform.localPosition = new Vector3
            (player.controlMainPuyo.getPuyoObj().transform.localPosition.x, player.controlMainPuyo.getPuyoObj().transform.localPosition.y - 32, player.controlMainPuyo.getPuyoObj().transform.localPosition.z);
        player.controlSubPuyo.setPosition(new Vector2(player.controlMainPuyo.getPosition().x, player.controlMainPuyo.getPosition().y - 1));
    }

    public void subPuyoMoveToLeft()
    {
        player.controlSubPuyo.getPuyoObj().transform.localPosition = new Vector3
            (player.controlMainPuyo.getPuyoObj().transform.localPosition.x - 32, player.controlMainPuyo.getPuyoObj().transform.localPosition.y, player.controlMainPuyo.getPuyoObj().transform.localPosition.z);
        player.controlSubPuyo.setPosition(new Vector2(player.controlMainPuyo.getPosition().x - 1, player.controlMainPuyo.getPosition().y));
    }

    public void subPuyoMoveToRight()
    {
        player.controlSubPuyo.getPuyoObj().transform.localPosition = new Vector3
            (player.controlMainPuyo.getPuyoObj().transform.localPosition.x + 32, player.controlMainPuyo.getPuyoObj().transform.localPosition.y, player.controlMainPuyo.getPuyoObj().transform.localPosition.z);
        player.controlSubPuyo.setPosition(new Vector2(player.controlMainPuyo.getPosition().x + 1, player.controlMainPuyo.getPosition().y));
    }

    //quinns new function
    public bool countLinearPuyo()
    {
        bool deletion = false;
        //Link horizontal obj
        for (int y = 0; y < 13; y++)
        {
            bool emptyRow = true;
            for (int x = 0; x < 5; x++)
            {
                if (player.puyoArr[x, y] != null)
                {
                    emptyRow = false;
                    if (x < 4) { 
                        if (
                            player.puyoArr[x + 1, y] != null && player.puyoArr[x, y].getColor() == player.puyoArr[x + 1, y].getColor() &&
                            player.puyoArr[x + 2, y] != null && player.puyoArr[x, y].getColor() == player.puyoArr[x + 2, y].getColor()
                            )
                        {
                            player.puyoArr[x, y].setLinkStatus(ImageController.ELIMINATE_FACE);
                            player.puyoArr[x + 1, y].setLinkStatus(ImageController.ELIMINATE_FACE);
                            player.puyoArr[x + 2, y].setLinkStatus(ImageController.ELIMINATE_FACE);
                            deletion = true;
                        }
                    }
                }
            }
            if (emptyRow)
                break;
        }

        //Link vertical obj
        for (int y = 0; y < 13; y++)
        {
            bool emptyRow = true;
            for (int x = 0; x < 6; x++)
            {
                if (player.puyoArr[x, y] != null)
                {
                    emptyRow = false;
                    if (y < 12)
                    {
                        if (
                            player.puyoArr[x, y + 1] != null && player.puyoArr[x, y].getColor() == player.puyoArr[x, y + 1].getColor() &&
                            player.puyoArr[x, y + 2] != null && player.puyoArr[x, y].getColor() == player.puyoArr[x, y + 2].getColor()
                            )
                        {
                            player.puyoArr[x, y].setLinkStatus(ImageController.ELIMINATE_FACE);
                            player.puyoArr[x, y + 1].setLinkStatus(ImageController.ELIMINATE_FACE);
                            player.puyoArr[x, y + 2].setLinkStatus(ImageController.ELIMINATE_FACE);
                            deletion = true;
                        }
                    }
                }
            }
            if (emptyRow)
                break;
        }

        updatePuyoImage();
        return deletion;
    }

    public void linkSamePuyo()
    {
        //Link horizontal obj
        for (int y = 0; y < 13; y++)
        {
            bool emptyRow = true;
            for (int x = 0; x < 5; x++)
            {
                if (player.puyoArr[x, y] != null)
                {
                    emptyRow = false;
                    if (player.puyoArr[x + 1, y] != null && player.puyoArr[x, y].getColor() == player.puyoArr[x + 1, y].getColor())
                    {
                        if (ImageController.LINK_LEFT == player.puyoArr[x, y].getLinkStatus())
                            player.puyoArr[x, y].setLinkStatus(ImageController.LINK_RIGHT_LEFT);
                        else
                            player.puyoArr[x, y].setLinkStatus(ImageController.LINK_RIGHT);
                        player.puyoArr[x + 1, y].setLinkStatus(ImageController.LINK_LEFT);

                        setPuyoALinkList(player.puyoArr[x, y], player.puyoArr[x + 1, y]);
                    }
                }
            }
            if (emptyRow)
                break;
        }
        //Link vertical obj
        for (int y = 0; y < 13; y++)
        {
            bool emptyRow = true;
            for (int x = 0; x < 6; x++)
            {
                if (player.puyoArr[x, y] != null)
                {
                    emptyRow = false;
                    if (player.puyoArr[x, y + 1] != null && player.puyoArr[x, y].getColor() == player.puyoArr[x, y + 1].getColor())
                    {
                        switch (player.puyoArr[x, y + 1].getLinkStatus())
                        {
                            case ImageController.NORMAL:
                                player.puyoArr[x, y + 1].setLinkStatus(ImageController.LINK_DOWN);
                                break;
                            case ImageController.LINK_TOP:
                                player.puyoArr[x, y + 1].setLinkStatus(ImageController.LINK_TOP_DOWN);
                                break;
                            case ImageController.LINK_LEFT:
                                player.puyoArr[x, y + 1].setLinkStatus(ImageController.LINK_LEFT_DOWN);
                                break;
                            case ImageController.LINK_RIGHT:
                                player.puyoArr[x, y + 1].setLinkStatus(ImageController.LINK_RIGHT_DOWN);
                                break;
                            case ImageController.LINK_RIGHT_LEFT:
                                player.puyoArr[x, y + 1].setLinkStatus(ImageController.LINK_RIGHT_LEFT_DOWN);
                                break;
                            case ImageController.LINK_TOP_LEFT:
                                player.puyoArr[x, y + 1].setLinkStatus(ImageController.LINK_TOP_LEFT_DOWN);
                                break;
                            case ImageController.LINK_TOP_RIGHT:
                                player.puyoArr[x, y + 1].setLinkStatus(ImageController.LINK_TOP_RIGHT_DOWN);
                                break;
                            case ImageController.LINK_TOP_RIGHT_LEFT:
                                player.puyoArr[x, y + 1].setLinkStatus(ImageController.LINK_TOP_RIGHT_LEFT_DOWN);
                                break;
                        }
                        switch (player.puyoArr[x, y].getLinkStatus())
                        {
                            case ImageController.NORMAL:
                                player.puyoArr[x, y].setLinkStatus(ImageController.LINK_TOP);
                                break;
                            case ImageController.LINK_LEFT:
                                player.puyoArr[x, y].setLinkStatus(ImageController.LINK_TOP_LEFT);
                                break;
                            case ImageController.LINK_RIGHT:
                                player.puyoArr[x, y].setLinkStatus(ImageController.LINK_TOP_RIGHT);
                                break;
                            case ImageController.LINK_RIGHT_LEFT:
                                player.puyoArr[x, y].setLinkStatus(ImageController.LINK_TOP_RIGHT_LEFT);
                                break;
                            case ImageController.LINK_DOWN:
                                player.puyoArr[x, y].setLinkStatus(ImageController.LINK_TOP_DOWN);
                                break;
                            case ImageController.LINK_LEFT_DOWN:
                                player.puyoArr[x, y].setLinkStatus(ImageController.LINK_TOP_LEFT_DOWN);
                                break;
                            case ImageController.LINK_RIGHT_DOWN:
                                player.puyoArr[x, y].setLinkStatus(ImageController.LINK_TOP_RIGHT_DOWN);
                                break;
                            case ImageController.LINK_RIGHT_LEFT_DOWN:
                                player.puyoArr[x, y].setLinkStatus(ImageController.LINK_TOP_RIGHT_LEFT_DOWN);
                                break;
                        }

                        setPuyoALinkList(player.puyoArr[x, y], player.puyoArr[x, y + 1]);
                    }
                }
            }
            if (emptyRow)
                break;
        }
        updatePuyoImage();
    }

    public void updatePuyoImage()
    {
        //change img
        for (int y = 0; y < 13; y++)
        {
            bool emptyRow = true;
            for (int x = 0; x < 6; x++)
            {
                if (player.puyoArr[x, y] != null)
                {
                    //print("("+x+", "+y+")===>"+player.puyoArr[x, y].getLinkPuyoList().Count);
                    emptyRow = false;
                    ImageController.setPuyoImage(player.puyoArr[x, y], player.puyoArr[x, y].getLinkStatus());
                }
            }
            if (emptyRow)
                break;
        }
    }

    public static void setPuyoALinkList(Puyo puyoA, Puyo puyoB)
    {
        List<Puyo> puyoAList = puyoA.getLinkPuyoList();
        if (!puyoAList.Contains(puyoB))
        {
            puyoAList.Add(puyoB);
        }
        List<Puyo> puyoBList = puyoB.getLinkPuyoList();
        if (!puyoBList.Contains(puyoA))
        {
            puyoBList.Add(puyoA);
        }
        List<Puyo> puyoCList = puyoAList.Union(puyoBList).ToList<Puyo>();

        for (int i = 0; i < puyoAList.Count; i++)
        {
            puyoAList[i].setLinkPuyoList(puyoCList);
        }
        for (int i = 0; i < puyoBList.Count; i++)
        {
            puyoBList[i].setLinkPuyoList(puyoCList);
        }
    }

    public void resetPuyoStatusAndLinkPuyoList()
    {
        for (int y = 0; y < 13; y++)
        {
            bool emptyRow = true;
            for (int x = 0; x < 6; x++)
            {
                if (player.puyoArr[x, y] != null)
                {
                    emptyRow = false;
                    player.puyoArr[x, y].setLinkStatus(ImageController.NORMAL);
                    List<Puyo> puyoList = new List<Puyo>();
                    puyoList.Add(player.puyoArr[x, y]);
                    player.puyoArr[x, y].setLinkPuyoList(puyoList);
                }
            }
            if (emptyRow)
                break;
        }
    }

    public bool readyToEliminatePuyo()
    {
        bool haveLinkPuyo = false;
        for (int y = 0; y < 13; y++)
        {
            bool emptyRow = true;
            for (int x = 0; x < 6; x++)
            {
                if (player.puyoArr[x, y] != null)
                {
                    emptyRow = false;
                    //print(puyoArr[x, y].getColor()+" " +puyoArr[x, y].getLinkedPuyoList().Count);
                    if (player.puyoArr[x, y].getLinkPuyoList().Count >= 4)
                    {
                        //haveLinkPuyo = true;
                        //player.puyoArr[x, y].setLinkStatus(ImageController.ELIMINATE_FACE);
                    }
                }
            }
            if (emptyRow)
                break;
        }
        updatePuyoImage();
        return haveLinkPuyo;
    }

    public void eliminatePuyo()
    {
        for (int y = 0; y < 13; y++)
        {
            bool emptyRow = true;
            for (int x = 0; x < 6; x++)
            {
                if (player.puyoArr[x, y] != null)
                {
                    if (ImageController.ELIMINATE_FACE == player.puyoArr[x, y].getLinkStatus())
                    {
                        FindObjectOfType<AudioManager>().Play("chain");
                        Destroy(player.puyoArr[x, y].getPuyoObj());
                        player.puyoArr[x, y] = null;
                    }
                    emptyRow = false;
                }
            }
            if (emptyRow)
                break;
        }
    }

//This eliminateRow function has to be broken into pieces. Detect overflow in any instance of PuyoController, and delete rows in all instances of PuyoController
    public void checkTowerTooHigh()
    {
        int rowDeleteHeight = 0;
        if (isCameraLimit())
        {
            for (int y = 7; y < 12; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    if (player.puyoArr[x, y] != null)
                    {
                        rowDeleteHeight++;
                        x = 0;
                        break;
                    }
                }
            }
            eventManager.OnRowDeleteMethod(rowDeleteHeight);
            //send event
        }
    }

    public void deleteRows(int rowDeleteHeight)
    {
        if (isGameOver(rowDeleteHeight))
        {
            FindObjectOfType<AudioManager>().Play("dead");
            player.gameOverObj.SetActive(true);
            player.gameStatus = GameMaster.GameStatus.GamePause;
        }
        for (int y = 0; y < rowDeleteHeight; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                if (player.puyoArr[x, y] != null)
                {
                    FindObjectOfType<AudioManager>().Play("fall");
                    Destroy(player.puyoArr[x, y].getPuyoObj());
                    player.puyoArr[x, y] = null;
                }
            }
        }
        if (isGameOver(rowDeleteHeight))
        {
            FindObjectOfType<AudioManager>().Play("dead");
            player.gameOverObj.SetActive(true);
            player.gameStatus = GameMaster.GameStatus.GamePause;
            FindObjectOfType<AudioManager>().StopPlaying("music");
        }
    }

    public bool isCameraLimit() //camera threshold for deletion
    {
        if (player.controlMainPuyo.getPosition().y >= 7 || player.controlSubPuyo.getPosition().y >= 7)
            return true;

        return false;
    }

    public bool isGameOver(int rowDeleteHeight)
    {
        for (int y = 0; y <= rowDeleteHeight; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                if (player.puyoArr[x, y] != null){
                    return false;
                }
            }
        }
        return true;
    }

    public void hold()
    {
        int tempMainColor = player.puyoInventory.ElementAt(1).getColor();
        int tempSubColor = player.puyoInventory.ElementAt(0).getColor();
        player.puyoInventory.ElementAt(1).setColor(player.controlMainPuyo.getColor());
        player.puyoInventory.ElementAt(0).setColor(player.controlSubPuyo.getColor());
        player.controlMainPuyo.setColor(tempMainColor);
        player.controlSubPuyo.setColor(tempSubColor);
        ImageController.setPuyoImage(player.puyoInventory.ElementAt(1), ImageController.NORMAL);
        ImageController.setPuyoImage(player.puyoInventory.ElementAt(0), ImageController.NORMAL);
        ImageController.setPuyoImage(player.controlMainPuyo, ImageController.NORMAL);
        ImageController.setPuyoImage(player.controlSubPuyo, ImageController.NORMAL);
    }
}
