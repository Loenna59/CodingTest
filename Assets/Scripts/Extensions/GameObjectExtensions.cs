#nullable enable

using UnityEngine;

public static class GameObjectExtensions
{
    public static void SafeDestroy(this GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        
        obj.LeanCancel();

#if UNITY_EDITOR
        if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(obj))
        {
            Debug.LogError("Implementation");
        }
        else if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(obj))
        {
            GameObject? prefabInstance = UnityEditor.PrefabUtility.GetPrefabInstanceHandle(obj) as GameObject;
            UnityEngine.Object.DestroyImmediate(prefabInstance);
            UnityEngine.Object.DestroyImmediate(obj);
        }
        else
#endif
        {
            UnityEngine.Object.Destroy(obj);
        }
    }
}