using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumScript : MonoBehaviour
{

    public enum ObjectState { Empty, Apple, Fast, Slow, Swap, Head, Body, Frame };
    public GameObject GameObject;

    ObjectState _currentState;

    public ObjectState ReturnState()
    {
        return _currentState;       
    }

    public void SetState(ObjectState CurrentState)
    {

        for(int i =0; i < GameObject.transform.childCount; i++)
        {
            GameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        switch (CurrentState)
        {
            case ObjectState.Empty:
                GameObject.transform.GetChild(0).gameObject.SetActive(true);
                _currentState = ObjectState.Empty;
                break;
            case ObjectState.Apple:
                GameObject.transform.GetChild(1).gameObject.SetActive(true);
                _currentState = ObjectState.Apple;
                break;
            case ObjectState.Fast:
                GameObject.transform.GetChild(2).gameObject.SetActive(true);
                _currentState = ObjectState.Fast;
                break;
            case ObjectState.Slow:
                GameObject.transform.GetChild(3).gameObject.SetActive(true);
                _currentState = ObjectState.Slow;
                break;
            case ObjectState.Swap:
                GameObject.transform.GetChild(4).gameObject.SetActive(true);
                _currentState = ObjectState.Swap;
                break;
            case ObjectState.Head:
                GameObject.transform.GetChild(5).gameObject.SetActive(true);
                _currentState = ObjectState.Head;
                break;
            case ObjectState.Body:
                GameObject.transform.GetChild(6).gameObject.SetActive(true);
                _currentState = ObjectState.Body;
                break;
            case ObjectState.Frame:
                GameObject.transform.GetChild(7).gameObject.SetActive(true);
                _currentState = ObjectState.Frame;
                break;
        }
        
    }
}
