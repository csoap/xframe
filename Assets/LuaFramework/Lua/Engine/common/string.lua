string._htmlspecialchars_set = {}
string._htmlspecialchars_set["&"] = "&amp;"
string._htmlspecialchars_set["\""] = "&quot;"
string._htmlspecialchars_set["'"] = "&#039;"
string._htmlspecialchars_set["<"] = "&lt;"
string._htmlspecialchars_set[">"] = "&gt;"

--[[--

将特殊字符转为 HTML 转义符

~~~ lua

print(string.htmlspecialchars("<ABC>"))
-- 输出 &lt;ABC&gt;

~~~

@param string input 输入字符串

@return string 转换结果

]]
function string.htmlspecialchars(input)
    for k, v in pairs(string._htmlspecialchars_set) do
        input = string.gsub(input, k, v)
    end
    return input
end

--[[--

将 HTML 转义符还原为特殊字符，功能与 string.htmlspecialchars() 正好相反

~~~ lua

print(string.restorehtmlspecialchars("&lt;ABC&gt;"))
-- 输出 <ABC>

~~~

@param string input 输入字符串

@return string 转换结果

]]
function string.restorehtmlspecialchars(input)
    for k, v in pairs(string._htmlspecialchars_set) do
        input = string.gsub(input, v, k)
    end
    return input
end

--[[--

将字符串中的 \n 换行符转换为 HTML 标记

~~~ lua

print(string.nl2br("Hello\nWorld"))
-- 输出
-- Hello<br />World

~~~

@param string input 输入字符串

@return string 转换结果

]]
function string.nl2br(input)
    return string.gsub(input, "\n", "<br />")
end

--[[--

将字符串中的特殊字符和 \n 换行符转换为 HTML 转移符和标记

~~~ lua

print(string.nl2br("<Hello>\nWorld"))
-- 输出
-- &lt;Hello&gt;<br />World

~~~

@param string input 输入字符串

@return string 转换结果

]]
function string.text2html(input)
    input = string.gsub(input, "\t", "    ")
    input = string.htmlspecialchars(input)
    input = string.gsub(input, " ", "&nbsp;")
    input = string.nl2br(input)
    return input
end

--[[--

用指定字符或字符串分割输入字符串，返回包含分割结果的数组

~~~ lua

local input = "Hello,World"
local res = string.split(input, ",")
-- res = {"Hello", "World"}

local input = "Hello-+-World-+-Quick"
local res = string.split(input, "-+-")
-- res = {"Hello", "World", "Quick"}

~~~

@param string input 输入字符串
@param string delimiter 分割标记字符或字符串

@return array 包含分割结果的数组

]]
function string.split(input, delimiter)
    input = tostring(input)
    delimiter = tostring(delimiter)
    if (delimiter == '') then
        return false
    end
    local pos, arr = 0, {}
    -- for each divider found
    for st, sp in function()
        return string.find(input, delimiter, pos, true)
    end do
        table.insert(arr, string.sub(input, pos, st - 1))
        pos = sp + 1
    end
    table.insert(arr, string.sub(input, pos))
    return arr
end
-- 按长度切割字符串到list  场景：二进制字符串
function string.substringWithLength(input, length)
    input = tostring(input)
    local pos, nextPos, arr = 1, 1, {}
    local n = #input
    local calCount = math.floor(n / length)
    for i = 0, calCount,1 do
        nextPos = pos + length
		local sub =  string.sub(input, pos, nextPos-1)
        -- 最后一个如果是空的话，就不要塞进数组
		if sub ~="" then 
			table.insert(arr, sub)
		end
        pos = nextPos
    end
    return arr
end

--[[--

去除输入字符串头部的空白字符，返回结果

~~~ lua

local input = "  ABC"
print(string.ltrim(input))
-- 输出 ABC，输入字符串前面的两个空格被去掉了

~~~

空白字符包括：

-   空格
-   制表符 \t
-   换行符 \n
-   回到行首符 \r

@param string input 输入字符串

@return string 结果

@see string.rtrim, string.trim

]]
function string.ltrim(input)
    return string.gsub(input, "^[ \t\n\r]+", "")
end

--[[--

去除输入字符串尾部的空白字符，返回结果

~~~ lua

local input = "ABC  "
print(string.ltrim(input))
-- 输出 ABC，输入字符串最后的两个空格被去掉了

~~~

@param string input 输入字符串

@return string 结果

@see string.ltrim, string.trim

]]
function string.rtrim(input)
    return string.gsub(input, "[ \t\n\r]+$", "")
end

--[[--

去掉字符串首尾的空白字符，返回结果

@param string input 输入字符串

@return string 结果

@see string.ltrim, string.rtrim

]]
function string.trim(input)
    input = string.gsub(input, "^[ \t\n\r]+", "")
    return string.gsub(input, "[ \t\n\r]+$", "")
end

--[[--

将字符串的第一个字符转为大写，返回结果

~~~ lua

local input = "hello"
print(string.ucfirst(input))
-- 输出 Hello

~~~

@param string input 输入字符串

@return string 结果

]]
function string.ucfirst(input)
    return string.upper(string.sub(input, 1, 1)) .. string.sub(input, 2)
end

local function urlencodechar(char)
    return "%" .. string.format("%02X", string.byte(char))
end

--[[--

将字符串转换为符合 URL 传递要求的格式，并返回转换结果

~~~ lua

local input = "hello world"
print(string.urlencode(input))
-- 输出
-- hello%20world

~~~

@param string input 输入字符串

@return string 转换后的结果

@see string.urldecode

]]
function string.urlencode(input)
    -- convert line endings
    input = string.gsub(tostring(input), "\n", "\r\n")
    -- escape all characters but alphanumeric, '.' and '-'
    input = string.gsub(input, "([^%w%.%- ])", urlencodechar)
    -- convert spaces to "+" symbols
    return string.gsub(input, " ", "+")
end

--[[--

将 URL 中的特殊字符还原，并返回结果

~~~ lua

local input = "hello%20world"
print(string.urldecode(input))
-- 输出
-- hello world

~~~

@param string input 输入字符串

@return string 转换后的结果

@see string.urlencode

]]
function string.urldecode(input)
    input = string.gsub(input, "+", " ")
    input = string.gsub(input, "%%(%x%x)", function(h)
        return string.char(checknumber(h, 16))
    end)
    input = string.gsub(input, "\r\n", "\n")
    return input
end

--[[--

计算 UTF8 字符串的长度，每一个中文算一个字符

~~~ lua

local input = "你好World"
print(string.utf8len(input))
-- 输出 7

~~~

@param string input 输入字符串

@return integer 长度

]]

local arr = {0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc}
function string.utf8len(input)
    local len = string.len(input)
    local left = len
    local cnt = 0
    while left ~= 0 do
        local tmp = string.byte(input, -left)
        local i = #arr
        while arr[i] do
            if tmp >= arr[i] then
                left = left - i
                break
            end
            i = i - 1
        end
        cnt = cnt + 1
    end
    return cnt
end

--[[--

将数值格式化为包含千分位分隔符的字符串

~~~ lua

print(string.formatnumberthousands(1924235))
-- 输出 1,924,235

~~~

@param number num 数值

@return string 格式化结果

]]
function string.formatnumberthousands(num)
    local formatted = tostring(checknumber(num))
    local k
    while true do
        formatted, k = string.gsub(formatted, "^(-?%d+)(%d%d%d)", '%1,%2')
        if k == 0 then
            break
        end
    end
    return formatted
end

-- 字符串转Point
function string.toPoint(value)
    value = value or "0,0"
    local ret = {
        x = 0,
        y = 0
    }

    local nums = string.split(value, ",")
    if #nums == 2 then
        ret.x = checknumber(nums[1])
        ret.y = checknumber(nums[2])
    end

    return ret
end

-- 字符串转SexyColor
function string:toSexyColor(str)
    local value = checkint("0x" .. str)

    local data = {}
    while value ~= 0 do
        table.insert(data, value % 256)
        value = checkint(value / 256)
    end

    local ret = SexyColor()

    local index = 0
    if string.len(str) > 6 then
        index = 1
        ret.alpha = data[1] or 255
    end

    ret.blue = data[index + 1] or 0
    ret.green = data[index + 2] or 0
    ret.red = data[index + 3] or 0

    return ret
end

-- 判断utf8字符byte长度
-- 0xxxxxxx - 1 byte
-- 110yxxxx - 192, 2 byte
-- 1110yyyy - 225, 3 byte
-- 11110zzz - 240, 4 byte
local function chsize(char)
    if not char then
        print("not char")
        return 0
    elseif char > 240 then
        return 4
    elseif char > 225 then
        return 3
    elseif char > 192 then
        return 2
    else
        return 1
    end
end

-- 截取utf8 字符串
-- str:         要截取的字符串
-- startChar:   开始字符下标,从1开始
-- numChars:    要截取的字符长度
function string.utf8sub(str, startChar, numChars)
    local startIndex = 1
    while startChar > 1 do
        local char = string.byte(str, startIndex)
        startIndex = startIndex + chsize(char)
        startChar = startChar - 1
    end

    local currentIndex = startIndex

    while numChars > 0 and currentIndex <= #str do
        local char = string.byte(str, currentIndex)
        currentIndex = currentIndex + chsize(char)
        numChars = numChars - 1
    end
    return str:sub(startIndex, currentIndex - 1)
end

function string.IsNilOrEmpty(str)
    if not str or str == "" then
        return true
    end
    return false
end

function string.startswith(str, substr)
    if str == nil or substr == nil then
        return nil
    end
    if string.find(str, substr) ~= 1 then
        return false
    else
        return true
    end
end

function string.safeFormat(fmt, ...)
    local bok, err = xpcall(string.format, debug.traceback, fmt, ...)
    if not bok then
        log.Error("sys", "string.safeFormat error", err, fmt, ...)
        return ""
    end

    return err
end

function string.combineLan(arr)
    if arr == nil then
        return ""
    end
    local str = ""
    for i = 1, #arr do
        str = str .. i18n(arr[i])
    end
    return str
end

 function string.unicodeToUtf8(value)
    local str
    -- 先把Unicode值表示二进制转成Utf_8格式， 在string.char返回字符
    if value < 128 then -- （Utf_8格式 = [0xxxxxxx]）（128 = 1 0000000）
        str = string.char(value)
    elseif value < 2048 then -- （Utf_8格式 = [110xxxxx]-[10xxxxxx]） (2048 = 1 00000 000000）
        local byte1 = 128 + value % 64
        local byte2 = 192 + math.floor(value / 64)
        str = string.char(byte2, byte1)
    elseif value < 65536 then -- (Utf_8格式 = [1110xxxx]-[10xxxxxx]-[10xxxxxx]) (65536 = 1 0000 000000 000000)
        local byte1 = 128 + value % 64
        local byte2 = 128 + (math.floor(value / 64) % 32)
        local byte3 = 224 + (math.floor(value / 4096) % 16)
        str = string.char(byte3, byte2, byte1)
    end
    return str
end

function string.gsubToUnicode(str, old, newUnicode)
    return string.gsub( str,old, string.unicodeToUtf8(newUnicode))
end
