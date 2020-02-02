using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagerPuyo : MonoBehaviour
{
    public delegate void DeleteRows(int rowNumber);

    public event DeleteRows OnRowDeleteEvent;
    
    public void OnRowDeleteMethod(int rowNumber)
    {
        if (OnRowDeleteEvent != null){
            OnRowDeleteEvent(rowNumber);
        }
    }
}
