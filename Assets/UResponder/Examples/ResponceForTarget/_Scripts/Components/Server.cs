
using System.Collections;
using System.Timers;

public class Server {
    public delegate void ResponceDelegate(ArrayList responce);

    static private Server __instance = new Server();
    static public Server getInstance() { return __instance; }

    private Server()
    {

    }
}
