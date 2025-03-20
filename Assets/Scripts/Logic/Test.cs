using UnityEngine;

public class Test : MonoBehaviour, INetStateListener
{
    public static void Create()
    {
        var go = new GameObject("Test");
        go.AddComponent<Test>();
    }
    void Awake()
    {
        // 设置网络状态监听
        ClientNet.instance.AddNetStateListener(this);
    }
    void Start()
    {
        // 测试网络
        // ClientNet.instance.AddNetStateListener(this);
        ClientNet.instance.Init();
        ClientNet.instance.Connect("10.1.65.226", 8888);
        // 测试资源
        // var panelObj = ResourceManager.instance.Instantiate<GameObject>(8);
        // panelObj.transform.SetParent(GlobalObjs.s_bgPanel, false);
        // // 测试I18N
        // GameLogger.LogGreen(I18N.GetStr(1));
        // // 测试音效
        // AudioMgr.instance.PlayMusic("bg.wav", true, true, true);
        // // 测试特效
        // ParticleManager.instance.PlayParticle(4, 3f, true);
        // DelayCallMgr.instance.Call(5, () =>
        // {
        //     ParticleManager.instance.PlayParticle(4, 3f, true);
        // });
    }

    public void OnNetStateChanged(NetState state, object param = null)
    {
        switch (state)
        {
            case NetState.ConnectSuccess:
                {
                    GameLogger.LogGreen("连接skynet服务端成功");
                    // LuaCall.CallFunc("Main.Send");
                    CSSayHello();
                    // CSSayHello();
                    break;
                }
            case NetState.ConnectFail:
                {
                    GameLogger.LogYellow("连接skynet服务端失败");
                    break;
                }
            default:
                {
                    GameLogger.Log("网络状态更新: " + state);
                    break;
                }
        }
    }
    // 客户端发送协议给服务端：C#层
    private void CSSayHello()
    {
        SpObject spObj = new SpObject(SpObject.ArgType.Table, "what", "hi, i am unity from lua");

        ClientNet.instance.Send("sayhello", spObj, (protoname, spobject) =>
        {
            GameLogger.LogCZZ("protoname:" + protoname);
            var error_code = SpObject.AsInt(spobject, "error_code", -1);
            var msg = SpObject.AsString(spobject, "msg", "");
            GameLogger.LogFormat("{0}: {1}", error_code, msg);

        });
    }
}
