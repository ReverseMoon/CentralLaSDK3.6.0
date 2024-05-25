using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Kittenji.Utils
{
    public static class ObjectUtils
    {
        [MenuItem("GameObject/Object Utils/Create Empty At Center", false, -1)]
        public static void CreateEmptyAtCenter(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("GameObject");
            GameObject parent = menuCommand.context as GameObject;
            if (parent != null)
            {
                GameObjectUtility.SetParentAndAlign(go, parent);
                Renderer renderer = parent.GetComponent<Renderer>();
                if (renderer)
                {
                    Vector3 center = renderer.bounds.center;
                    go.transform.position = center;
                    go.transform.localScale = renderer.bounds.extents;
                }
            }
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}
