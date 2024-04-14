using System;

public class GlobalEventManager
{
    public static Action evtChangeUserData;

    public static void ChangeUserData()
    {
        evtChangeUserData?.Invoke();
    }
}
