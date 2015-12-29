using UnityEngine;
using System.Collections;
using uresponder;

public class Model : MonoBehaviour {
    
    void Awake()
    {
        UResponder.add(ServerResponces.GET_LEVELS, new Server.ResponceDelegate(ServerResponce_GetLevels), 0, this);
        UResponder.add(ServerResponces.POST_LEVEL_RESULT, new Server.ResponceDelegate(ServerResponce_PostLevelResult), 0, this);
    }

	public void ServerResponce_GetLevels(ArrayList responce)
    {
        Debug.Log("> Model : ServerResponce ===> GetLevels : " + responce);
    }

    public void ServerResponce_PostLevelResult(ArrayList responce)
    {
        Debug.Log("> Model : ServerResponce ===> PostLevelResult :  " + responce);
    }
}
