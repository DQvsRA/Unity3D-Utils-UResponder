﻿using UnityEngine;
using System.Collections;
using uresponder;

public class Main : MonoBehaviour {

    private readonly string ACTION_ONE = "ACTION_ONE";

    public delegate void UDelegate();
    public delegate void UDelegate2(ArrayList args);

    public int counter1 = 0, counter2 = 0;

    void Awake()
    {
        UDelegate simpleDelegate1 = new UDelegate(MyFunc1);
        UDelegate2 simpleDelegate2 = new UDelegate2(MyFunc2);
        UResponder.add(ACTION_ONE, simpleDelegate1, 2);
        UResponder.add(ACTION_ONE, simpleDelegate2, 1);
    }

    private void MyFunc1()
    {
        counter1++;
        Debug.Log("Func 1 --> called from UResponder " + counter1 + " times" );
    }

    private void MyFunc2(ArrayList args)
    {
        counter2++;
        Debug.Log("Func 1 --> called from UResponder " + counter2 + " times");
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(PerformEvery());
	}
	
    IEnumerator PerformEvery()
    {
        Debug.Log("TEST: BEGIN");
        UResponder.dispatch(First.ACTION_FIRST);
        while (true)
        {
            if (UResponder.dispatch(ACTION_ONE, new ArrayList() { 1, "2" })) yield return new WaitForSeconds(1f);
            else break;
        }
        UResponder.dispatch(Second.ACTION_SECOND);
        Debug.Log("TEST: COMPLETE");
    }
}
