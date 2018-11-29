using System;
using UnityEngine;

public static class TransformExtension {
    public static void DeleteChildren(this Transform transform) {
        for (var i = 0; i < transform.childCount; i++) {
            var child = transform.GetChild(i);
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }
}
