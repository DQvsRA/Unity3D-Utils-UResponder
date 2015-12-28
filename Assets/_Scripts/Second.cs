using UnityEngine;
using System.Collections;
using uresponder;

public class Second : MonoBehaviour {

    public static string ACTION_SECOND = "ACTION_SECOND";

    public delegate void UDelegate();

    void Awake()
    {
        UDelegate uDelegate = new UDelegate(SecondFunction);

        UResponder.add(ACTION_SECOND, uDelegate);
    }

	// Use this for initialization
	void Start () {
	
	}

    void SecondFunction()
    {
        Debug.Log("> Second ======> URESPONCE");
    }

}
