using UnityEngine;
using MelonLoader;
using static MelonLoader.MelonLogger;
using Il2CppDecaGames.RotMG.Managers;
using Il2Cpp;
using ModTemplate;
using Il2CppDecaGames.RotMG.Objects.Map.Data;
using static Il2CppDecaGames.RotMG.Objects.Map.Data.ProjectileProperties;
using HarmonyLib;
using UnityEngine.UI;
using Il2CppDecaGames.RotMG.UI.Shop;
using System.Transactions;
using System.Runtime.InteropServices;
using System.Collections;

namespace BasicMelonMod
{
    public class MyMod : MelonMod
    {

        /*
        thank you mimi and DIA4A
        FKALGHJIADI is player
        BHJFNEAHAOE is player speed
        DFALIKKKGLI is dictionary
        PMMFLLAIPGN is enemy
        CLFEOFKBNEJ is x
        PKEECFNFEIO is y
        GGBCADDBAPN is object property
        PLKLDKMMKIP is damageable
        LKHPPBEGNOM.OADOHPKBPJB for max hp
        LKHPPBEGNOM.ABCPKBGJPEP for hp
        (OLD) 1: JENHPHEJIMK, 2: PJDGMIGGJKL is ??????
        FKALGHJIADI.GAFGPNKFMOJ is player speed?
        */
        public static MyMod? instance;
        private bool isMain = false;
        private GameObject? player;

        private GameObject? GameControllerObj;
        private ApplicationManager? ApplicationManagerObj;
        private HJMBOMEHGDJ? sceneInformation;
        private static bool _floatingTextInit = false;
        public static Color32 whit = new Color32(255, 255, 255, 255);
        public static Il2CppSystem.Nullable<UnityEngine.Color32> White = new Il2CppSystem.Nullable<UnityEngine.Color32>(whit);
        private float x, y, mx, my;
        private float playerX, PlayerY;
        private Vector3 playerPosition, enemyPosition, playerPosition2;
        private float angleRadians;
        private HashSet<Vector3> activeCollisions = new HashSet<Vector3>();
        private Vector3? lastLoggedCollisionPosition = null;
        private HashSet<Vector3> collisionPositions = new HashSet<Vector3>();
        private LKHPPBEGNOM? cEnemy;
        private LKHPPBEGNOM? cEnemyM;
        private int lastAbilityTime = 0;
        private float cDist = float.MaxValue;
        private Vector3 cEP = Vector3.zero;
        private float cDistM = float.MaxValue;
        private float cDist2 = float.MaxValue;


        //gui stuff
        private bool showGUI = false;
        private float aimRange = 8f;
        private int moveRange = 10;
        private bool aim = false;
        private bool silent = false;
        private bool ability = false;
        private float minHP = 0;
        private float abilRange = 10;
        private bool cringe = false;
        private bool useMouse = false;
        private bool smooth = false;
        private float smoothness = 1;

        public override void OnEarlyInitializeMelon()
        {
            instance = this;
        }

        public override void OnLateUpdate()
        {
            // This function is called every frame, except after OnUpdate() is called.
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "Main") Setup();
        }

        private void InitializePlayer()
        {
            if (player == null) player = GameObject.Find("Player/Player");
        }

        public override void OnUpdate()
        {
            if (sceneInformation == null) return;
            if (sceneInformation?.DFALIKKKGLI == null) return;
            InitializePlayer();
            ToggleOnKeypad();
            StoreCollision();

            if (!_floatingTextInit) InitializeFloatingText(DGKAANOAENH.LevelUp, new Il2CppSystem.Nullable<Color32>(new Color32(220, 220, 220, 255)));
            ViewHandler component = this.player.GetComponent<ViewHandler>();
            AHOGLGEEHHC destroyEntity = component.destroyEntity;
            FKALGHJIADI? playerData = destroyEntity.Cast<FKALGHJIADI>(); 
            if (playerData == null) return;
            this.playerPosition = new Vector3(playerData.CLFEOFKBNEJ, playerData.PKEECFNFEIO, 0);
            this.playerPosition2 = new Vector3(playerData.CLFEOFKBNEJ, -playerData.PKEECFNFEIO, 0);
            //this.sceneInformation, HMBPMEGLIKJ, and LLLJMPGPFHC
            Cringe();
            lastAbilityTime++;
            if (ApplicationManagerObj == null)
            {

                // This function is called every frame.
                // Useful for when you need to constantly update game information.
            }

        }

        public override void OnGUI()
        {
            if (showGUI)
            {
                float centerX = (Screen.width - 200) / 2f;
                float centerY = (Screen.height - 200) / 2f;

                GUILayout.BeginArea(new Rect(centerX, centerY, 300,300)); 
                GUILayout.BeginVertical("box");

                MyWindow();

                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }


        private void MyWindow()
        {
            GUIStyle checkboxStyle = new GUIStyle(GUI.skin.toggle);
            checkboxStyle.fixedWidth = 30f; 
            checkboxStyle.fixedHeight = 30f;

            // Checkbox 1
            GUILayout.BeginHorizontal();
            GUILayout.Label("Aim Assist:");
            aim = GUILayout.Toggle(aim, "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Smooth Cursor Aim:");
            smooth = GUILayout.Toggle(smooth, "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Smoothness: " + smoothness.ToString("F1"));
            smoothness = GUILayout.HorizontalSlider(smoothness, 0.0f, 10.0f, GUILayout.Width(100f));
            GUILayout.EndHorizontal();

            // Checkbox 2
            GUILayout.BeginHorizontal();
            GUILayout.Label("Silent Aim:");
            silent = GUILayout.Toggle(silent, "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Target Closest To Mouse:");
            useMouse = GUILayout.Toggle(useMouse, "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Aim Range: " + aimRange.ToString("F1"));
            aimRange = GUILayout.HorizontalSlider(aimRange, 0.0f, 10.0f, GUILayout.Width(100f));
            //float.TryParse(GUILayout.TextField(aimRange.ToString()), out aimRange);
            GUILayout.EndHorizontal();

            /*GUILayout.BeginHorizontal();
            GUILayout.Label("AutoAbility:");
            ability = GUILayout.Toggle(ability, "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Ability Min HP: " + minHP);
            float.TryParse(GUILayout.TextField(minHP.ToString()), out minHP);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Ability Range: " + abilRange);
            float.TryParse(GUILayout.TextField(abilRange.ToString()), out abilRange);
            GUILayout.EndHorizontal();*/

            // Checkbox 3
            GUILayout.BeginHorizontal();
            GUILayout.Label("Move To Closest:");
            cringe = GUILayout.Toggle(cringe, "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Movement Min Distance: " + moveRange);
            moveRange = (int)GUILayout.HorizontalSlider(moveRange, 0.0f, 30.0f, GUILayout.Width(100f));
            GUILayout.EndHorizontal();

        }
        public override void OnApplicationQuit()
        {
            // Shut down gracefully or cancel the shutdown here.
        }

        private void ToggleOnKeypad()
        {
            if (Input.GetKeyDown(KeyCode.C)) showGUI = !showGUI;
        }

        private void Cringe()
        {
            //if (sceneInformation?.DFALIKKKGLI == null) ClearData();
            if (sceneInformation?.DFALIKKKGLI == null) return;
            ViewHandler component = this.player.GetComponent<ViewHandler>();
            AHOGLGEEHHC destroyEntity = component.destroyEntity;
            FKALGHJIADI? playerData = destroyEntity.Cast<FKALGHJIADI>();
            if(playerData == null) ClearData(); 
            if(playerData == null) return;

            //ShowFloatingText("" + playerData.GAFGPNKFMOJ(), true);

            UpdateData();
            UpdateData2();

            FindClosestEnemy();
            if (useMouse && silent)
            {
                FindClosestEnemy2();
            }
            if (cDist <= aimRange)
            {
                if (silent && !useMouse)
                {
                    ShootAtPosition();
                }
                if(aim)
                {
                    AimAtEnemy();
                }
            }

            if(cDist2 <= aimRange && silent && useMouse)
            {
                ShootAtPosition();
            }

            if (cDist < float.MaxValue && cringe)
            {
                MoveToEnemy();
            }

            /*if (ability && cDist <= abilRange && lastAbilityTime >= 20 && cEnemy.ABCPKBGJPEP <= minHP)
            {
                UseAbility();
            }*/

        }

        private bool HasEffect(LKHPPBEGNOM enemy, int effect)
        {
            int overflow = 31;
            int index = 0;
            int shift = effect - 1;

            if(effect >= overflow)
            {
                index = 1;
                shift -= overflow;
            }
            return ((enemy.COHCKAPOLCA[index] & (1 << shift)) != 0);
        }
        private void ClearData()
        {
            cDistM = float.MaxValue;
            cDist2 = float.MaxValue;
            cEnemyM = null;
            cDist = float.MaxValue;
            cEP = Vector3.zero;
            cEnemy = null;
            StopMovingMouse();
        }
        private void UpdateData()
        {
            if (cEnemy != null && cEnemy.ABCPKBGJPEP <= 0)
            {
                cDist = float.MaxValue;
                cEP = Vector3.zero;
                cEnemy = null;
                StopMovingMouse();
            }

            if (cEnemy != null)
            { 
                this.x = cEnemy.CLFEOFKBNEJ;
                this.y = cEnemy.PKEECFNFEIO;
                this.enemyPosition = new Vector3(this.x, this.y);
                cDist = Vector3.Distance(this.playerPosition, this.enemyPosition);
                //Msg(cEnemy.GGBCADDBAPN.type + "");
                //ShowFloatingText("" + HasEffect(cEnemy, 24) +  HasEffect(cEnemy, 22) + HasEffect(cEnemy, 25));
                if (HasEffect(cEnemy, 24) || HasEffect(cEnemy, 22) || HasEffect(cEnemy, 25))
                {
                    cDist = float.MaxValue;
                    cEP = Vector3.zero;
                    cEnemy = null;
                }
            }
        }

        private void UpdateData2()
        {
            if(!useMouse && cEnemyM != null)
            {
                cDistM = float.MaxValue;
                cDist2 = float.MaxValue;
                cEnemyM = null;
            }
            if (cEnemyM != null && cEnemyM.ABCPKBGJPEP <= 0)
            {
                cDistM = float.MaxValue;
                cDist2 = float.MaxValue;
                cEnemyM = null;
            }
            if (cEnemyM != null)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cDistM = Vector3.Distance(new Vector3(mx, my, 0), new Vector3(mousePosition.x, -mousePosition.y, 0));
                cDist2 = Vector3.Distance(new Vector3(mx, my, 0), this.playerPosition);
                this.mx = cEnemyM.CLFEOFKBNEJ;
                this.my = cEnemyM.PKEECFNFEIO;
                if (HasEffect(cEnemyM, 24) || HasEffect(cEnemyM, 22) || HasEffect(cEnemyM, 25))
                {
                    cDistM = float.MaxValue;
                    cDist2 = float.MaxValue;
                    cEnemyM = null;
                }
            }
        }
        private void MoveToEnemy()
        {
            var viewHandler = player.GetComponent<Il2CppDecaGames.RotMG.Objects.Map.Data.ViewHandler>();
            var destroyEntity = viewHandler.destroyEntity;
            var playerData = destroyEntity.Cast<FKALGHJIADI>();
            if (playerData == null) return;

            Vector3 currentPosition = new Vector3(playerData.CLFEOFKBNEJ, -playerData.PKEECFNFEIO, 0);
            Vector3 eP = new Vector3(enemyPosition.x, -enemyPosition.y, 0);
            float edist = Vector3.Distance(playerPosition, this.enemyPosition);
            Vector3 np = Vector3.zero;
            float SPD = playerData.BHJFNEAHAOE;
            float speed = (float)(4 + 5.6 * (SPD / 75));
            
            if(edist>= moveRange)
            {
                np = (eP - currentPosition).normalized;
            }
            if (edist <= moveRange)
            {
                np = Vector3.zero;
            }

            //ShowFloatingText(speed + " " + playerData.BHJFNEAHAOE, true);

            Vector3 nextPosition = currentPosition + np * speed * Time.deltaTime;
            if (!IsMoving())
            {
                if (IsNearCollision(nextPosition))
                { 
                    nextPosition = currentPosition + DetermineSidestepDirection(currentPosition, eP) * speed * Time.deltaTime;
                }
                playerData.CLFEOFKBNEJ = nextPosition.x;
                playerData.PKEECFNFEIO = -nextPosition.y;
                playerData.DGNPJNFGFPE = new Vector3(nextPosition.x, nextPosition.y, 0);
            }
        }

        private bool IsMoving()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                return true;
            }
            return false;
        }
        private void StoreCollision()
        {
            if (sceneInformation?.DFALIKKKGLI == null) return;
            activeCollisions.Clear();

            foreach (var entry in sceneInformation.DFALIKKKGLI)
            {
                LKHPPBEGNOM? enemy = entry.Value;
                if (enemy?.MEFAKOMHNJH?.occupySquare == true)
                {
                    Vector3 collisionPosition = new Vector3(enemy.CLFEOFKBNEJ, -enemy.PKEECFNFEIO, 0f);
                    AddCollisionPosition(entry.Key.ToString(), collisionPosition);
                    activeCollisions.Add(collisionPosition);
                }
            }
            CleanupCollisionPositions();
        }


        private void CleanupCollisionPositions()
        {
            collisionPositions.RemoveWhere(collisionPosition => !activeCollisions.Contains(collisionPosition));
        }

        private void AddCollisionPosition(string enemyName, Vector3 enemyPosition)
        {
            if (!collisionPositions.Contains(enemyPosition))
            {

                collisionPositions.Add(enemyPosition);
            }
        }

        private bool IsNearCollision(Vector3 checkPosition)
        {
            foreach (var collisionPosition in collisionPositions)
            {
                float distance = Vector3.Distance(checkPosition, collisionPosition);
                if (distance < 1f)
                {
                    if (!lastLoggedCollisionPosition.HasValue || lastLoggedCollisionPosition.Value != collisionPosition)
                    {

                        lastLoggedCollisionPosition = collisionPosition;
                    }
                    return true;
                }
            }

            lastLoggedCollisionPosition = null;
            return false;
        }

        private void FindClosestEnemy()
        {
            if (sceneInformation == null) return;
            if (sceneInformation?.DFALIKKKGLI == null) return;

            foreach (var entry in sceneInformation?.DFALIKKKGLI)
            {
                LKHPPBEGNOM? enemy = entry.Value;

                if (enemy == null) continue;

                if (enemy is PMMFLLAIPGN && enemy.PLKLDKMMKIP && !enemy.GGBCADDBAPN.isEnemy) continue;

                if (HasEffect(enemy, 24) || HasEffect(enemy, 22) || HasEffect(enemy, 25)) continue;
                Vector3 eP = new Vector3(enemy.CLFEOFKBNEJ, enemy.PKEECFNFEIO);
                float dist = Vector3.Distance(this.playerPosition, eP);
                if (dist < cDist && enemy.GGBCADDBAPN.isEnemy)
                {
                    cEnemy = enemy;
                    cEP = eP;
                    cDist = Vector3.Distance(this.playerPosition, new Vector3(cEnemy.CLFEOFKBNEJ, cEnemy.PKEECFNFEIO));
                }
            }
        }

        private void FindClosestEnemy2()
        {
            if (sceneInformation == null) return;
            if (sceneInformation?.DFALIKKKGLI == null) return;

            //ShowFloatingText2("Player Found At X: " + playerX + " Y: " + PlayerY, true);
            foreach (var entry in sceneInformation?.DFALIKKKGLI)
            {
                LKHPPBEGNOM? enemy = entry.Value;

                if (enemy == null) continue;

                if (enemy is PMMFLLAIPGN && enemy.PLKLDKMMKIP && !enemy.GGBCADDBAPN.isEnemy)
                    continue;

                if (HasEffect(enemy, 24) || HasEffect(enemy, 22) || HasEffect(enemy, 25)) continue;
                //if (enemy.GGBCADDBAPN.type == 20847) continue;

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 eP = new Vector3(enemy.CLFEOFKBNEJ, enemy.PKEECFNFEIO);
                float distance = Vector3.Distance(eP, new Vector3(mousePosition.x, -mousePosition.y, 0));
                float dist = Vector3.Distance(this.playerPosition, eP);
                if (distance < cDistM && dist <= aimRange && enemy.GGBCADDBAPN.isEnemy)
                {
                    cDistM = distance;
                    cEnemyM = enemy;
                }
            }
        }

        /*private bool HasEffect(LKHPPBEGNOM entity) 
        {
            var conditionArray = entity.COHCKAPOLCA;
            int condition1 = conditionArray[0];
        }*/
        private Vector3 DetermineSidestepDirection(Vector3 currentPosition, Vector3 targetPosition)
        {
            Vector3 desiredDirection = (targetPosition - currentPosition).normalized;
            Vector3 sidestepLeft = new Vector3(-desiredDirection.y, desiredDirection.x, 0).normalized;
            Vector3 sidestepRight = new Vector3(desiredDirection.y, -desiredDirection.x, 0).normalized;
            Vector3 sidestepDown = new Vector3(desiredDirection.x, desiredDirection.y + 1, 0).normalized;
            Vector3 sidestepUp = new Vector3(desiredDirection.x, desiredDirection.y - 1, 0).normalized;

            if (!IsNearCollision(currentPosition + sidestepLeft))
                return sidestepLeft;
            else if (!IsNearCollision(currentPosition + sidestepRight))
                return sidestepRight;
            else if (!IsNearCollision(currentPosition + sidestepDown))
                return sidestepDown;
            else if (!IsNearCollision(currentPosition + sidestepUp))
                return sidestepUp;
            else
                return Vector3.zero;
        }

        private void AimAtEnemy()
        {
            Vector3 y = Camera.main.transform.eulerAngles;
            this.angleRadians = y.z * Mathf.Deg2Rad;
            Vector3 screenPosition = CalculateScreenPosition(this.angleRadians);
            if (this.x > 0.0f && this.y > 0.0f && !showGUI)
            {
                if (!smooth) MouseMover.MoveMouse((int)screenPosition.x, (int)screenPosition.y);
                if (smooth)  MoveMouseSmoothly(screenPosition.x, screenPosition.y, smoothness);
            }
        }
        private Vector3 CalculateScreenPosition(float angleRadians)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(new Vector3(enemyPosition.x, -enemyPosition.y, 0));
            screenPosition.y = Screen.height - screenPosition.y;
            float screenRotation = angleRadians;
            float radians = screenRotation * Mathf.Deg2Rad;
            float cosTheta = Mathf.Cos(radians);
            float sinTheta = Mathf.Sin(radians);
            Vector3 center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            Vector3 offset = screenPosition - center;
            float rotatedX = offset.x * cosTheta - offset.y * sinTheta;
            float rotatedY = offset.x * sinTheta + offset.y * cosTheta;
            screenPosition.x = rotatedX + center.x;
            screenPosition.y = rotatedY + center.y;
            return screenPosition;
        }

        private float CalculateAngle(Vector2 position)
        {
            return Mathf.Atan2(position.y - playerPosition.y, position.x - playerPosition.x);
        }
         
        private void UseAbility()
        {
            ViewHandler component = this.player.GetComponent<ViewHandler>();
            AHOGLGEEHHC destroyEntity = component.destroyEntity;
            var playerData = destroyEntity.Cast<FKALGHJIADI>();
            playerData.PBABCOMDDPO(this.x, this.y, ELAINNINAMO.Default);
            lastAbilityTime = 0;
        }
        public void ShootAtPosition()
        {
            ViewHandler component = this.player.GetComponent<ViewHandler>();
            AHOGLGEEHHC destroyEntity = component.destroyEntity;
            var playerData = destroyEntity.Cast<FKALGHJIADI>();
            if (!useMouse) playerData.CDJLLHJOCNM(CalculateAngle(new Vector2(this.x, this.y)));
            if (useMouse) playerData.CDJLLHJOCNM(CalculateAngle(new Vector2(this.mx, this.my)));
        }
        public void ShowFloatingText(string text, Color32 color)
        {
            Il2CppSystem.Nullable<Color32> newColor = new Il2CppSystem.Nullable<Color32>(color);
            var effectType = DGKAANOAENH.Xp;
            if (!_floatingTextInit) InitializeFloatingText(effectType, newColor);
            sceneInformation?.OCLNLBHDEFK.MPGOFIHIDML.iGUIManager.ShowFloatingText(effectType, text, newColor, 0.0f, 0.0f, 0.0f);
        }
        public void ShowFloatingText2(string text, Color32 color)
        {
            Il2CppSystem.Nullable<Color32> newColor = new Il2CppSystem.Nullable<Color32>(color);
            var effectType = DGKAANOAENH.LevelUp;
            if (!_floatingTextInit) InitializeFloatingText(DGKAANOAENH.LevelUp, new Il2CppSystem.Nullable<Color32>(color));
            sceneInformation?.OCLNLBHDEFK.MPGOFIHIDML.iGUIManager.ShowFloatingText(effectType, text, newColor, 0.0f, 0.0f, 0.0f);
        }

        private void InitializeFloatingText(DGKAANOAENH effectType, Il2CppSystem.Nullable<Color32> newColor)
        {
            for (int i = 0; i < 12; i++)
                sceneInformation?.OCLNLBHDEFK.MPGOFIHIDML.iGUIManager.ShowFloatingText(effectType, "", newColor, 0.0f, 0.0f,0.0f);
            _floatingTextInit = true;
        }

        /// <summary>
        /// Shows floating text of green, red or whit color above the player in-game.
        /// </summary>
        /// <param name="text">The text to show.</param>
        /// <param name="toggle">(optional) True for green text, False for red text. Default is white.</param>
        public void ShowFloatingText(String text, bool? toggle = null)
        {
            var color = toggle switch
            {
                null => new Color32(220, 220, 220, 255), // white
                true => new Color32(32, 220, 0, 255), // green
                false => new Color32(255, 0, 25, 255) // red
            };

            ShowFloatingText(text, color);
        }

        public void ShowFloatingText2(String text, bool? toggle = null)
        {
            var color = toggle switch
            {
                null => new Color32(220, 220, 220, 255), // white
                true => new Color32(32, 220, 0, 255), // green
                _ => new Color32(255, 0, 25, 255) // red
            };

            ShowFloatingText2(text, color);
        }
        /// <summary>
        /// Initialize variables required for the mod such as finding the GameController and sceneInformation.
        /// sceneInformation is important as it contains most objects loaded in the current scene.
        /// </summary>
        private void Setup()
        {
            GameControllerObj = GameObject.Find("GameController");
            if (GameControllerObj == null)
            {
                Msg("ERROR: Failed to find GameController object.");
                return;
            }

            ApplicationManagerObj = GameControllerObj.GetComponent<ApplicationManager>();
            if (ApplicationManagerObj == null)
            {
                Msg("ERROR: Failed to find ApplicationManager object.");
                return;
            }

            sceneInformation = ApplicationManagerObj?._CHDFAEBMILI_k__BackingField;
            if (sceneInformation == null)
            {
                Msg("ERROR: Failed to find sceneInformation.");
                return;

            }
        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        private static Coroutine? moveCoroutine;

        public static void StopMovingMouse()
        {
            if (moveCoroutine != null)
            {
                MelonCoroutines.Stop(moveCoroutine);
                moveCoroutine = null;
            }
        }
        public static void MoveMouseSmoothly(float targetX, float targetY, float durationInSeconds)
        {
            if (moveCoroutine != null)
            {
                MelonCoroutines.Stop(moveCoroutine);
            }
            moveCoroutine = (Coroutine?)MelonCoroutines.Start(MoveMouseCoroutine(targetX, targetY, durationInSeconds));
        }
        private static IEnumerator MoveMouseCoroutine(float targetX, float targetY, float durationInSeconds)
        {
            POINT cursorPosition;
            if (!GetCursorPos(out cursorPosition))
            {
                yield break; // Exit the coroutine if getting cursor position fails
            }

            int steps = (int)(durationInSeconds * 100); // Adjust the number of steps as needed
            float stepX = (targetX - cursorPosition.X) / steps;
            float stepY = (targetY - cursorPosition.Y) / steps;

            for (int i = 0; i < steps; i++)
            {

                float currentX = cursorPosition.X + stepX * i;
                float currentY = cursorPosition.Y + stepY * i;
                SetCursorPos((int)currentX, (int)currentY);
                yield return new WaitForSeconds(0.01f); // Adjust the wait time for desired speed
            }

            SetCursorPos((int)targetX, (int)targetY);
            moveCoroutine = null; // Reset the coroutine reference when done
        }
    }
}
