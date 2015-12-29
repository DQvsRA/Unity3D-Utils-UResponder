using UnityEngine;
using System.Collections;
using uresponder;
using System;

public class MainTarget : MonoBehaviour {

    public Model model;
    
    void Awake()
    {
        UResponder.add(ServerResponces.GET_LEVELS, new Server.ResponceDelegate(ServerResponce_GetLevels), 0, this);
    }

    private void ServerResponce_GetLevels(ArrayList responce)
    {
        Debug.Log("> MainTarget : ServerResponce ===> GetLevels : " + responce);
    }

    // Use this for initialization
    void Start () {
        //StartCoroutine(PerformEvery());
	}
	
    //IEnumerator PerformEvery()
    //{
    //    Debug.Log("TEST: BEGIN =======================================");
    //    Debug.Log("");
    //    //UResponder.dispatch(ActionListenerFirst.ACTION_FIRST);
    //    while (true)
    //    {
    //        if (UResponder.dispatch(SERVER_RESPONCE__GET_LEVELS, new ArrayList() { 1, "2" })) yield return new WaitForSeconds(1f);
    //        else break;
    //    }
    //    //UResponder.dispatch(ActionListenerSecond.ACTION_SECOND);
    //    Debug.Log("");
    //    Debug.Log("TEST: COMPLETE =======================================");
    //}
}
