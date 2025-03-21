//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class ParticleManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(ParticleManager), typeof(System.Object));
		L.RegFunction("PlayParticle", PlayParticle);
		L.RegFunction("ClearCache", ClearCache);
		L.RegFunction("New", _CreateParticleManager);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("instance", get_instance, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateParticleManager(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				ParticleManager obj = new ParticleManager();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: ParticleManager.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayParticle(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			ParticleManager obj = (ParticleManager)ToLua.CheckObject<ParticleManager>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
			bool arg2 = LuaDLL.luaL_checkboolean(L, 4);
			UnityEngine.GameObject o = obj.PlayParticle(arg0, arg1, arg2);
			ToLua.PushSealed(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearCache(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ParticleManager obj = (ParticleManager)ToLua.CheckObject<ParticleManager>(L, 1);
			obj.ClearCache();
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
			ToLua.PushObject(L, ParticleManager.instance);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

