using UnityEngine;
using System.Collections;
using uresponder;

public class ActionListenerFirst : MonoBehaviour {

    public static string ACTION_FIRST = "ACTION_FIRST";

    public delegate void UDelegate();

    void Awake()
    {
        UDelegate uDelegate = new UDelegate(FirstFunction);

        UResponder.add(ACTION_FIRST, uDelegate);
    }

    void FirstFunction()
    {
        Debug.Log("> URESPONCE ======> First");
    }
}
