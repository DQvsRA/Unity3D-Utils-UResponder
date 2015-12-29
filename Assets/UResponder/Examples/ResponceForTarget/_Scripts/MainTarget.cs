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
        Debug.Log("> MainTarget : Process Level Data");
        UResponder.dispatch(LevelNotifications.UPDATE_LEVEL);
    }

    void Start () {
        StartCoroutine(PerformServer());
    }

    IEnumerator PerformServer()
    {
        Debug.Log("TEST: BEGIN =======================================");
        Debug.Log("");
        var server = Server.getInstance();
        server.getLevels();
        yield return new WaitForSeconds(3);
        server.postLevelResults();
        Debug.Log("");
        Debug.Log("TEST: COMPLETE =======================================");
    }
}
