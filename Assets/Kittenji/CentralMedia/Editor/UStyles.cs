using UnityEditor;
using UnityEngine;

namespace Kittenji
{
    public static class UStyles
    {
        static GUIStyle m_MiniLabelRight;
        public static GUIStyle MiniLabelRight => m_MiniLabelRight ?? (m_MiniLabelRight = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleRight });

        static GUIStyle m_MiniLabelCenter;
        public static GUIStyle MiniLabelCenter => m_MiniLabelCenter ?? (m_MiniLabelCenter = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleCenter });

        static GUIStyle m_FoldoutBold;
        public static GUIStyle FoldoutBold => m_FoldoutBold ?? (m_FoldoutBold = new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });

        static GUIStyle m_ToolbarButtonLeft;
        public static GUIStyle ToolbarButtonLeft => m_ToolbarButtonLeft ?? (m_ToolbarButtonLeft = new GUIStyle(EditorStyles.toolbarButton) { alignment = TextAnchor.MiddleLeft, richText = true });

        static GUIStyle m_WrappedTextArea;
        public static GUIStyle WrappedTextArea => m_WrappedTextArea ?? (m_WrappedTextArea = new GUIStyle(EditorStyles.textArea) { wordWrap = true });
    }
}