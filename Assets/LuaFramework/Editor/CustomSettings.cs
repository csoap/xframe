﻿using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;

using BindType = ToLuaMenu.BindType;
// using Holoville.HOTween;
// using Holoville.HOTween.Plugins.Core;
using UnityEngine.UI;
using LuaFramework;
#if UNITY_EDITOR
using UnityEditor;
#endif

// namespace LuaFramework
// {
    public static class CustomSettings
    {
        public static string FrameworkPath = AppConst.FrameworkRoot;
        public static string saveDir = FrameworkPath + "/ToLua/Source/Generate/";
        public static string toluaBaseType = FrameworkPath + "/ToLua/BaseType/";
        public static string baseLuaDir = FrameworkPath + "/ToLua/Lua";
        public static string luaDir = FrameworkPath + "/Lua/";
        public static string injectionFilesPath = FrameworkPath + "/ToLua/Injection/";

        //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
        //unity 有些类作为sealed class, 其实完全等价于静态类
        public static List<Type> staticClassTypes = new List<Type>
        {
            typeof(UnityEngine.Application),
            typeof(UnityEngine.Time),
            typeof(UnityEngine.Screen),
            typeof(UnityEngine.SleepTimeout),
            typeof(UnityEngine.Input),
            typeof(UnityEngine.Resources),
            typeof(UnityEngine.Physics),
            typeof(UnityEngine.RenderSettings),
            // typeof(UnityEngine.QualitySettings),
            typeof(UnityEngine.GL),
            
        };

        //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
        public static DelegateType[] customDelegateList =
        {
            _DT(typeof(Action)),
            _DT(typeof(Action<int>)),
            _DT(typeof(Action<string, string>)),
            _DT(typeof(Action<bool>)),
            _DT(typeof(Action<float>)),
            _DT(typeof(Action<string>)),
            _DT(typeof(Action<object>)),
            _DT(typeof(System.Action<UnityEngine.GameObject>)),
            _DT(typeof(UnityEngine.Events.UnityAction)),
            _DT(typeof(UnityEngine.Events.UnityAction<int>)),
            _DT(typeof(UnityEngine.Events.UnityAction<bool>)),
            _DT(typeof(UnityEngine.Events.UnityAction<float>)),
            _DT(typeof(UnityEngine.Events.UnityAction<string>)),
            _DT(typeof(UnityEngine.Events.UnityAction<object>)),
            _DT(typeof(System.Predicate<int>)),
            _DT(typeof(System.Comparison<int>)),
            _DT(typeof(System.Func<int, int>)),




        };

        //在这里添加你要导出注册到lua的类型列表
        public static BindType[] customTypeList =
        {          
            #region  system
            _GT(typeof(System.Collections.Generic.List<byte[]>)),
            #endregion
            //------------------------为例子导出--------------------------------
            //_GT(typeof(TestEventListener)),
            //_GT(typeof(TestProtol)),
            //_GT(typeof(TestAccount)),
            //_GT(typeof(Dictionary<int, TestAccount>)).SetLibName("AccountMap"),
            //_GT(typeof(KeyValuePair<int, TestAccount>)),    
            //_GT(typeof(TestExport)),
            //_GT(typeof(TestExport.Space)),
            //-------------------------------------------------------------------        
            _GT(typeof(LuaInjectionStation)),
            _GT(typeof(InjectType)),
            _GT(typeof(Debugger)).SetNameSpace(null),        

    #if USING_DOTWEENING
            _GT(typeof(DG.Tweening.DOTween)),
            _GT(typeof(DG.Tweening.Tween)).SetBaseType(typeof(System.Object)).AddExtendType(typeof(DG.Tweening.TweenExtensions)),
            _GT(typeof(DG.Tweening.Sequence)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
            _GT(typeof(DG.Tweening.Tweener)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
            _GT(typeof(DG.Tweening.LoopType)),
            _GT(typeof(DG.Tweening.PathMode)),
            _GT(typeof(DG.Tweening.PathType)),
            _GT(typeof(DG.Tweening.RotateMode)),
            _GT(typeof(Component)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(Transform)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(Light)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(Material)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(Rigidbody)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(Camera)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(AudioSource)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            //_GT(typeof(LineRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            //_GT(typeof(TrailRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),    
    #else
                                            
            _GT(typeof(Component)),
            _GT(typeof(Transform)),
            _GT(typeof(Material)),
            // _GT(typeof(Light)),
            _GT(typeof(Rigidbody)),
            _GT(typeof(Camera)),
            // _GT(typeof(AudioSource)),
            //_GT(typeof(LineRenderer))
            //_GT(typeof(TrailRenderer))
    #endif

            //HOTween
            // _GT(typeof(HOTween)),
            // _GT(typeof(Sequence)),
            // _GT(typeof(Tweener)),
            // _GT(typeof(PlugColor)),

            _GT(typeof(Behaviour)),
            _GT(typeof(MonoBehaviour)),
            _GT(typeof(GameObject)),
            _GT(typeof(TrackedReference)),
            _GT(typeof(Application)),
            _GT(typeof(Physics)),
            _GT(typeof(Collider)),
            _GT(typeof(Time)),
            _GT(typeof(Texture)),
            _GT(typeof(Texture2D)),
            _GT(typeof(Shader)),
            _GT(typeof(Renderer)),
            _GT(typeof(WWW)),
            _GT(typeof(Screen)),
            _GT(typeof(CameraClearFlags)),
            _GT(typeof(AudioClip)),
            //_GT(typeof(AssetBundle)), 
            // _GT(typeof(ParticleSystem)),
            _GT(typeof(AsyncOperation)).SetBaseType(typeof(System.Object)),
            _GT(typeof(LightType)),
            _GT(typeof(SleepTimeout)),
            _GT(typeof(Animator)),
            _GT(typeof(Input)),
            _GT(typeof(KeyCode)),
            _GT(typeof(SkinnedMeshRenderer)),
            _GT(typeof(Space)),
            _GT(typeof(RuntimePlatform)),


            // _GT(typeof(MeshRenderer)),


            _GT(typeof(BoxCollider)),
            _GT(typeof(MeshCollider)),
            _GT(typeof(SphereCollider)),
            _GT(typeof(CharacterController)),
            _GT(typeof(CapsuleCollider)),

            _GT(typeof(Animation)),
            _GT(typeof(AnimationClip)).SetBaseType(typeof(UnityEngine.Object)),
            _GT(typeof(AnimationState)),
            _GT(typeof(AnimationBlendMode)),
            _GT(typeof(QueueMode)),
            _GT(typeof(PlayMode)),
            _GT(typeof(WrapMode)),

            // _GT(typeof(QualitySettings)),
            _GT(typeof(RenderSettings)),
            _GT(typeof(SkinWeights)),
            _GT(typeof(RenderTexture)),       
            
            // LuaFramework相关
            _GT(typeof(Util)),
            _GT(typeof(AppConst)),
            _GT(typeof(LuaHelper)),
            _GT(typeof(ByteBuffer)),
            _GT(typeof(LuaBehaviour)),


            // UGUI相关
            _GT(typeof(UGUITool)),
            _GT(typeof(RectTransform)),
            _GT(typeof(Button)),
            _GT(typeof(Button.ButtonClickedEvent)),
            _GT(typeof(Text)),
            _GT(typeof(Image)),
            _GT(typeof(RawImage)),
            _GT(typeof(Toggle)),
            _GT(typeof(Slider)),
            _GT(typeof(InputField)),
            _GT(typeof(InputField.OnChangeEvent)),
            _GT(typeof(Dropdown)),

            // TODO: 添加自定义的类
            _GT(typeof(ClientNet)),
            _GT(typeof(Cache)),
            _GT(typeof(NetworkManager)),
            _GT(typeof(EventDispatcher)),
            _GT(typeof(GlobalObjs)),
            _GT(typeof(ResourceManager)),
            _GT(typeof(PrefabBinder)),
            _GT(typeof(DelayCallMgr)),
            _GT(typeof(PanelMgr)),
            _GT(typeof(BasePanel)),
            _GT(typeof(SpriteManager)),
            _GT(typeof(ParticleManager)),
            _GT(typeof(AnimationEventTrigger)),
            _GT(typeof(I18N)),
            _GT(typeof(LanguageMgr)),
            _GT(typeof(GuideMaskBhv)),
            _GT(typeof(AppQuitDefend)),
            // _GT(typeof(LoopScrollRect))
            _GT(typeof(DelegateTest)),
        };

        private static Type NonEdtType(string className)
        {
            return Type.GetType(className + ",Assembly-CSharp");
        }

        public static List<Type> dynamicList = new List<Type>()
        {
            typeof(MeshRenderer),


            typeof(BoxCollider),
            // typeof(MeshCollider),
            // typeof(SphereCollider),
            // typeof(CharacterController),
            // typeof(CapsuleCollider),

            typeof(Animation),
            typeof(AnimationClip),
            typeof(AnimationState),

            typeof(SkinWeights),
            typeof(RenderTexture),
            // typeof(Rigidbody),
        };

        //重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
        //使用方法参见例子14
        public static List<Type> outList = new List<Type>()
        {

        };
        
        //ngui优化，下面的类没有派生类，可以作为sealed class
        public static List<Type> sealedList = new List<Type>()
        {
            /*typeof(Transform),
            typeof(UIRoot),
            typeof(UICamera),
            typeof(UIViewport),
            typeof(UIPanel),
            typeof(UILabel),
            typeof(UIAnchor),
            typeof(UIAtlas),
            typeof(UIFont),
            typeof(UITexture),
            typeof(UISprite),
            typeof(UIGrid),
            typeof(UITable),
            typeof(UIWrapGrid),
            typeof(UIInput),
            typeof(UIScrollView),
            typeof(UIEventListener),
            typeof(UIScrollBar),
            typeof(UICenterOnChild),
            typeof(UIScrollView),        
            typeof(UIButton),
            typeof(UITextList),
            typeof(UIPlayTween),
            typeof(UIDragScrollView),
            typeof(UISpriteAnimation),
            typeof(UIWrapContent),
            typeof(TweenWidth),
            typeof(TweenAlpha),
            typeof(TweenColor),
            typeof(TweenRotation),
            typeof(TweenPosition),
            typeof(TweenScale),
            typeof(TweenHeight),
            typeof(TypewriterEffect),
            typeof(UIToggle),
            typeof(Localization),*/
        };
        
        public static BindType _GT(Type t)
        {
            return new BindType(t);
        }

        public static DelegateType _DT(Type t)
        {
            return new DelegateType(t);
        }    


        [MenuItem("Lua/Attach Profiler", false, 151)]
        static void AttachProfiler()
        {
            if (!Application.isPlaying)
            {
                EditorUtility.DisplayDialog("警告", "请在运行时执行此功能", "确定");
                return;
            }

            LuaClient.Instance.AttachProfiler();
        }

        [MenuItem("Lua/Detach Profiler", false, 152)]
        static void DetachProfiler()
        {
            if (!Application.isPlaying)
            {            
                return;
            }

            LuaClient.Instance.DetachProfiler();
        }
}

