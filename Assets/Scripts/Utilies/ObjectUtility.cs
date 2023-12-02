#nullable enable

public static class ObjectUtility
{
    public static bool IsNullOrDestroyed<T>(T? obj)
    {
        if (obj == null)
        {
            return true;
        }

        return obj is UnityEngine.Object unityObj ? unityObj == null : obj == null;
    }
}