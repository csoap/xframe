//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UnityEngine_Events_UnityEvent_stringWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.Events.UnityEvent<string>), typeof(UnityEngine.Events.UnityEventBase), "UnityEvent_string");
		L.RegFunction("AddListener", AddListener);
		L.RegFunction("RemoveListener", RemoveListener);
		L.RegFunction("Invoke", Invoke);
		L.RegFunction("New", _CreateUnityEngine_Events_UnityEvent_string);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_Events_UnityEvent_string(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				UnityEngine.Events.UnityEvent<string> obj = new UnityEngine.Events.UnityEvent<string>();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UnityEngine.Events.UnityEvent<string>.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddListener(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Events.UnityEvent<string> obj = (UnityEngine.Events.UnityEvent<string>)ToLua.CheckObject<UnityEngine.Events.UnityEvent<string>>(L, 1);
			UnityEngine.Events.UnityAction<string> arg0 = (UnityEngine.Events.UnityAction<string>)ToLua.CheckDelegate<UnityEngine.Events.UnityAction<string>>(L, 2);
			obj.AddListener(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveListener(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Events.UnityEvent<string> obj = (UnityEngine.Events.UnityEvent<string>)ToLua.CheckObject<UnityEngine.Events.UnityEvent<string>>(L, 1);
			UnityEngine.Events.UnityAction<string> arg0 = (UnityEngine.Events.UnityAction<string>)ToLua.CheckDelegate<UnityEngine.Events.UnityAction<string>>(L, 2);
			obj.RemoveListener(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Invoke(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Events.UnityEvent<string> obj = (UnityEngine.Events.UnityEvent<string>)ToLua.CheckObject<UnityEngine.Events.UnityEvent<string>>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			obj.Invoke(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

