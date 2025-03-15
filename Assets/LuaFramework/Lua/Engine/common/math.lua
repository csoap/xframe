--[[--

根据系统时间初始化随机数种子，让后续的 math.random() 返回更随机的值

]]
function math.newrandomseed()
    local ok, socket = pcall(function()
        return require("socket")
    end)

    if ok then
        -- 如果集成了 socket 模块，则使用 socket.gettime() 获取随机数种子
        math.randomseed(socket.gettime() * 1000)
    else
        math.randomseed(os.time())
    end
    math.random()
    math.random()
    math.random()
    math.random()
end

--[[--

对数值进行四舍五入，如果不是数值则返回 0

@param number value 输入值

@return number

]]
function math.round(value)
    return math.floor(value + 0.5)
end

function math.angle2radian(angle)
	return angle*math.pi/180
end

function math.radian2angle(radian)
	return radian/math.pi*180
end


function math.clamp(value, min, max)
    return math.min(math.max(min, value), max)
end

