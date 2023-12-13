using UnityEngine;

#if UNITY_EDITOR
public static class DebugUtil
{
    public static void AssertNullException(bool condition, string name)
    {
        Debug.Assert(condition,$"Null Exception : {name}");
    }

    public static void AssertNotAllocateInInspector(bool condition, string name)
    {
        Debug.Assert(condition,$"Not Allocate In Inspector : {name}");
    }
}
#endif