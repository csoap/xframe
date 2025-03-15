-- 生成只读表 table.readOnly
enum = function ( t )
    local proxy = {}
    local mt = {
        __index = function( _, key )            
            if t[key] ~= nil then
                return t[key]
            else
                -- error(string.format("not exist key:", key), 2)
                return nil
            end
        end,
    
        __newindex = function( _, key, value ) 
            error("attempt to update a read-only table", 2)
        end,

        tospecialstring = function(level, nometa)
            return "enumdetail = " .. tostring(t, nometa, level)
        end
    }

    setmetatable(proxy, mt)
    return proxy
end
