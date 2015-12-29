using UnityEngine;
using System.Collections;
using uresponder;

public class View : MonoBehaviour {

    void Awake()
    {
        UResponder.add(LevelNotifications.UPDATE_LEVEL, new LevelNotifications.LevelUpdateDelegate(UpdateLevelView), 0, this);
    }

    private void UpdateLevelView(object input)
    {
        uint[] data = input as uint[];
        Debug.Log("> View : LevelNotifications ===> UpdateLevelView : " + data[0]);
    }
}
