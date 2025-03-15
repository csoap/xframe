public class DelegateTest
{
    public static void TestAction(System.Action<string> callback)
    {
        callback("Hello from C#");
    }
}