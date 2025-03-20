#define LUA_VERSION_5_3

using UnityEngine;
using LuaInterface;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace LuaFramework
{
    public class LuaManager
    {
        private static LuaManager _instance;
        public static LuaManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LuaManager();
            }

            return _instance;
        }

        public static LuaManager instance
        {
            get { return GetInstance(); }
        }

        private LuaState lua;
        //private LuaLooper loop = null;

        private LuaManager()
        {
            
            lua = new LuaState();
            
#if LUA_VERSION_5_3

#else
            LuaDLL.tolua_setflag(ToLuaFlags.USE_INT64, true);
            LuaDLL.tolua_setflag(ToLuaFlags.FLAG_UINT64, true);
#endif

            this.OpenLibs();
            lua.LuaSetTop(0);

            LuaBinder.Bind(lua);
            OpenBindLib();
            DelegateFactory.Init(); //todo 就是这个导致lua到C#action绑定不到
        }


        public void InitStart(Action okCb)
        {
            this.lua.Start();    //启动LUAVM
            this.StartMain(() =>
            {
                LuaLooper.GetInstance().OnInit(lua);
                if(null != okCb)
                    okCb();
            });

        }


        // cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson()
        {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        void StartMain(Action okCb)
        {
            if (AppConst.UseFileList)
            {
                //string outPutStr = "";
                string fileListStr = LuaFileUtils.Instance.ReadStringFromFile(AppConst.LuaFileListName);
                if (fileListStr != null)
                {
                    char[] sperateChars = { '\r', '\n' };
                    string[] files = fileListStr.Split(sperateChars);
                    int totalCnt = files.Length;
                    WalkCoroutine.DoWalk(0.03f, 10, (index) =>
                    {
                        if (index >= totalCnt)
                        {   
                            if (null != okCb)
                                okCb();
                            return false;
                        }
                        string fileName = files[index];
                        if (!string.IsNullOrEmpty(fileName) && !fileName.StartsWith("--"))
                        {
                            lua.DoFile(fileName);
                        }
                        return true;
                    });
                }
                else
                {
                    Debug.LogError("LuaManager StartMian Load filelist file not Exist ");
                }
            }
        }

        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        void OpenLibs()
        {
            lua.OpenLibs(LuaDLL.luaopen_pb); // lua-protobuf
            lua.OpenLibs(LuaDLL.luaopen_sproto_core); // sproto库
            lua.OpenLibs(LuaDLL.luaopen_protobuf_c); // pbc库
            lua.OpenLibs(LuaDLL.luaopen_lpeg); // lpeg库
// #if LUA_VERSION_5_3
//             lua.OpenLibs(LuaDLL.luaopen_bit32);
// #else
//             lua.OpenLibs(LuaDLL.luaopen_bit);
// #endif
            lua.OpenLibs(LuaDLL.luaopen_bit);

            lua.OpenLibs(LuaDLL.luaopen_socket_core);

            this.OpenCJson();
        }

        void OpenBindLib()
        {
            lua.BeginModule(null);
            lua.BeginModule("LuaFramework");
            WrapCSObj.Register(lua);
            lua.EndModule();
            lua.EndModule();
        }


        public LuaTable CreateLuaTable(int narr = 0, int nec = 0)
        {
            lua.LuaCreateTable(narr, nec);
            LuaTable tab = lua.CheckLuaTable(-1);

            return tab;
        }

        public LuaTable CheckLuaTable(int stackPos)
        {
            return lua.CheckLuaTable(stackPos);
        }

        // public object[] DoFile(string filename)
        // {
        //     return lua.DoFile(filename);
        // }

        // Update is called once per frame
        public void CallFunction(string funcName)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null)
            {
                func.Call();
            }
        }
        
        // public object[] CallFunction(string funcName, params object[] args)
        // {
        //     LuaFunction func = lua.GetFunction(funcName);
        //     if (func != null)
        //     {
        //         GameLogger.Log("czz args" + args.Length);
        //
        //         if (args != null && args.Length > 0)
        //         {
        //             
        //         }
        //         else
        //         {
        //             return func.Invoke<object[]>();
        //         }
        //         
        //         return null;
        //     }
        //     GameLogger.Log("function nil" + funcName);
        //     return null;
        // }
        
        public object[] CallFunction(string funcName, params object[] args)
        {
            LuaFunction func = lua.GetFunction(funcName);
            GameLogger.LogCZZ("funcName :" + funcName);
            if (func != null)
            {
                return func.CallArgs(args);
            }
            return null;
        }
        
        // public object[] CallFunction(string funcName, params object[] args)
        // {
        //     LuaFunction func = lua.GetFunction(funcName);
        //     if (func == null)
        //     {
        //         GameLogger.LogError($"Lua函数不存在: {funcName}");
        //         return null;
        //     }
        //
        //     try
        //     {
        //         // 1. 处理 null 和空参数
        //         // 1. 过滤 null 值并生成新数组
        //         List<object> validArgs = new List<object>();
        //         if (args != null)
        //         {
        //             foreach (object arg in args)
        //             {
        //                 if (arg != null) // 过滤掉 null
        //                 {
        //                     validArgs.Add(arg);
        //                 }
        //             }
        //         }
        //         object[] filteredArgs = validArgs.ToArray();
        //         GameLogger.Log($"调用 {funcName}，参数数量: {args.Length}");
        //
        //         // 2. 根据参数数量动态调用
        //         switch (filteredArgs.Length)
        //         {
        //             case 0:
        //                 return func.Invoke<object[]>();
        //             case 1:
        //                 return func.Invoke<object, object[]>(filteredArgs[0]);
        //             case 2:
        //                 return func.Invoke<object, object, object[]>(filteredArgs[0], filteredArgs[1]);
        //             case 3:
        //                 return func.Invoke<object, object, object, object[]>(filteredArgs[0], filteredArgs[1], filteredArgs[2]);
        //             // 扩展更多参数...
        //             default:
        //                 // 通用方案：反射调用任意数量参数（需性能优化）
        //                 return null;//InvokeWithReflection(func, args);
        //         }
        //     }
        //     catch (LuaException ex)
        //     {
        //         GameLogger.LogError($"Lua调用失败: {funcName}，错误: {ex.Message}");
        //         return null;
        //     }
        //     finally
        //     {
        //         func.Dispose();
        //     }
        // }
        //
        //
        
        public void CallFunction(string funcName, GameObject go)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null)
            {
                func.Call(go);
            }
        }
        
        public T CallFunctionArgs<T>(string funcName)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null)
            {
                T results = func.Invoke<T>(); // 直接传递参数并获取结果
                return results;
            }
            return default(T);
        }
        
        public T CallFunctionArgs<T>(string funcName, object param1)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null)
            {
                T results = func.Invoke<object, T>(param1); // 直接传递参数并获取结果
                return results;
            }
            return default(T);
        }
        
        public T CallFunctionArgs<T>(string funcName, object param1, object param2)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null)
            {
                T results = func.Invoke<object,object, T>(param1, param2); // 直接传递参数并获取结果
                return results;
            }
            return default(T);
        }
        
        public T CallFunctionArgs<T>(string funcName, object param1, object param2, object param3)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null)
            {
                T results = func.Invoke<object, object, object, T>(param1, param2, param3); // 直接传递参数并获取结果
                return results;
            }
            return default(T);
        }


        public LuaFunction GetLuaFunc(string funcName)
        {
            return lua.GetFunction(funcName);
        }

        public LuaFunction GetFunction(string funcName, bool beLogMiss = true)
        {
            return lua.GetFunction(funcName, beLogMiss);
        }

        // public string GetLuaFunDebugStr()
        // {
        //     return lua.GetLuaFunDebugStr();
        // }


        public void LuaGC()
        {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close()
        {
            if (lua != null)
            {
                lua.Dispose();
                lua = null;
            }
        }

        public LuaState GetState()
        {
            return lua;
        }

        /// <summary>
        /// 重载脚本
        /// </summary>
        /// <param name="luafilenames"></param>
        public void ReloadLuaFiles(string[] luafilenames)
        {
            for (int i = 0; i < luafilenames.Length; i++)
            {
                string fileName = luafilenames[i];
                if (!string.IsNullOrEmpty(fileName) && !fileName.StartsWith("--"))
                {
                    lua.DoFile(fileName);
                    GameLogger.LogGreen("Reload: " + fileName);
                }
            }
        }
    }
}

/// <summary>
/// 封装一个调用Lua的简短写法的接口
/// </summary>
public class LuaCall
{
    public static object[] CallFunc(string funcName, params object[] args)
    {
        return LuaFramework.LuaManager.instance.CallFunction(funcName, args);
    }

    public static void CallFunc(string funcName, GameObject go)
    {
        LuaFramework.LuaManager.instance.CallFunction(funcName, go);
    }
    public static void CallFunc(string funcName)
    {
        LuaFramework.LuaManager.instance.CallFunction(funcName);
    }
}