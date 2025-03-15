----其他的一些零碎函数

-- 游戏内向下取整
function game_floor(value)
    if not value then
        return 0
    end

    value = tonumber(value)

    if not value then
        return 0
    end

    return math.floor(value + 0.0000001)
end

-- 游戏内向上取整
function game_ceil(value)
    if not value or ("number" ~= type(value)) or value == 0 then
        return 0
    end

    return math.ceil(value - 0.0000001)
end

-- sign +-号 向上修正还是向下修正
function FixFLoat(value, sign)
    return value + 0.00001 * sign
end

-- 安全的格式化字符串
function string_format_safe(str, ...)
    local ok, result = xpcall(string.format, debug.traceback, str, ...)
    if not ok then
        printError("string.format[", str, "]格式错误!", result)
        return "", false
    end

    return result, true
end


--[[--

检查并尝试转换为数值，如果无法转换则返回 0

@param mixed value 要检查的值
@param [integer base] 进制，默认为十进制

@return number

]]
function checknumber(value, base)
    return tonumber(value, base) or 0
end

--[[--

检查并尝试转换为整数，如果无法转换则返回 0

@param mixed value 要检查的值

@return integer

]]
function checkint(value)
    return math.floor(checknumber(value) + 0.00001)
end

--[[--

检查并尝试转换为浮点数(浮点数位数可选)，如果无法转换则返回 0

@param mixed value 要检查的值

@return integer

]]
function checkfloat(value, floatLength)
    floatLength = floatLength or 0
    local multi = math.pow(10, floatLength)
    return checkint(value * multi) / multi
end


--[[--

检查值是否是一个表格，如果不是则返回一个空表格

@param mixed value 要检查的值

@return table

]]
function checktable(value)
    if type(value) ~= "table" then
        value = {}
    end
    return value
end

--[[--

如果表格中指定 key 的值为 nil，或者输入值不是表格，返回 false，否则返回 true

@param table hashtable 要检查的表格
@param mixed key 要检查的键名

@return boolean

]]
function isset(hashtable, key)
    local t = type(hashtable)
    return (t == "table" or t == "userdata") and hashtable[key] ~= nil
end

--[[--

深度克隆一个值

~~~ lua

-- 下面的代码，t2 是 t1 的引用，修改 t2 的属性时，t1 的内容也会发生变化
local t1 = {a = 1, b = 2}
local t2 = t1
t2.b = 3    -- t1 = {a = 1, b = 3} <-- t1.b 发生变化

-- clone() 返回 t1 的副本，修改 t2 不会影响 t1
local t1 = {a = 1, b = 2}
local t2 = clone(t1)
t2.b = 3    -- t1 = {a = 1, b = 2} <-- t1.b 不受影响

~~~

@param mixed object 要克隆的值

@return mixed

]]
function clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    return _copy(object)
end

-- 游戏内向上取整
function Mathf.Ceil(value)
    if not value or ("number" ~= type(value)) then
        return 0
    end

    return math.ceil(value - 0.0000001)
end

function Mathf.V2ExceedRange(x, y, range)
    return x * x + y * y >= range * range
end

--延迟导入表,只有真正使用到表的时候才会被require
--class类不能使用，setmetatable会覆盖父类
function delayImport(target, field, filePath)
    local delayTable = rawget(target, "__delayTable")
    if delayTable then
        delayTable[field] = filePath
        return
    end

    rawset(target, "__delayTable", { [field] = filePath })
    local meta = getmetatable(target) or {}
    meta.__index = function(t, k)
        local value = rawget(t, k)
        if value then
            return value
        end
        local delayTable = rawget(t, "__delayTable")
        local filePath = delayTable[k]
        if filePath then
            local data = importData(filePath)
            if data then
                rawset(t, k, data)
            end
            delayTable[k] = nil
            return data
        end
    end
    setmetatable(target, meta)
end

function Compare(l, r)
    if l > r then
        return 1
    elseif l == r then
        return 0
    else
        return -1
    end
end

---@return fun():void
function RunOnceFunc(func)
    local isFirst = true
    local result = nil
    return function()
        if isFirst then
            isFirst = false
            result = func()
            return result
        end
        return result
    end
end

-- 放入一整张配置，取出其中一条
function RandomOneData(cfg,rateKeyName)
    if not cfg then
        printAError("[RandomOneData]没有传入cfg!")
        return
    end

    local totalRate = 0
    rateKeyName = rateKeyName or "rate"
    local randomMap = {}
    for index, v in pairs(cfg) do
        totalRate = totalRate + v[rateKeyName]
        randomMap[index] = totalRate
    end

    local num = math.random(1, totalRate)
    for index, rate in pairs(randomMap) do
        if num <= rate then
            return cfg[index], index
        end
    end
end

-- local bit32 = require("bit32")
-- local band = bit32.band

-- 是否是偶数
-- function IsEven(int)
--     return band(int,1) == 0
-- end

local gsub = string.gsub
function CleaningColorAndHyperLink(str)
    str = gsub(str, "<hyl.->(.-)</hyl>", "%1")
    str =  gsub(str,"<color.->(.-)</color>", "%1")
    return str
end

function Debounce(fun, wait)
    local timeID = nil
    return function()
        TimerMgr.Stop(timeID)
        timeID = TimerMgr.Start(nil, fun, wait, 1)
    end
end

local bytemarkers = { {0x7FF,192}, {0xFFFF,224}, {0x1FFFFF,240} }
function StrFromUtf8CodePoint(decimal)
    if decimal<128 then return string.char(decimal) end
    local charbytes = {}
    for bytes,vals in ipairs(bytemarkers) do
        if decimal<=vals[1] then
            for b=bytes+1,2,-1 do
                local mod = decimal%64
                decimal = (decimal-mod)/64
                charbytes[b] = string.char(128+mod)
            end
            charbytes[1] = string.char(vals[2]+decimal)
            break
        end
    end
    return table.concat(charbytes)
end

-- function GetStringCodePoint(s, refResTable)
--     assert(type(s) == "string")
--     if not refResTable then
--         refResTable = {}
--     end

--     local res, seq, val = refResTable, 0, nil

--     for i = 1, #s do
--         local c = string.byte(s, i)
--         if seq == 0 then
--             table.insert(res, val)
--             seq = c < 0x80 and 1 or c < 0xE0 and 2 or c < 0xF0 and 3 or
--                     c < 0xF8 and 4 or c < 0xFC and 5 or c < 0xFE and 6 or
--                     error("invalid UTF-8 character sequence")
--             val = bit32.band(c, 2 ^ (8 - seq) - 1)
--         else
--             val = bit32.bor(bit32.lshift(val, 6), bit32.band(c, 0x3F))
--         end
--         seq = seq - 1
--     end
--     table.insert(res, val)

--     return res
-- end

local arr = { 0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc }
function ConvertStringToArray(input, refArrayTable)
    if "string" ~= type(input) and string.len(input) > 0 then
        return { input }
    end

    if not refArrayTable then
        refArrayTable = {}
    end

    local array = refArrayTable
    local len = string.len(input)
    local pos = 1
    while 0 < pos and pos <= len do
        local tmp = string.byte(input, pos)
        local i = #arr
        while arr[i] do
            if tmp >= arr[i] then
                local c = string.sub(input, pos, pos + i - 1)
                table.insert(array, c)
                pos = pos + i
                break
            end
            i = i - 1
        end
    end

    return array
end

function SafeCallFuc(target, funcName, param1)
    local func = target[funcName]
    if func then
        func(target, param1)
    end
end