using Puerts;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// 如果你全ts/js编程，可以参考这份自动化配置
/// </summary>
[Configure]
public class PuertsConfig
{
    [Typing]
    static IEnumerable<Type> Typeing
    {
        get
        {
            return new List<Type>()
            {
                //仅生成ts接口, 不生成C#静态代码
                typeof(UnityEngine.ArticulationReducedSpace)
            };
        }
    }
    [BlittableCopy]
    static IEnumerable<Type> Blittables
    {
        get
        {
            return new List<Type>()
            {
                //打开这个可以优化Vector3的GC，但需要开启unsafe编译
                //typeof(Vector3),
            };
        }
    }
    static IEnumerable<Type> Bindings
    {
        get
        {
            return new List<Type>()
            {
               //直接指定的类型
                typeof(JsEnv),
                typeof(ILoader),
            };
        }
    }
    [Binding]
    static IEnumerable<Type> DynamicBindings
    {
        get
        {

            List<string> namespaces = new List<string>()
{
// "UnityEngine",
"FairyGUI",
"FairyGUI.Utils",
// "Unity.Entities",
// "Unity.Transforms",
// "Unity.Jobs",
"System.Threading.Tasks",
"UnityEngine.SceneManagement",
};


            //     List<string> namespaces2 = new List<string>()
            //   {
            //       "FairyGUI",
            //       "FairyGUI.Utils",
            //   };

            var result = new List<Type>();
            Assembly[] ass = AppDomain.CurrentDomain.GetAssemblies();
            result.AddRange((from assembly in ass
                             where !(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder)
                             from type in assembly.GetExportedTypes()
                             where type.Namespace != null && namespaces.Contains(type.Namespace) && !IsExcluded(type)
                                   && type.BaseType != typeof(MulticastDelegate) && !type.IsEnum //&&type!=typeof(FairyGUI)
                                    && type.Namespace != "UnityEngine.ArticulationReducedSpace" && type.FullName != "UnityEngine.ArticulationReducedSpace+<x>e__FixedBuffer"

                             select type));

            // foreach (var type in result)
            // {
            //     UnityEngine.Debug.Log(type.Namespace + "-" + type.FullName);
            // }

            // var unityTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
            //                   where !(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder)
            //                   from type in assembly.GetExportedTypes()
            //                   where type.Namespace != null && namespaces.Contains(type.Namespace) && !IsExcluded(type)
            //                   select type);
            // string[] customAssemblys = new string[] {
            //     "Assembly-CSharp",
            // };
            // var customTypes = (from assembly in customAssemblys.Select(s => Assembly.Load(s))
            //                    where !(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder)
            //                    from type in assembly.GetExportedTypes()
            //                    where (type.Namespace == null || (!type.Namespace.StartsWith("Puerts") && !IsExcluded(type))) && type.Namespace != "UnityEngine.ArticulationReducedSpace"
            //                    select type);

            // foreach (var type in customTypes)
            // {
            //     UnityEngine.Debug.Log("qqq= " + type.Namespace + "-" + type.FullName);
            // }
            result.Add(typeof(UnityEngine.MonoBehaviour));
            result.Add(typeof(UnityEngine.Component));
            result.Add(typeof(UnityEngine.Object));
            result.Add(typeof(UnityEngine.Transform));
            result.Add(typeof(EventCenter));
            result.Add(typeof(Timer));
            result.Add(typeof(TSEvent));
            result.Add(typeof(TSMain));
            result.Add(typeof(Log));
            result.Add(typeof(AddressablesMgr));
            result.Add(typeof(System.String));
            result.Add(typeof(System.Object));
            result.Add(typeof(System.Array));
            result.Add(typeof(GameStart.UI_GameStartView));
            result.Add(typeof(UnityEngine.Animator));
            result.Add(typeof(UnityEngine.PlayerPrefs));
            result.Add(typeof(UnityEngine.GameObject));
            result.Add(typeof(UnityEngine.TextAsset));
            result.Add(typeof(UnityEngine.Physics));
            result.Add(typeof(UnityEngine.Vector3));
            result.Add(typeof(UnityEngine.Quaternion));
            result.Add(typeof(UnityEngine.Time));
            result.Add(typeof(UnityEngine.Rigidbody));
            result.Add(typeof(UnityEngine.MeshCollider));
            result.Add(typeof(UnityEngine.Renderer));
            result.Add(typeof(UnityEngine.Material));
            result.Add(typeof(UnityEngine.Color));
            result.Add(typeof(UnityEngine.MeshRenderer));
            result.Add(typeof(UnityEngine.Camera));
            result.Add(typeof(UnityEngine.Screen));
            result.Add(typeof(UnityEngine.Vector2));
            result.Add(typeof(UnityEngine.Rect));
            result.Add(typeof(UnityEngine.Application));
            // result.Add(typeof(UnityEngine.SceneManagement))
            // result.Add(typeof(Unity.Entities.IComponentData));

            // result.Add(typeof(Unity.Entities.GameObjectConversionSettings));
            // result.Add(typeof(Unity.Entities.EntityManager));
            // result.Add(typeof(Unity.Jobs.IJob));
            // result.Add(typeof(Unity.Entities.Entity));
            // result.Add(typeof(Unity.Entities.ComponentSystem));

            return result
                // .Concat(customTypes)
                .Concat(Bindings)
                .Distinct();
        }
    }
    static bool IsExcluded(Type type)
    {
        if (type == null)
            return false;

        string assemblyName = Path.GetFileName(type.Assembly.Location);
        if (excludeAssemblys.Contains(assemblyName))
            return true;

        string fullname = type.FullName != null ? type.FullName.Replace("+", ".") : "";
        if (excludeTypes.Contains(fullname))
            return true;
        return IsExcluded(type.BaseType);
    }
    //需要排除的程序集
    static List<string> excludeAssemblys = new List<string>{
        "UnityEditor.dll",
        "Assembly-CSharp-Editor.dll",
    };
    //需要排除的类型
    static List<string> excludeTypes = new List<string>
    {
        "UnityEngine.iPhone",
        "UnityEngine.iPhoneTouch",
        "UnityEngine.iPhoneKeyboard",
        "UnityEngine.iPhoneInput",
        "UnityEngine.iPhoneAccelerationEvent",
        "UnityEngine.iPhoneUtils",
        "UnityEngine.iPhoneSettings",
        "UnityEngine.AndroidInput",
        "UnityEngine.AndroidJavaProxy",
        "UnityEngine.BitStream",
        "UnityEngine.ADBannerView",
        "UnityEngine.ADInterstitialAd",
        "UnityEngine.RemoteNotification",
        "UnityEngine.LocalNotification",
        "UnityEngine.NotificationServices",
        "UnityEngine.MasterServer",
        "UnityEngine.Network",
        "UnityEngine.NetworkView",
        "UnityEngine.ParticleSystemRenderer",
        "UnityEngine.ParticleSystem.CollisionEvent",
        "UnityEngine.ProceduralPropertyDescription",
        "UnityEngine.ProceduralTexture",
        "UnityEngine.ProceduralMaterial",
        "UnityEngine.ProceduralSystemRenderer",
        "UnityEngine.TerrainData",
        "UnityEngine.HostData",
        "UnityEngine.RPC",
        "UnityEngine.AnimationInfo",
        "UnityEngine.UI.IMask",
        "UnityEngine.Caching",
        "UnityEngine.Handheld",
        "UnityEngine.MeshRenderer",
        "UnityEngine.UI.DefaultControls",
        "UnityEngine.AnimationClipPair", //Obsolete
        "UnityEngine.CacheIndex", //Obsolete
        "UnityEngine.SerializePrivateVariables", //Obsolete
        "UnityEngine.Networking.NetworkTransport", //Obsolete
        "UnityEngine.Networking.ChannelQOS", //Obsolete
        "UnityEngine.Networking.ConnectionConfig", //Obsolete
        "UnityEngine.Networking.HostTopology", //Obsolete
        "UnityEngine.Networking.GlobalConfig", //Obsolete
        "UnityEngine.Networking.ConnectionSimulatorConfig", //Obsolete
        "UnityEngine.Networking.DownloadHandlerMovieTexture", //Obsolete
        "AssetModificationProcessor", //Obsolete
        "AddressablesPlayerBuildProcessor", //Obsolete
        "UnityEngine.WWW", //Obsolete
        "UnityEngine.EventSystems.TouchInputModule", //Obsolete
        "UnityEngine.MovieTexture", //Obsolete[ERROR]
        "UnityEngine.NetworkPlayer", //Obsolete[ERROR]
        "UnityEngine.NetworkViewID", //Obsolete[ERROR]
        "UnityEngine.NetworkMessageInfo", //Obsolete[ERROR]
        "UnityEngine.UI.BaseVertexEffect", //Obsolete[ERROR]
        "UnityEngine.UI.IVertexModifier", //Obsolete[ERROR]
        //Windows Obsolete[ERROR]
        "UnityEngine.EventProvider",
        "UnityEngine.UI.GraphicRebuildTracker",
        "UnityEngine.GUI.GroupScope",
        "UnityEngine.GUI.ScrollViewScope",
        "UnityEngine.GUI.ClipScope",
        "UnityEngine.GUILayout.HorizontalScope",
        "UnityEngine.GUILayout.VerticalScope",
        "UnityEngine.GUILayout.AreaScope",
        "UnityEngine.GUILayout.ScrollViewScope",
        "UnityEngine.GUIElement",
        "UnityEngine.GUILayer",
        "UnityEngine.GUIText",
        "UnityEngine.GUITexture",
        "UnityEngine.ClusterInput",
        "UnityEngine.ClusterNetwork",
        //System
        "System.Tuple",
        "System.Double",
        "System.Single",
        "System.ArgIterator",
        "System.SpanExtensions",
        "System.TypedReference",
        "System.StringBuilderExt",
        "System.IO.Stream",
        "System.Net.HttpListenerTimeoutManager",
        "System.Net.Sockets.SocketAsyncEventArgs",
        "Unity.Entities.InternalCompilerInterface",
        "UnityEngine.Behaviour",
        "UnityEngine.ArticulationReducedSpace",
        "UnityEngine.ClusterSerialization",
        "Unity.Entities.EntityContainer",
        "Unity.Entities.EntityManager.EntityManagerDebug",
        "UnityEngine.LightingSettings",
        "UnityEngine.Texture",
        "UnityEngine.Texture2D"
    };

    [Filter]
    static bool Filter(System.Reflection.MemberInfo memberInfo)
    {
        // UnityEngine.Debug.Log("name " + memberInfo.DeclaringType.Name + " " + memberInfo.Name);

        if ((memberInfo.DeclaringType.Name == "MonoBehaviour" && memberInfo.Name == "runInEditMode") ||
            (memberInfo.DeclaringType.Name == "MeshRenderer" && memberInfo.Name == "scaleInLightmap") ||
            (memberInfo.DeclaringType.Name == "MeshRenderer" && memberInfo.Name == "stitchLightmapSeams") ||
            (memberInfo.DeclaringType.Name == "MeshRenderer" && memberInfo.Name == "receiveGI"))
        {
            return true;
        }
        return false;
        return memberInfo.DeclaringType.Name == "MonoBehaviour" && memberInfo.Name == "runInEditMode";
    }
}