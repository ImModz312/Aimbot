using UnityEngine;
using MelonLoader;
using static MelonLoader.MelonLogger;
using Il2CppDecaGames.RotMG.Managers;
using Il2Cpp;
using ModTemplate;

namespace BasicMelonMod
{
    public class MyMod : MelonMod
    {
        public static MyMod? instance;
        private bool isMain = false;
        private GameObject player;

        private int counter = 0;
        private GameObject? GameControllerObj;
        private ApplicationManager? ApplicationManagerObj;
        private NGNAIHHOBOC? sceneInformation;
        private GameObject? cameraa;
        private static bool _floatingTextInit = false;
        private bool firstrun = true;
        public static Color32 whit = new Color32(255, 255, 255, 255);
        public static Il2CppSystem.Nullable<UnityEngine.Color32> White = new Il2CppSystem.Nullable<UnityEngine.Color32>(whit);
        private float x, y, sx, sy;
        private float px, py;
        private Vector3 pp, ep;
        private float centerX = 777;
        private float centerY = 527;
        private float newX, newY, angleRadians, angleDegrees;
        private bool cringe = false;



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
            if (sceneInformation?.BMGIOONAILC == null) return;
            if (sceneInformation?.LLLJMPGPFHC == null) return;
            InitializePlayer();
            ToggleOnKeypad();
            //this.sceneInformation, BMGIOONAILC, and LLLJMPGPFHC
            if (cringe)
            {
                Cringe();
            }
            if (!cringe)
            {
                //MouseMover.ReleaseKey(Keys.L);
            }
            if (ApplicationManagerObj == null)
            {

                // This function is called every frame.
                // Useful for when you need to constantly update game information.
            }
        }

        public override void OnGUI()
        {
            // This is called on every IMGUI event. Only use this for IMGUI interaction as it can run multiple times per frame.
        }

        public override void OnApplicationQuit()
        {
            // Shut down gracefully or cancel the shutdown here.
        }

        /// <summary>
        /// Shows floating text of any color above the player in-game.
        /// </summary>
        /// <param name="text">The text to show.</param>
        /// <param name="color">Color32 object for text color.</param>
        /// 
        private void ToggleOnKeypad()
        {

            if (Input.GetKeyDown(KeyCode.C)) TCringe();
        }

        private void TCringe()
        {
            cringe = !cringe;
            ShowFloatingText(cringe ? "Aimbot On" : "Aimbot Off", cringe);
        }


        private void Cringe()
        {
            // BMGIOONAILC is localplayer
            if (this.sceneInformation?.LLLJMPGPFHC == null) return;
            if (this.sceneInformation?.BMGIOONAILC is not JENHPHEJIMK playerInfo) return;
            px = (float)sceneInformation?.BMGIOONAILC.OIKPMFBGJPD;
            py = (float)sceneInformation?.BMGIOONAILC.LEKACFCHONN;
            pp = new Vector3(px, py);
            float cDist = float.MaxValue;
            Vector3 cEP = Vector3.zero;
            //777, 527
            //ShowFloatingText2("Player Found At X: " + px + " Y: " + py, true);
            foreach (var entry in sceneInformation?.LLLJMPGPFHC)
            {

                var enemy = entry.Value;
                x = 0;
                y = 0;
                if (enemy is not JENHPHEJIMK enemyInfo || enemyInfo.ToString() != "FIKCLEFPDLC")
                    continue;
                ShowFloatingText(" " + enemy.CJJFFIAHCLJ.isEnemy + " " + enemy.CJJFFIAHCLJ.ignoreHit, true);
                if (!enemy.CJJFFIAHCLJ.isEnemy) continue;
                if (enemy.CJJFFIAHCLJ.ignoreHit) continue;
                //FIKCLEFPDLC is enemy, OIKPMFBGJPD is x, LEKACFCHONN is y
                //var dist = Vector3.Distance(pp, new Vector3(enemy.OIKPMFBGJPD, enemy.LEKACFCHONN));
                
                Vector3 eP = new Vector3(enemy.OIKPMFBGJPD, enemy.LEKACFCHONN);
                float dist = Vector3.Distance(this.pp, eP);
                    if (dist < cDist && dist < 10.0f && !enemy.CJJFFIAHCLJ.ignoreHit && enemy.CJJFFIAHCLJ.isEnemy)
                    {
                        cDist = dist;
                        cEP = eP;
                    }
                    if (cDist < float.MaxValue && !enemy.CJJFFIAHCLJ.ignoreHit && enemy.CJJFFIAHCLJ.isEnemy)
                    {
                        AimAtEnemy(cEP);
                    }
            }
            //ShowFloatingText("Cringe", true);

        }
        private void AimAtEnemy(Vector3 enemyPosition)
        {
            this.x = enemyPosition.x;
            this.y = enemyPosition.y;
            this.ep = new Vector3(this.x, this.y);
            this.firstrun = false;
            this.sx = this.x - this.px;
            this.sy = this.y - this.py;
            this.angleDegrees = sceneInformation?.BMGIOONAILC.AFGMGNPBBNE ?? 0.0f;
            this.angleRadians = this.angleDegrees * Mathf.Deg2Rad;
            Vector3 screenPosition = CalculateScreenPosition(this.sx, this.sy, this.angleRadians);
            if (this.x > 0.0f && this.y > 0.0f)
            {
                MouseMover.SetCursorPos((int)screenPosition.x, (int)screenPosition.y);
            }
        }

        private Vector3 CalculateScreenPosition(float sx, float sy, float angleRadians)
        {
            float adjustedX = sx * 50f;
            float adjustedY = sy * 50f;
            float cosAngle = Mathf.Cos(angleRadians);
            float sinAngle = Mathf.Sin(angleRadians);
            float screenX = adjustedX * cosAngle - adjustedY * sinAngle + this.centerX;
            float screenY = adjustedX * sinAngle + adjustedY * cosAngle + this.centerY;

            return new Vector3(screenX, screenY);
        }
        public void ShowFloatingText(string text, Color32 color)
        {
            Il2CppSystem.Nullable<Color32> newColor = new Il2CppSystem.Nullable<Color32>(color);
            var effectType = BPBCHGDJEDB.Xp;
            if (!_floatingTextInit) InitializeFloatingText(effectType, newColor);
            sceneInformation?.BMGIOONAILC.LFGMMLKFAPL.iGUIManager.ShowFloatingText(effectType, text, newColor, 0.0f, 0.0f, 0.0f);
        }
        public void ShowFloatingText2(string text, Color32 color)
        {
            Il2CppSystem.Nullable<Color32> newColor = new Il2CppSystem.Nullable<Color32>(color);
            var effectType = BPBCHGDJEDB.LevelUp;
            if (!_floatingTextInit) InitializeFloatingText(effectType, newColor);
            sceneInformation?.BMGIOONAILC.LFGMMLKFAPL.iGUIManager.ShowFloatingText(effectType, text, newColor, 0.0f, 0.0f, 0.0f);
        }

        private void InitializeFloatingText(BPBCHGDJEDB effectType, Il2CppSystem.Nullable<Color32> newColor)
        {
            for (int i = 0; i < 12; i++)
                sceneInformation?.BMGIOONAILC.LFGMMLKFAPL.iGUIManager.ShowFloatingText(effectType, "", newColor, 0.0f, 0.0f,0.0f);
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

            sceneInformation = ApplicationManagerObj._KICMDDAKCDF_k__BackingField;
            if (sceneInformation == null)
            {
                Msg("ERROR: Failed to find sceneInformation.");
                return;

            }
        }
    }
}