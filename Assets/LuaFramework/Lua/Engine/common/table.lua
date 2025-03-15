local tRemove  = table.remove
local tInsert  = table.insert

function table.maxkeynum(t)
    local maxIdx = 0
    for k, v in pairs(t) do
        if k > maxIdx then
            maxIdx = k
        end
    end
    return maxIdx
end

--[[--

计算表格包含的字段数量

Lua table 的 "#" 操作只对依次排序的数值下标数组有效，table.nums() 则计算 table 中所有不为 nil 的值的个数。

@param table t 要检查的表格

@return integer

]]
function table.nums(t)
    local count = 0
    for k, v in pairs(t) do
        count = count + 1
    end
    return count
end

--[[--

返回指定表格中的所有键

~~~ lua

local hashtable = {a = 1, b = 2, c = 3}
local keys = table.keys(hashtable)
-- keys = {"a", "b", "c"}

~~~

@param table hashtable 要检查的表格

@return table

]]
function table.keys(hashtable)
    local keys = {}
    for k, v in pairs(hashtable) do
        keys[#keys + 1] = k
    end
    return keys
end

--[[--

返回指定表格中的所有值

~~~ lua

local hashtable = {a = 1, b = 2, c = 3}
local values = table.values(hashtable)
-- values = {1, 2, 3}

~~~

@param table hashtable 要检查的表格

@return table

]]
function table.values(hashtable)
    local values = {}
    for k, v in pairs(hashtable) do
        values[#values + 1] = v
    end
    return values
end

--[[--

将来源表格中所有键及其值复制到目标表格对象中，如果存在同名键，则覆盖其值

~~~ lua

local dest = {a = 1, b = 2}
local src  = {c = 3, d = 4}
table.merge(dest, src)
-- dest = {a = 1, b = 2, c = 3, d = 4}

~~~

@param table dest 目标表格
@param table src 来源表格

]]
function table.merge(dest, src)
    for k, v in pairs(src) do
        dest[k] = v
    end
end

function table.insertmerge(dest, src)
    for k, v in pairs(src) do
        if not dest[k] then
            dest[k] = v
        end
    end
end

--[[--

在目标表格的指定位置插入来源表格，如果没有指定位置则连接两个表格

~~~ lua

local dest = {1, 2, 3}
local src  = {4, 5, 6}
table.insertto(dest, src)
-- dest = {1, 2, 3, 4, 5, 6}

dest = {1, 2, 3}
table.insertto(dest, src, 5)
-- dest = {1, 2, 3, nil, 4, 5, 6}

~~~

@param table dest 目标表格
@param table src 来源表格
@param [integer begin] 插入位置

]]
function table.insertto(dest, src, begin)
    begin = checkint(begin)
    if begin <= 0 then
        begin = #dest + 1
    end

    local len = #src
    for i = 0, len - 1 do
        dest[i + begin] = src[i + 1]
    end
end

function table.copyrange(dest, src, begin, count)
    begin = begin or 1
    count = count or 0

    countSrc = #src
    local indexEnd = begin + count - 1
    for i = begin, indexEnd do
        if i > countSrc then
            break
        end

        tInsert(dest, src[i])
    end
end

function table.insertRange(dest, src)
    for i, v in ipairs(src) do
        tInsert(dest, v)
    end
end

--[[

从表格中查找指定值，返回其索引，如果没找到返回 -1

~~~ lua

local array = {"a", "b", "c"}
print(table.indexof(array, "b")) -- 输出 2

~~~

@param table array 表格
@param mixed value 要查找的值
@param [integer begin] 起始索引值

@return integer

]]
function table.indexof(array, value, begin)
    for i = begin or 1, #array do
        if array[i] == value then
            return i
        end
    end
    return -1
end

function table.valindexof(array, value, fieldName)
    for i = 1, #array do
        local rec = array[i]
        if rec[fieldName] == value then
            return i,rec
        end
    end
    return -1
end

--[[--

从表格中查找指定值，返回其 key，如果没找到返回 nil

~~~ lua

local hashtable = {name = "dualface", comp = "chukong"}
print(table.keyof(hashtable, "chukong")) -- 输出 comp

~~~

@param table hashtable 表格
@param mixed value 要查找的值

@return string 该值对应的 key

]]
function table.keyof(hashtable, value)
    for k, v in pairs(hashtable) do
        if v == value then
            return k
        end
    end
    return nil
end

--[[--

从表格中删除指定值，返回删除的值的个数

~~~ lua

local array = {"a", "b", "c", "c"}
print(table.removebyvalue(array, "c", true)) -- 输出 2

~~~

@param table array 表格
@param mixed value 要删除的值
@param [boolean removeall] 是否删除所有相同的值

@return integer

]]
function table.removebyvalue(array, value, removeall)
    local c, i, max = 0, 1, #array
    while i <= max do
        if array[i] == value then
            tRemove(array, i)
            c = c + 1
            i = i - 1
            max = max - 1
            if not removeall then
                break
            end
        end
        i = i + 1
    end
    return c
end

--[[--

对表格中每一个值执行一次指定的函数，并用函数返回值更新表格内容

~~~ lua

local t = {name = "dualface", comp = "chukong"}
table.map(t, function(v, k)
    -- 在每一个值前后添加括号
    return "[" .. v .. "]"
end)

-- 输出修改后的表格内容
for k, v in pairs(t) do
    print(k, v)
end

-- 输出
-- name [dualface]
-- comp [chukong]

~~~

fn 参数指定的函数具有两个参数，并且返回一个值。原型如下：

~~~ lua

function map_function(value, key)
    return value
end

~~~

@param table t 表格
@param function fn 函数

]]
function table.map(t, fn)
    for k, v in pairs(t) do
        t[k] = fn(v, k)
    end
end

--[[--

对表格中每一个值执行一次指定的函数，但不改变表格内容

~~~ lua

local t = {name = "dualface", comp = "chukong"}
table.walk(t, function(v, k)
    -- 输出每一个值
    print(v)
end)

~~~

fn 参数指定的函数具有两个参数，没有返回值。原型如下：

~~~ lua

function map_function(value, key)

end

~~~

@param table t 表格
@param function fn 函数

]]
function table.walk(t, fn)
    for k, v in pairs(t) do
        fn(v, k)
    end
end

--[[--

对表格中每一个值执行一次指定的函数，如果该函数返回 false，则对应的值会从表格中删除

~~~ lua

local t = {name = "dualface", comp = "chukong"}
table.filter(t, function(v, k)
    return v ~= "dualface" -- 当值等于 dualface 时过滤掉该值
end)

-- 输出修改后的表格内容
for k, v in pairs(t) do
    print(k, v)
end

-- 输出
-- comp chukong

~~~

fn 参数指定的函数具有两个参数，并且返回一个 boolean 值。原型如下：

~~~ lua

function map_function(value, key)
    return true or false
end

~~~

@param table t 表格
@param function fn 函数

]]
function table.filter(t, fn)
    for k, v in pairs(t) do
        if not fn(v, k) then
            t[k] = nil
        end
    end
end

--[[--

遍历表格，确保其中的值唯一

~~~ lua

local t = {"a", "a", "b", "c"} -- 重复的 a 会被过滤掉
local n = table.unique(t)

for k, v in pairs(n) do
    print(v)
end

-- 输出
-- a
-- b
-- c

~~~

@param table t 表格

@return table 包含所有唯一值的新表格

]]
function table.unique(t)
    local check = {}
    local n = {}
    for k, v in pairs(t) do
        if not check[v] then
            n[k] = v
            check[v] = true
        end
    end
    return n
end

function table.slice(t)
    local ret = {}
    for k, v in pairs(t) do
        ret[k] = v
    end

    return ret
end

function table.copy(ori_table, new_table)
    if type(ori_table) ~= "table" or type(new_table) ~= "table" or #new_table ~= 0 then
        return
    end

    for k, v in pairs(ori_table) do
        local vtype = type(v)
        if vtype == "table" then
            new_table[k] = {}
            table.copy(v, new_table[k])
        else
            new_table[k] = v
        end
    end
end

function table.reverse(t)
    local cloneTable = clone(t)

    for i = 1, #t do
        t[i] = cloneTable[#t - i + 1]
    end
end
--[[--
    倒叙返回table
]]
function table.reverseTable(tab)
    local tmp = {}
    for i = 1, #tab do
        tmp[i] = tRemove(tab)
    end
    return tmp
end

function table.clear(tab)
    for i, v in pairs(tab) do
        tab[i] = nil
    end
end

function table.clearArray(tab)
    if tab == nil then
        return
    end
    for i = #tab, 1, -1 do
        tab[i] = nil
    end
end

function table.unpack(values, index)
    if values then
        index = index or 1
        if #values >= index then
            return values[index], table.unpack(values, index + 1)
        end
    end
end

function table.IsEmpty(table)
    return next(table) == nil
end

--- 采用二分查找插入 测试代码 table_test.lua-->TestSortInsert
-- @number t: 源表，必须要有序，不然插入有问题
-- @number v: 插入的对象
-- @number f: 排序sort方法
-- @return pos,newTb 插入位置,新表
-- @usage table.binaryInsert(t, v, sort)
function table.binaryInsert(t, v, f)
    local len = #t -- length
    local left = 1
    local right = len
    while (left <= right) do
        local mid = left + math.floor((right - left) / 2)
        if f(t[mid], v) then
            left = mid + 1
        elseif f(v, t[mid]) then
            right = mid - 1
        else
            return mid, tInsert(t, mid, v)
        end
    end
    return left, tInsert(t, left, v)
end

--- 采用二分查找删除 测试代码 table_test.lua
-- @number t: 源表，必须要有序，不然删除有问题
-- @number v: 删除的对象
-- @number f: 排序sort方法
-- @return pos,newTb 删除位子,新表
-- @usage table.binaryRemove(t, v, sort)
function table.binaryRemove(t, v, f)
    local len = #t -- length
    local left = 1
    local right = len
    while (left <= right) do
        local mid = left + math.floor((right - left) / 2)
        if f(t[mid], v) then
            left = mid + 1
        elseif f(v, t[mid]) then
            right = mid - 1
        else
            return mid, tRemove(t, mid)
        end
    end
    return left, nil -- table.remove(t, left)
end

-- 采用二分查询
-- @number t: 源表，必须要有序
-- @number v: 比较的对象
-- @number f: 排序sort方法
-- @return find, v该位置的值
-- 返回第一个满足条件的值，即如果大到小，返回第一个大的数，反之第一个小的数
-- @usage local find, val = table.binaryFind(t, v, sort)
function table.binaryFind(t, v, f)
    local len = #t -- length
    local left = 1
    local right = len
    local find = 1
    while (left <= right) do
        local mid = left + math.floor((right - left) / 2)
        if f(t[mid], v) then
            left = mid + 1
            find = mid
        elseif f(v, t[mid]) then
            right = mid - 1
        else
            find = mid
            return mid, t[mid]
        end
    end
    return find, t[find]
end

--返回一个表中数组部分的随机元素
function table.getRandomElement(tab)
    if tab then
        local length = #tab
        if length > 0 then
            local index = math.random(1, length)
            return tab[index]
        end
    end
end

-- 升序
function table.ascFunc(l, r)
    return l < r
end

-- 降序
function table.descFunc(l, r)
    return l > r
end

--[[
    将一个数组分割成按比例分割成另一个数组
    比如：
   t = { key, value, key, value ... }

    keys = { "id", "count" }

    那么
    ret = { { id = key, count = value } }
--]]
function table.tableSplitFormat(t, keys)
    local ret = {}
    local n = #keys
    if n <= 0 then
        return t
    end
    local totalNum = #t

    assert(totalNum % n == 0, "配置表和分割数不对应")
    for i = 1, #t, n do
        local d = {}
        for j, key in ipairs(keys) do
            d[key] = t[i + j - 1]
        end

        ret[#ret + 1] = d
    end

    return ret
end

---有序表
function table.removeRange(src, filterTable, filterFunc)
    if type(filterTable) ~= "table" or type(filterFunc) ~= "function" then
        return
    end

    local function inTable(srcValue, tb)
        for _, v in ipairs(tb) do
            if filterFunc(srcValue, v) then
                return true
            end
            return false
        end
    end

    local index, newIndex, length = 1, 1, #src
    while index <= length do
        local v = src[index]
        src[index] = nil
        if not inTable(v, filterTable) then
            src[newIndex] = v
            newIndex = newIndex + 1
        end

        index = index + 1
    end
end

function table.FindKeyValue(target, element, comp)
    for idx, v in ipairs(target) do
        if comp(v, element) then
            return idx, v
        end
    end
    return nil
end

---有序表 src 除去 有序表 target（元素）剩下的元素组成的列表
---@generic T
---@alias comp fun(T,T):boolean
---@param target T[]
---@param src T[]
---@param comp comp
---@return  T[]
function table.GetDifferenceTable(src, target, comp)
    local tmp = {}
    for k, v in ipairs(src) do
        local key = table.FindKeyValue(target, v, comp)
        if not key then
            tInsert(tmp, v)
        end
    end

    return tmp
end

function table.FindMax(tb, func)
    if type(tb) ~= "table" or type(func) ~= "function" then
        return
    end

    local max = -10000000
    for k, v in pairs(tb) do
        local curr = func(v)
        if curr > max then
            max = curr
        end
    end

    return max
end

---@generic T
---@param t T
---@return T
function table.ShallowCopyArray(t)
    local tmp = {}
    for k, v in pairs(t) do
        tmp[k] = v
    end
    return tmp
end

function table.FilterByFunc(src, func)
    local index, newIndex, length = 1, 1, #src
    while index <= length do
        local v = src[index]
        src[index] = nil
        if func(v) then
            src[newIndex] = v
            newIndex = newIndex + 1
        end

        index = index + 1
    end
end

---获取首个元素，无序表
function table.GetFisrtElement(src)
    for k, v in pairs(src) do
        return k, v
    end
end

--把数组t转成字典，全都用value赋值
function table.ListToMap(t, value)
    local map = {}
    for _, v in pairs(t) do
        map[v] = value
    end
    return map
end

function table.AccumulatValue(t, offset)
    local count = #t
    if count == 0 then
        return
    end

    offset = offset or 0
    t[1] = t[1] + offset

    for i = 2, count do
        t[i] = t[i] + t[i - 1]
    end
end

---列表逆序（直接修改原始数据）
function table.ReverseOrder(t)
    local count = #t
    if count < 2 then
        return
    end

    local half = math.floor(count * 0.5)
    local hightIndex
    for i = 1, half do
        hightIndex = count - i + 1
        t[i], t[hightIndex] = t[hightIndex], t[i]
    end
end

---列表逆序（直接修改原始数据）
function table.ReverseOrderByRange(t, startIndex, endIndex)
    local count = endIndex - startIndex + 1
    if count < 2 then
        return
    end

    local half = math.floor(count * 0.5)
    local hightIndex
    for i = startIndex, half do
        hightIndex = count - i + 1
        t[i], t[hightIndex] = t[hightIndex], t[i]
    end
end


local function SortFuncAsc(l, r)
    return l.sort < r.sort
end
local function SortFuncDesc(l, r)
    return l.sort > r.sort
end
-- 按sort字段升序
function table.sortAscBySort(t)
    table.sort( t, SortFuncAsc )
end
-- 按sort字段降序
function table.sortDescBySort(t)
    table.sort( t, SortFuncDesc )
end