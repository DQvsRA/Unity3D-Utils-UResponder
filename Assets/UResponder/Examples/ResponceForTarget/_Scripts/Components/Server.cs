
using System.Collections;
using System.Timers;
using uresponder;

public class Server {
    public delegate void ResponceDelegate(ArrayList responce);

    static private Server __instance = new Server();
    static public Server getInstance() { return __instance; }

    private Server()
    {

    }

    public void getLevels()
    {
        UResponder.dispatch(ServerResponces.GET_LEVELS, new ArrayList() { "ok", new uint[1, 3, 4, 5, 6]});
    }

    public void postLevelResults()
    {
        UResponder.dispatch(ServerResponces.POST_LEVEL_RESULT, new ArrayList() { "ok", new uint[9,6,3] });
    }
}
