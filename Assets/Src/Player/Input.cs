using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

public class Input : MonoBehaviour
{
    private static Controls controls;

    public static Controls GetInstance()
    {
        if (controls == null)
        {
            controls = new Controls();
        }
        return controls;
    }
}
