using BepInEx;
using System.Collections.Generic;
using System.Linq;
using Studio;
using UnityEngine;
using KKAPI.Studio;
using KKAPI.Utilities;
using System;

namespace AlignedPlacement
{
    [BepInPlugin(Constants.PluginGUID, Constants.PluginName, Constants.PluginVersion)]
    [BepInProcess(Constants.StudioProcessName)]
    public class AlignedPlacement : BaseUnityPlugin
    {
        // GUI parts
        private string m_linearPosXStr = "0.000";
        private float m_linearPosX = 0.0f;
        private string m_linearPosYStr = "0.000";
        private float m_linearPosY = 0.0f;
        private string m_linearPosZStr = "0.000";
        private float m_linearPosZ = 0.0f;
        private string m_linearAngleXStr = "0.000";
        private float m_linearAngleX = 0.0f;
        private string m_linearAngleYStr = "0.000";
        private float m_linearAngleY = 0.0f;
        private string m_linearAngleZStr = "0.000";
        private float m_linearAngleZ = 0.0f;

        private bool m_useFirstObjectAsCenterPoint = false;
        private bool m_isEndAngleInclusive = true;
        private int m_circleAxes = 2;
        private int m_circleRotationAxis = 1;
        private string m_circleRadiusStr = "1.000";
        private float m_circleRadius = 1.0f;
        private string m_circleAngleStartStr = "0.000";
        private float m_circleAngleStart = 180.0f;
        private string m_circleAngleEndStr = "180.000";
        private float m_circleAngleEnd = 0.0f;
        private bool m_circleEnableRotation = false;
        private string m_circleRotationOffsetStr = "90.000";
        private float m_circleRotationOffset = 90.0f;

        private string m_speedMinStr = "0.8";
        private float m_speedMin = 0.8f;
        private string m_speedMaxStr = "1.2";
        private float m_speedMax = 1.2f;

        // Informations of target objects
        private Dictionary<int, Vector3> m_oldPosDic;
        private Dictionary<int, Vector3> m_oldRotDic;

        // GUI status and parameters
        private string m_prevFocused = "";
        private string m_nowFocused = "";
        public bool IsDialogVisible;

        private Rect m_guiRect = new Rect(110, 510, 450, 360);
        private static readonly GUIStyleState HeadingColorState = new GUIStyleState()
        {
            textColor = Color.white
        };
        private static GUIStyle HeadingStyle = new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            normal = HeadingColorState
        };
        private static readonly Rect CircleAxesRect = new Rect(123, 120, 316, 40);
        private static readonly string[] CircleAxesChoices = new string[] { "XY\n(Vertical)", "ZY\n(Vertical)", "ZX\n(Horizontal)" };
        private static readonly Rect RotationAxisRect = new Rect(189, 284, 250, 20);
        private static readonly string[] RotationAxisChoices = new string[] { "X", "Y", "Z" };

        public void Start()
        {
            // Memo: Doing this initialization on Awake() may fail due to loading order
            Texture2D buttonTex = ResourceUtils.GetEmbeddedResource("studio_icon.png", typeof(AlignedPlacement).Assembly).LoadTexture();
            KKAPI.Studio.UI.CustomToolbarButtons.AddLeftToolbarToggle(buttonTex, false, b => IsDialogVisible = b);
        }

        public void OnDestroy()
        {
            GameObject obj = GameObject.Find(Constants.PluginName);
            Destroy(obj);
        }

        public void OnGUI()
        {
            GUI.skin = IMGUIUtils.SolidBackgroundGuiSkin;
            if (!IsDialogVisible)
            {
                return;
            }

            m_guiRect = GUILayout.Window(0, m_guiRect, GuiContents, "Aligned Placement Helper");
            IMGUIUtils.EatInputInRect(m_guiRect);
        }

        private void GuiContents(int windowId)
        {
            m_nowFocused = GUI.GetNameOfFocusedControl();

            GuiLinearIncrementContents();
            GuiCircularPlacmentContents();
            GuiSpeedRandomizerContents();

            m_prevFocused = GUI.GetNameOfFocusedControl();
            GUI.DragWindow();
        }

        private void GuiLinearIncrementContents()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Linear increment", HeadingStyle);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Position");
            DefineFloatField("pos_x", ref m_linearPosXStr, ref m_linearPosX);
            DefineFloatField("pos_y", ref m_linearPosYStr, ref m_linearPosY);
            DefineFloatField("pos_z", ref m_linearPosZStr, ref m_linearPosZ);
            if (GUILayout.Button("Go", GUILayout.Width(100)))
            {
                UpdateFloatValue(ref m_linearPosXStr, ref m_linearPosX);
                UpdateFloatValue(ref m_linearPosYStr, ref m_linearPosY);
                UpdateFloatValue(ref m_linearPosZStr, ref m_linearPosZ);
                ApplyLinearMove();
            };
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation");
            DefineFloatField("angle_x", ref m_linearAngleXStr, ref m_linearAngleX);
            DefineFloatField("angle_y", ref m_linearAngleYStr, ref m_linearAngleY);
            DefineFloatField("angle_z", ref m_linearAngleZStr, ref m_linearAngleZ);
            if (GUILayout.Button("Go", GUILayout.Width(100)))
            {
                UpdateFloatValue(ref m_linearAngleXStr, ref m_linearAngleX);
                UpdateFloatValue(ref m_linearAngleYStr, ref m_linearAngleY);
                UpdateFloatValue(ref m_linearAngleZStr, ref m_linearAngleZ);
                ApplyLinearRotation();
            };
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void GuiCircularPlacmentContents()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Circular placement", HeadingStyle);
            GUILayout.Label("Axes");
            m_circleAxes = GUI.Toolbar(CircleAxesRect, m_circleAxes, CircleAxesChoices);
            GUILayout.Space(20);

            m_useFirstObjectAsCenterPoint = GUILayout.Toggle(m_useFirstObjectAsCenterPoint, "Use the first object as a center point");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius");
            DefineFloatField("circle_radius", ref m_circleRadiusStr, ref m_circleRadius);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Placement start angle");
            DefineFloatField("circle_angle_start", ref m_circleAngleStartStr, ref m_circleAngleStart);
            GUILayout.Label("End angle");
            DefineFloatField("circle_angle_end", ref m_circleAngleEndStr, ref m_circleAngleEnd);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            m_isEndAngleInclusive = GUILayout.Toggle(m_isEndAngleInclusive, "End angle is inclusive");
            GUILayout.EndHorizontal();

            m_circleEnableRotation = GUILayout.Toggle(m_circleEnableRotation, "Rotate each objects");
            GUILayout.Label("Rotation axis");
            m_circleRotationAxis = GUI.Toolbar(RotationAxisRect, m_circleRotationAxis, RotationAxisChoices);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation offset angle");
            DefineFloatField("circle_rotation_offset", ref m_circleRotationOffsetStr, ref m_circleRotationOffset);
            if (GUILayout.Button("Go", GUILayout.Width(100)))
            {
                UpdateFloatValue(ref m_circleRadiusStr, ref m_circleRadius);
                UpdateFloatValue(ref m_circleAngleStartStr, ref m_circleAngleStart);
                UpdateFloatValue(ref m_circleAngleEndStr, ref m_circleAngleEnd);
                UpdateFloatValue(ref m_circleRotationOffsetStr, ref m_circleRotationOffset);
                ApplyCircularPlacement();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void GuiSpeedRandomizerContents()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Randomize animation speed", HeadingStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label("min.");
            DefineFloatField("speed_min", ref m_speedMinStr, ref m_speedMin);
            GUILayout.Label("Max.");
            DefineFloatField("speed_max", ref m_speedMaxStr, ref m_speedMax);
            if (GUILayout.Button("Go", GUILayout.Width(100)))
            {
                UpdateFloatValue(ref m_speedMinStr, ref m_speedMin);
                UpdateFloatValue(ref m_speedMaxStr, ref m_speedMax);
                ApplySpeedRandomizer();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DefineFloatField(string name, ref string strMember, ref float floatMember, int maxLength = 7, int widthPx = 80)
        {
            GUILayoutOption[] options = { GUILayout.Width(widthPx), GUILayout.ExpandWidth(false) };

            GUI.SetNextControlName(name);
            strMember = GUILayout.TextField(strMember, maxLength, options);
            if ((m_prevFocused == name) && (m_nowFocused != name))
            {
                UpdateFloatValue(ref strMember, ref floatMember);
            }
        }

        private void UpdateFloatValue(ref string strMember, ref float floatMember)
        {
            if (float.TryParse(strMember, out float tmp))
            {
                floatMember = tmp;
            }
            else
            {
                floatMember = 0;
            }
            strMember = floatMember.ToString("F3");
        }

        private void ApplyLinearMove()
        {
            GuideObject[] selectedObjects = Singleton<GuideObjectManager>.Instance.selectObjects;
            if ((selectedObjects == null) || (selectedObjects.Count() <= 1))
            {
                return;
            }
            RememberCurrentPosAndRot(selectedObjects);

            GuideObject firstTarget = GetTargetObject();
            int count = 1;
            foreach (GuideObject obj in selectedObjects)
            {
                if (firstTarget != obj)
                {
                    if (obj.enablePos)
                    {
                        Vector3 newPos = new Vector3(
                            firstTarget.transformTarget.position.x + (m_linearPosX * count),
                            firstTarget.transformTarget.position.y + (m_linearPosY * count),
                            firstTarget.transformTarget.position.z + (m_linearPosZ * count)
                        );
                        count++;
                        obj.transformTarget.position = newPos;
                        obj.changeAmount.pos = obj.transformTarget.localPosition;
                    }
                }
            }
            PushMovementToUndoList(selectedObjects);
        }

        private void ApplyLinearRotation()
        {
            GuideObject[] selectedObjects = Singleton<GuideObjectManager>.Instance.selectObjects;
            if ((selectedObjects == null) || (selectedObjects.Count() <= 1))
            {
                return;
            }
            RememberCurrentPosAndRot(selectedObjects);

            GuideObject firstTarget = GetTargetObject();
            int count = 1;
            foreach (GuideObject obj in selectedObjects)
            {
                if (firstTarget != obj)
                {
                    if (obj.enablePos)
                    {
                        Vector3 newAngle = new Vector3(
                                firstTarget.transformTarget.localEulerAngles.x + (m_linearAngleX * count),
                                firstTarget.transformTarget.localEulerAngles.y + (m_linearAngleY * count),
                                firstTarget.transformTarget.localEulerAngles.z + (m_linearAngleZ * count)
                            );
                        count++;
                        obj.transformTarget.localEulerAngles = newAngle;
                        obj.changeAmount.rot = obj.transformTarget.localEulerAngles;
                    }
                }
            }
            PushRotationToUndoList(selectedObjects);
        }

        private void ApplyCircularPlacement()
        {
            GuideObject[] selectedObjects = Singleton<GuideObjectManager>.Instance.selectObjects;
            if ((selectedObjects == null) || (selectedObjects.Count() <= 1))
            {
                return;
            }

            RememberCurrentPosAndRot(selectedObjects);

            // Determine target axes
            float posx = 0, posy = 0, posz = 0, rotx = 0, roty = 0, rotz = 0;
            ref float posAxis0Ref = ref posz;
            ref float posAxis1Ref = ref posx;
            ref float rotAxisRef = ref rotz;
            switch (m_circleAxes)
            {
                case 0:
                    posAxis0Ref = ref posx;
                    posAxis1Ref = ref posy;
                    break;
                case 1:
                    posAxis0Ref = ref posz;
                    posAxis1Ref = ref posy;
                    break;
                case 2:
                    posAxis0Ref = ref posz;
                    posAxis1Ref = ref posx;
                    break;
                default:
                    return;
            }
            switch (m_circleRotationAxis)
            {
                case 0:
                    rotAxisRef = ref rotx;
                    break;
                case 1:
                    rotAxisRef = ref roty;
                    break;
                case 2:
                    rotAxisRef = ref rotz;
                    break;
                default:
                    return;
            }

            // Calcurate the degree between two objects
            double degreeStep;
            int nObjectsToBeAligned = selectedObjects.Count();
            if (m_useFirstObjectAsCenterPoint) nObjectsToBeAligned--;
            if (!m_isEndAngleInclusive) nObjectsToBeAligned++;
            if (m_circleAngleEnd > m_circleAngleStart)
            {
                degreeStep = (m_circleAngleEnd - m_circleAngleStart) / (nObjectsToBeAligned - 1);
            }
            else
            {
                degreeStep = -(m_circleAngleStart - m_circleAngleEnd) / (nObjectsToBeAligned - 1);
            }

            // Determine the center point
            GuideObject firstTarget = GetTargetObject();
            Vector3 centerPoint;
            if (m_useFirstObjectAsCenterPoint)
            {
                centerPoint = firstTarget.transformTarget.position;
            }
            else
            {
                double radian = Math.PI * m_circleAngleStart / 180.0;
                posAxis0Ref = (float)(Math.Cos(radian) * m_circleRadius);
                posAxis1Ref = (float)(Math.Sin(radian) * m_circleRadius);
                centerPoint = new Vector3(
                    firstTarget.transformTarget.position.x - posx,
                    firstTarget.transformTarget.position.y - posy,
                    firstTarget.transformTarget.position.z - posz
                );
            }

            // Place all the selected objects
            double curCircleDegree = m_circleAngleStart;
            double curRotation = m_circleRotationOffset;
            int count = 0;
            foreach (GuideObject obj in selectedObjects)
            {
                if (m_useFirstObjectAsCenterPoint && (firstTarget == obj))
                {
                    continue;
                }
                if (obj.enablePos)
                {
                    count++;
                    double radian = Math.PI * curCircleDegree / 180.0;
                    posAxis0Ref = (float)(Math.Cos(radian) * m_circleRadius);
                    posAxis1Ref = (float)(Math.Sin(radian) * m_circleRadius);
                    Vector3 newPos = new Vector3(
                        centerPoint.x + posx,
                        centerPoint.y + posy,
                        centerPoint.z + posz
                    );
                    obj.transformTarget.position = newPos;
                    obj.changeAmount.pos = obj.transformTarget.localPosition;
                    curCircleDegree += degreeStep;
                    if (m_circleEnableRotation)
                    {
                        rotx = firstTarget.transformTarget.localEulerAngles.x;
                        roty = firstTarget.transformTarget.localEulerAngles.y;
                        rotz = firstTarget.transformTarget.localEulerAngles.z;
                        rotAxisRef = (float)curRotation;
                        Vector3 newAngle = new Vector3(rotx, roty, rotz);

                        obj.transformTarget.localEulerAngles = newAngle;
                        obj.changeAmount.rot = obj.transformTarget.localEulerAngles;
                    }
                    curRotation += degreeStep;
                }
            }

            // Push these actions to the undo list
            PushMovementToUndoList(selectedObjects);
            PushRotationToUndoList(selectedObjects);
        }

        private void ApplySpeedRandomizer()
        {
            IEnumerable<ObjectCtrlInfo> objCtrls = StudioAPI.GetSelectedObjects();
            foreach (ObjectCtrlInfo objCtrl in objCtrls)
            {
                if (m_speedMin < 0) m_speedMin = 0;
                if (m_speedMax < 0) m_speedMax = 0;
                if (m_speedMin > m_speedMax)
                {
                    float tmp = m_speedMin;
                    m_speedMin = m_speedMax;
                    m_speedMax = tmp;
                }

                double newResult = UnityEngine.Random.Range(m_speedMin, m_speedMax);
                objCtrl.animeSpeed = (float)newResult;
            }
        }

        private GuideObject GetTargetObject() => Singleton<GuideObjectManager>.Instance.operationTarget == null ? Singleton<GuideObjectManager>.Instance.selectObject : Singleton<GuideObjectManager>.Instance.operationTarget;

        private void RememberCurrentPosAndRot(GuideObject[] selectedObjects)
        {
            m_oldPosDic = new Dictionary<int, Vector3>();
            m_oldRotDic = new Dictionary<int, Vector3>();
            foreach (GuideObject obj in selectedObjects)
            {
                if (obj.enablePos)
                {
                    m_oldPosDic.Add(obj.dicKey, obj.changeAmount.pos);
                    m_oldRotDic.Add(obj.dicKey, obj.changeAmount.rot);
                }
            }
        }

        private void PushMovementToUndoList(GuideObject[] selectedObjects)
        {
            GuideCommand.EqualsInfo[] posChangeInfo = (from v in selectedObjects
                                                       select new GuideCommand.EqualsInfo
                                                       {
                                                           dicKey = v.dicKey,
                                                           oldValue = m_oldPosDic[v.dicKey],
                                                           newValue = v.changeAmount.pos
                                                       }).ToArray();
            Singleton<UndoRedoManager>.Instance.Push(new GuideCommand.MoveEqualsCommand(posChangeInfo.ToArray()));
        }

        private void PushRotationToUndoList(GuideObject[] selectedObjects)
        {
            GuideCommand.EqualsInfo[] rotChangeInfo = (from v in selectedObjects
                                                       select new GuideCommand.EqualsInfo
                                                       {
                                                           dicKey = v.dicKey,
                                                           oldValue = m_oldRotDic[v.dicKey],
                                                           newValue = v.changeAmount.rot
                                                       }).ToArray();
            Singleton<UndoRedoManager>.Instance.Push(new GuideCommand.RotationEqualsCommand(rotChangeInfo.ToArray()));
        }
    }
}
