//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class PanelMgrWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(PanelMgr), typeof(System.Object));
		L.RegFunction("Init", Init);
		L.RegFunction("GetPanelById", GetPanelById);
		L.RegFunction("ShowPanel", ShowPanel);
		L.RegFunction("HidePanel", HidePanel);
		L.RegFunction("HideAllPanels", HideAllPanels);
		L.RegFunction("New", _CreatePanelMgr);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("instance", get_instance, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreatePanelMgr(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				PanelMgr obj = new PanelMgr();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: PanelMgr.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			PanelMgr obj = (PanelMgr)ToLua.CheckObject<PanelMgr>(L, 1);
			obj.Init();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPanelById(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			PanelMgr obj = (PanelMgr)ToLua.CheckObject<PanelMgr>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			BasePanel o = obj.GetPanelById(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowPanel(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			PanelMgr obj = (PanelMgr)ToLua.CheckObject<PanelMgr>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			LuaTable arg1 = ToLua.CheckLuaTable(L, 3);
			UnityEngine.Transform arg2 = (UnityEngine.Transform)ToLua.CheckObject<UnityEngine.Transform>(L, 4);
			BasePanel o = obj.ShowPanel(arg0, arg1, arg2);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HidePanel(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			PanelMgr obj = (PanelMgr)ToLua.CheckObject<PanelMgr>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.HidePanel(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HideAllPanels(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			PanelMgr obj = (PanelMgr)ToLua.CheckObject<PanelMgr>(L, 1);
			obj.HideAllPanels();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_instance(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, PanelMgr.instance);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

