﻿/*
Copyright (c) 2015-2017 topameng(topameng@qq.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text;

namespace LuaInterface
{
    public class LuaFileUtils
    {
        public static LuaFileUtils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LuaFileUtils();
                }

                return instance;
            }

            protected set
            {
                instance = value;
            }
        }

        //beZip = false 在search path 中查找读取lua文件。否则从外部设置过来bundel文件中读取lua文件
        // 编辑器环境设为false, 打包模式设为true
        public bool beZip = true;
        protected List<string> searchPaths = new List<string>();
        protected Dictionary<string, AssetBundle> zipMap = new Dictionary<string, AssetBundle>();

        private List<AssetBundle> m_luaBundleList = new List<AssetBundle>();
        protected static LuaFileUtils instance = null;

        public LuaFileUtils()
        {
            instance = this;
            //Editor下从项目目录里面读取文件
            if (Application.isEditor)
            {
                beZip = false;
            }
        }

        public void Init()
        {
            GameLogger.LogCZZ("beZip" + beZip);
            if (beZip)
            {
                //update (只有在更新时候用到)
                var luaUpdateAb = AssetBundleMgr.instance.LoadAssetBundle("lua_update.bundle");
                if (luaUpdateAb != null)
                {
                    m_luaBundleList.Add(luaUpdateAb);
                }

                var luaScriptsAb = AssetBundleMgr.instance.LoadAssetBundle("lua.bundle");
                if (luaScriptsAb != null)
                {
                    m_luaBundleList.Add(luaScriptsAb);
                }
            }
        }
        
        public virtual void Dispose()
        {
            if (instance != null)
            {
                instance = null;
                searchPaths.Clear();

                foreach (KeyValuePair<string, AssetBundle> iter in zipMap)
                {
                    iter.Value.Unload(true);
                }

                zipMap.Clear();
            }
        }

        //格式: 路径/?.lua
        public bool AddSearchPath(string path, bool front = false)
        {
            int index = searchPaths.IndexOf(path);

            if (index >= 0)
            {
                return false;
            }

            if (front)
            {
                searchPaths.Insert(0, path);
            }
            else
            {
                searchPaths.Add(path);
            }

            return true;
        }

        public bool RemoveSearchPath(string path)
        {
            int index = searchPaths.IndexOf(path);

            if (index >= 0)
            {
                searchPaths.RemoveAt(index);
                return true;
            }

            return false;
        }

        public void AddSearchBundle(string name, AssetBundle bundle)
        {
            zipMap[name] = bundle;
        }

        public string FindFile(string fileName)
        {
            if (fileName == string.Empty)
            {
                return string.Empty;
            }

            if (Path.IsPathRooted(fileName))
            {
                if (!fileName.EndsWith(".lua"))
                {
                    fileName += ".lua";
                }

                return fileName;
            }

            if (fileName.EndsWith(".lua"))
            {
                fileName = fileName.Substring(0, fileName.Length - 4);
            }

            string fullPath = null;

            for (int i = 0; i < searchPaths.Count; i++)
            {
                fullPath = searchPaths[i].Replace("?", fileName);

                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }

            return null;
        }

        public virtual byte[] ReadFile(string fileName)
        {
            if (!beZip)
            {
                string path = FindFile(fileName);
                byte[] str = null;

                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
#if !UNITY_WEBPLAYER
                    str = File.ReadAllBytes(path);
#else
                    throw new LuaException("can't run in web platform, please switch to other platform");
#endif
                }

                return str;
            }
            else
            {
                // return ReadZipFile(fileName);
                return ReadBytesFromAssetBundle(fileName);
            }
        }

        //读取非.lua文件
        public string ReadOtherFile(string fileName)
        {
            if (!beZip)
            {
                string str = null;
                //从项目里面读取
                string filePath = LuaConst.luaDir + "/" + fileName;
                if (File.Exists(filePath))
                {
#if !UNITY_WEBPLAYER
                    str = File.ReadAllText(filePath);
#else
                    throw new LuaException("can't run in web platform, please switch to other platform");
#endif
                }

                return str;

            }
            else
            {
                return ReadStringFromAssetBundle(fileName);
            }

        }
        
        public string ReadStringFromFile(string fileName)
        {
            if (!beZip)
            {
                string path = FindFile(fileName);
                string str = null;

                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
#if !UNITY_WEBPLAYER
                    str = File.ReadAllText(path);
#else
                    throw new LuaException("can't run in web platform, please switch to other platform");
#endif
                }

                return str;
            }
            else
            {
                return ReadStringFromAssetBundle(fileName);
            }
        }
        
        
        public virtual string FindFileError(string fileName)
        {
            if (Path.IsPathRooted(fileName))
            {
                return fileName;
            }

            if (fileName.EndsWith(".lua"))
            {
                fileName = fileName.Substring(0, fileName.Length - 4);
            }

            using (CString.Block())
            {
                CString sb = CString.Alloc(512);

                for (int i = 0; i < searchPaths.Count; i++)
                {
                    sb.Append("\n\tno file '").Append(searchPaths[i]).Append('\'');
                }

                sb = sb.Replace("?", fileName);

                if (beZip)
                {
                    int pos = fileName.LastIndexOf('/');

                    if (pos > 0)
                    {
                        int tmp = pos + 1;
                        sb.Append("\n\tno file '").Append(fileName, tmp, fileName.Length - tmp).Append(".lua' in ").Append("lua_");
                        tmp = sb.Length;
                        sb.Append(fileName, 0, pos).Replace('/', '_', tmp, pos).Append(".unity3d");
                    }
                    else
                    {
                        sb.Append("\n\tno file '").Append(fileName).Append(".lua' in ").Append("lua.unity3d");
                    }
                }

                return sb.ToString();
            }
        }

        byte[] ReadZipFile(string fileName)
        {
            AssetBundle zipFile = null;
            byte[] buffer = null;
            string zipName = null;

            using (CString.Block())
            {
                CString sb = CString.Alloc(256);
                sb.Append("lua");
                int pos = fileName.LastIndexOf('/');

                if (pos > 0)
                {
                    sb.Append("_");
                    sb.Append(fileName, 0, pos).ToLower().Replace('/', '_');
                    fileName = fileName.Substring(pos + 1);
                }

                if (!fileName.EndsWith(".lua"))
                {
                    fileName += ".lua";
                }

#if UNITY_5 || UNITY_5_3_OR_NEWER
                fileName += ".bytes";
#endif
                zipName = sb.ToString();
                zipMap.TryGetValue(zipName, out zipFile);
            }

            if (zipFile != null)
            {
#if UNITY_4_6 || UNITY_4_7
                TextAsset luaCode = zipFile.Load(fileName, typeof(TextAsset)) as TextAsset;
#else
                TextAsset luaCode = zipFile.LoadAsset<TextAsset>(fileName);
#endif
                if (luaCode != null)
                {
                    buffer = luaCode.bytes;
                    Resources.UnloadAsset(luaCode);
                }
            }

            return buffer;
        }

        public static string GetOSDir()
        {
            return LuaConst.osDir;
        }
        
        private string ReadStringFromAssetBundle(string fileName)
        {
            fileName = "Assets/luabundle/" + fileName;

            string bundleFileName = fileName + ".bytes";
            int bundleCount = m_luaBundleList.Count;
            for (int i = 0; i < bundleCount; i++)
            {
                AssetBundle ab = m_luaBundleList[i];
                TextAsset luaCode = ab.LoadAsset<TextAsset>(bundleFileName);
                if (luaCode == null)
                {
                    //require过来的 没有包含.lua后缀
                    string extendStr = Path.GetExtension(fileName);
                    if (string.IsNullOrEmpty(extendStr))
                    {
                        bundleFileName = fileName + ".lua.bytes";
                        luaCode = ab.LoadAsset<TextAsset>(bundleFileName);
                    }
                }

                string luaStr = null;
                if (luaCode != null)
                {
                    // 解密
                    var bytes = AESEncrypt.Decrypt(luaCode.bytes);
                    // 转字符串
                    luaStr = System.Text.Encoding.GetEncoding(65001).GetString(bytes);
                    Resources.UnloadAsset(luaCode);
                    return luaStr;
                }
            }
            return null;

        }
        
        private byte[] ReadBytesFromAssetBundle(string fileName)
        {
            //使用全名， 避免冲突
            fileName = "Assets/luabundle/" + fileName;

            string bundleFileName = fileName + ".bytes";
            int bundleCount = m_luaBundleList.Count;
            for (int i = 0; i < bundleCount; i++)
            {
                AssetBundle ab = m_luaBundleList[i];
                TextAsset luaCode = ab.LoadAsset<TextAsset>(bundleFileName);
                if (luaCode == null)
                {
                    //require过来的 没有包含.lua后缀
                    string extendStr = Path.GetExtension(fileName);
                    if (string.IsNullOrEmpty(extendStr))
                    {
                        bundleFileName = fileName + ".lua.bytes";
                        luaCode = ab.LoadAsset<TextAsset>(bundleFileName);
                    }
                }

                byte[] luaBytes = null;
                if (luaCode != null)
                {
                    // 解密
                    luaBytes =  AESEncrypt.Decrypt(luaCode.bytes);
                    Resources.UnloadAsset(luaCode);
                    return luaBytes;
                }
            }

            GameLogger.LogError("LuaFileUtils.ReadBytesFromAssetBundle " + fileName);
            return null;
        }

        
    }
}
