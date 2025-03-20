-- 入口脚本

Main = Main or {}
local this = Main

-- 测试lua配置加载
local TestLuaCfg = require "Config/TestLuaCfg"
require("Engine.common.init")
require("Network.HeartMgr")
function Main.Init()
    -- HeartMgr = {}
    log("Lua Main.Init")
    -- 红点系统
    RedpointTree.Init()

    -- 树初始化
    TreeLogic.Init()
    
    -- 输出配置表
    LuaUtil.PrintTable(TestLuaCfg)
end

function Main.Start()
    log("Lua Main.Start")

    -- 显示登录界面
    LoginPanel.Show()
end

function Main.Send()
    -- 客户端发送协议给服务端：Lua层
    Network.SendData("sayhello", { what = "hi, i am unity from lua" }, function(data)
            log("on response: " .. data.error_code .. " " .. data.msg)
        end
    )
end

-- endregion
