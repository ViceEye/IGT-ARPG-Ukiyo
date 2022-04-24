using Ukiyo.Framework;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Ukiyo.Editor
{
    [CustomEditor(typeof(MessagesBehavior))]
    public class MessageBehaviorsEd : UnityEditor.Editor
    {
        private ReorderableList
            listOnEnter,
            listOnExit,
            listOnTime;


        bool onEnter, onExit, onTime;

        private MessagesBehavior mMessage;

        private void OnEnable()
        {

            mMessage = (MessagesBehavior) target;

            listOnEnter = new ReorderableList(serializedObject, serializedObject.FindProperty("onEnterMessage"), true,
                true, true, true);
            listOnExit = new ReorderableList(serializedObject, serializedObject.FindProperty("onExitMessage"), true,
                true, true, true);
            listOnTime = new ReorderableList(serializedObject, serializedObject.FindProperty("onTimeMessage"), true,
                true, true, true);

            listOnEnter.drawElementCallback = OnEnterCallback;
            listOnEnter.drawHeaderCallback = OnEnterExitHeaderCallbackDelegate;

            listOnExit.drawElementCallback = OnExitCallback;
            listOnExit.drawHeaderCallback = OnEnterExitHeaderCallbackDelegate;

            listOnTime.drawElementCallback = OnTimeCallback;
            listOnTime.drawHeaderCallback = OnTimeHeaderCallbackDelegate;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(Style(new Color(0, 0.5f, 1f, 0.3f)));
            EditorGUILayout.HelpBox("Script version animation event",
                MessageType.None);
            EditorGUILayout.EndVertical();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginVertical(Style(new Color(0.5f, 0.5f, 0.5f, 0.3f)));
            {

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUI.indentLevel++;
                if (listOnEnter.count > 0) onEnter = true;
                onEnter = EditorGUILayout.Foldout(onEnter, "On Enter (" + listOnEnter.count + ")");
                EditorGUI.indentLevel--;
                if (onEnter)
                {
                    listOnEnter.DoLayoutList();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;
                if (listOnExit.count > 0) onExit = true;
                onExit = EditorGUILayout.Foldout(onExit, "On Exit (" + listOnExit.count + ")");
                EditorGUI.indentLevel--;
                if (onExit)
                {
                    listOnExit.DoLayoutList();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;
                if (listOnTime.count > 0) onTime = true;
                onTime = EditorGUILayout.Foldout(onTime, "On Time (" + listOnTime.count + ")");
                EditorGUI.indentLevel--;
                if (onTime)
                {
                    listOnTime.DoLayoutList();
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }

            serializedObject.ApplyModifiedProperties();
        }


        void OnEnterExitHeaderCallbackDelegate(Rect rect)
        {
            Rect r1 = new Rect(rect.x + 10, rect.y, (rect.width / 3) + 30, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(r1, "Message");

            Rect r3 = new Rect(rect.x + 10 + rect.width / 3 + 5 + 30, rect.y, (rect.width) / 3 - 5 - 15,
                EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(r3, "Type");

            Rect r5 = new Rect(rect.x + 10 + rect.width / 3 * 2 + 5 + 15, rect.y, rect.width / 3 - 5 - 15,
                EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(r5, "Value");
        }

        void OnTimeHeaderCallbackDelegate(Rect rect)
        {
            Rect r1 = new Rect(rect.x + 10, rect.y, rect.width / 4 + 30, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(r1, "Message");

            Rect r3 = new Rect(rect.x + 10 + ((rect.width) / 4) + 5 + 30, rect.y, (rect.width / 4) - 5 - 15,
                EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(r3, "Type");
            Rect r4 = new Rect(rect.x + 10 + ((rect.width) / 4) * 2 + 5 + 20, rect.y, ((rect.width) / 4) - 5,
                EditorGUIUtility.singleLineHeight);

            EditorGUI.LabelField(r4, "Time");
            Rect r5 = new Rect(rect.x + ((rect.width) / 4) * 3 + 5 + 10, rect.y, ((rect.width) / 4) - 5,
                EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(r5, "Value");

        }

        void OnEnterCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = mMessage.onEnterMessage[index];
            rect.y += 2;

            Rect r0 = new Rect(rect.x, rect.y, 15, EditorGUIUtility.singleLineHeight);
            element.Active = EditorGUI.Toggle(r0, element.Active);

            Rect r1 = new Rect(rect.x + 15, rect.y, (rect.width / 3) + 15, EditorGUIUtility.singleLineHeight);
            element.message = EditorGUI.TextField(r1, element.message);


            Rect r3 = new Rect(rect.x + ((rect.width) / 3) + 5 + 30, rect.y, ((rect.width) / 3) - 5 - 15,
                EditorGUIUtility.singleLineHeight);
            element.typeM = (MessagesBehavior.TypeMessage) EditorGUI.EnumPopup(r3, element.typeM);

            Rect r5 = new Rect(rect.x + ((rect.width) / 3) * 2 + 5 + 15, rect.y, ((rect.width) / 3) - 5 - 15,
                EditorGUIUtility.singleLineHeight);
            switch (element.typeM)
            {
                case MessagesBehavior.TypeMessage.Bool:
                    element.boolValue =
                        EditorGUI.ToggleLeft(r5, element.boolValue ? " True" : " False", element.boolValue);
                    break;
                case MessagesBehavior.TypeMessage.Int:
                    element.intValue = EditorGUI.IntField(r5, element.intValue);
                    break;
                case MessagesBehavior.TypeMessage.Float:
                    element.floatValue = EditorGUI.FloatField(r5, element.floatValue);
                    break;
                case MessagesBehavior.TypeMessage.String:
                    element.stringValue = EditorGUI.TextField(r5, element.stringValue);
                    break;
            }
        }

        void OnExitCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = mMessage.onExitMessage[index];
            rect.y += 2;

            Rect r0 = new Rect(rect.x, rect.y, 15, EditorGUIUtility.singleLineHeight);
            element.Active = EditorGUI.Toggle(r0, element.Active);

            Rect r1 = new Rect(rect.x + 15, rect.y, (rect.width / 3) + 15, EditorGUIUtility.singleLineHeight);
            element.message = EditorGUI.TextField(r1, element.message);

            Rect r3 = new Rect(rect.x + rect.width / 3 + 5 + 30, rect.y, rect.width / 3 - 5 - 15,
                EditorGUIUtility.singleLineHeight);
            element.typeM = (MessagesBehavior.TypeMessage) EditorGUI.EnumPopup(r3, element.typeM);

            Rect r5 = new Rect(rect.x + rect.width / 3 * 2 + 5 + 15, rect.y, rect.width / 3 - 5 - 15,
                EditorGUIUtility.singleLineHeight);
            switch (element.typeM)
            {
                case MessagesBehavior.TypeMessage.Bool:
                    element.boolValue =
                        EditorGUI.ToggleLeft(r5, element.boolValue ? " True" : " False", element.boolValue);
                    break;
                case MessagesBehavior.TypeMessage.Int:
                    element.intValue = EditorGUI.IntField(r5, element.intValue);
                    break;
                case MessagesBehavior.TypeMessage.Float:
                    element.floatValue = EditorGUI.FloatField(r5, element.floatValue);
                    break;
                case MessagesBehavior.TypeMessage.String:
                    element.stringValue = EditorGUI.TextField(r5, element.stringValue);
                    break;
            }
        }
        
        void OnTimeCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = mMessage.onTimeMessage[index];
            rect.y += 2;

            Rect r0 = new Rect(rect.x, rect.y, 15, EditorGUIUtility.singleLineHeight);
            element.Active = EditorGUI.Toggle(r0, element.Active);

            Rect r1 = new Rect(rect.x + 15, rect.y, rect.width / 4 + 15, EditorGUIUtility.singleLineHeight);
            element.message = EditorGUI.TextField(r1, element.message);

            Rect r3 = new Rect(rect.x + rect.width / 4 + 5 + 30, rect.y, rect.width / 4 - 5 - 5,
                EditorGUIUtility.singleLineHeight);
            element.typeM = (MessagesBehavior.TypeMessage) EditorGUI.EnumPopup(r3, element.typeM);

            Rect r4 = new Rect(rect.x + rect.width / 4 * 2 + 5 + 25, rect.y, rect.width / 4 - 5 - 15,
                EditorGUIUtility.singleLineHeight);

            element.time = EditorGUI.FloatField(r4, element.time);

            if (element.time > 1) element.time = 1;
            if (element.time < 0) element.time = 0;

            Rect r5 = new Rect(rect.x + rect.width / 4 * 3 + 15, rect.y, rect.width / 4 - 15,
                EditorGUIUtility.singleLineHeight);
            switch (element.typeM)
            {
                case MessagesBehavior.TypeMessage.Bool:
                    element.boolValue =
                        EditorGUI.ToggleLeft(r5, element.boolValue ? " True" : " False", element.boolValue);
                    break;
                case MessagesBehavior.TypeMessage.Int:
                    element.intValue = EditorGUI.IntField(r5, element.intValue);
                    break;
                case MessagesBehavior.TypeMessage.Float:
                    element.floatValue = EditorGUI.FloatField(r5, element.floatValue);
                    break;
                case MessagesBehavior.TypeMessage.String:
                    element.stringValue = EditorGUI.TextField(r5, element.stringValue);
                    break;
            }
        }

        public static GUIStyle Style(Color color)
        {
            GUIStyle currentStyle = new GUIStyle(GUI.skin.box)
            {
                border = new RectOffset(-1, -1, -1, -1)
            };

            Color[] pix = new Color[1];
            pix[0] = color;
            Texture2D bg = new Texture2D(1, 1);
            bg.SetPixels(pix);
            bg.Apply();


            currentStyle.normal.background = bg;
            return currentStyle;
        }
    }
}