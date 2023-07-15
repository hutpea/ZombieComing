using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using HyperCasual.Gameplay;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class used to store game state information, 
    /// load levels, and save/load statistics as applicable.
    /// The GameManager class manages all game-related 
    /// state changes.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Returns the GameManager.
        /// </summary>
        public static GameManager Instance => s_Instance;

        static GameManager s_Instance;

        public AbstractGameEvent m_WinEvent;

        [SerializeField] AbstractGameEvent m_LoseEvent;

        [SerializeField] private EndGameEvent endGameEvent;
        [SerializeField] private EndGameEvent2 endGameEvent2;
        [SerializeField] private EndGameEventCastle endGameEventCastle;
        [SerializeField] private GameObject gameMenuPrefab;
        public GameMainMenuUI gameMainMenuUI;
        [SerializeField] private List<GameObject> runningManList;
        public Material skyboxMaterial;
        public GameObject groundPrefab;
        public List<GameObject> leftBuildingPrefabs;
        public List<GameObject> rightBuildingPrefabs;
        public GameObject seaPrefab;

        public LevelDefinition m_CurrentLevel;

        /// <summary>
        /// Returns true if the game is currently active.
        /// Returns false if the game is paused, has not yet begun,
        /// or has ended.
        /// </summary>
        public bool IsPlaying => m_IsPlaying;

        bool m_IsPlaying;
        GameObject m_CurrentLevelGO;
        GameObject m_CurrentTerrainGO;
        GameObject m_LevelMarkersGO;

        List<Spawnable> m_ActiveSpawnables = new List<Spawnable>();

        public GameObject endLineObject;

#if UNITY_EDITOR
        bool m_LevelEditorMode;
#endif

        void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;

#if UNITY_EDITOR
            // If LevelManager already exists, user is in the LevelEditorWindow
            if (LevelManager.Instance != null)
            {
                StartGame();
                m_LevelEditorMode = true;
            }
#endif
        }

        /// <summary>
        /// This method calls all methods necessary to load and
        /// instantiate a level from a level definition.
        /// </summary>
        public void LoadLevel(LevelDefinition levelDefinition)
        {
            m_CurrentLevel = levelDefinition;
            LoadLevel(m_CurrentLevel, ref m_CurrentLevelGO);
            CreateTerrain(m_CurrentLevel, ref m_CurrentTerrainGO);
            PlaceLevelMarkers(m_CurrentLevel, ref m_LevelMarkersGO);

            StartGame();
        }

        /// <summary>
        /// This method calls all methods necessary to restart a level,
        /// including resetting the player to their starting position
        /// </summary>
        public void ResetLevel()
        {
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.ResetPlayer();
            }

            if (CameraManager.Instance != null)
            {
                CameraManager.Instance.ResetCamera();
            }

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.ResetSpawnables();
            }
        }

        /// <summary>
        /// This method loads and instantiates the level defined in levelDefinition,
        /// storing a reference to its parent GameObject in levelGameObject
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that holds all information needed to 
        /// load and instantiate a level.
        /// </param>
        /// <param name="levelGameObject">
        /// A new GameObject to be created, acting as the parent for the level to be loaded
        /// </param>
        public static void LoadLevel(LevelDefinition levelDefinition, ref GameObject levelGameObject)
        {
            if (levelDefinition == null)
            {
                Debug.LogError("Invalid Level!");
                return;
            }

            if (levelGameObject != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(levelGameObject);
                }
                else
                {
                    DestroyImmediate(levelGameObject);
                }
            }
            //Debug.Log("(3) " + levelDefinition);
            //LevelManager levelManager = levelGameObject.AddComponent<LevelManager>() ?? LevelManager.Instance;
            LevelManager levelManager;
            if (LevelManager.Instance != null)
            {
                levelGameObject = LevelManager.Instance.gameObject;
                levelManager = LevelManager.Instance;
            }
            else
            {
                levelGameObject = new GameObject("LevelManager");
                levelManager = levelGameObject.AddComponent<LevelManager>();
            }
            //Debug.Log("(4) " + (levelManager == null));
            
            levelManager.LevelDefinition = levelDefinition;

            Transform levelParent = levelGameObject.transform;
            

            for (int i = 0; i < levelDefinition.Spawnables.Length; i++)
            {
                LevelDefinition.SpawnableObject spawnableObject = levelDefinition.Spawnables[i];

                if (spawnableObject.SpawnablePrefab == null)
                {
                    continue;
                }

                Vector3 position = spawnableObject.Position;
                Vector3 eulerAngles = spawnableObject.EulerAngles;
                Vector3 scale = spawnableObject.Scale;

                GameObject go = null;

                if (Application.isPlaying)
                {
                    go = GameObject.Instantiate(spawnableObject.SpawnablePrefab, position,
                        Quaternion.Euler(eulerAngles));
                }
                else
                {
#if UNITY_EDITOR
                    go = (GameObject)PrefabUtility.InstantiatePrefab(spawnableObject.SpawnablePrefab);
                    go.transform.position = position;
                    go.transform.eulerAngles = eulerAngles;
#endif
                }

                if (go == null)
                {
                    return;
                }

                // Set Base Color
                Spawnable spawnable = go.GetComponent<Spawnable>();
                if (spawnable != null)
                {
                    spawnable.SetBaseColor(spawnableObject.BaseColor);
                    spawnable.SetScale(scale);
                    levelManager.AddSpawnable(spawnable);
                }

                if (go != null)
                {
                    go.transform.SetParent(levelParent);
                }
            }
            //Debug.Log("LoadLevel Complete!");
        }

        public void UnloadCurrentLevel()
        {
            if (m_CurrentLevelGO != null)
            {
                GameObject.Destroy(m_CurrentLevelGO);
            }

            if (m_LevelMarkersGO != null)
            {
                GameObject.Destroy(m_LevelMarkersGO);
            }

            if (m_CurrentTerrainGO != null)
            {
                GameObject.Destroy(m_CurrentTerrainGO);
            }

            m_CurrentLevel = null;
        }

        void StartGame()
        {
            isInCastleMode = false;
            gameMainMenuUI = Instantiate(gameMenuPrefab).GetComponentInChildren<GameMainMenuUI>();
            GameData.LevelLadderLevel = 1;
            /*
            GameData.LevelCoinMultiplier += 0.1f;
            GameData.StartZombie += 1;
            GameData.ZombieMaxHealth += 2;*/
            Debug.Log("Start Game");
            isWin = false;
            ResetLevel();
            m_IsPlaying = true;
            CameraManager.Instance.StartGameCamera();
            endGameEvent2.StartGame();
            if (gameMainMenuUI != null)
            {
                gameMainMenuUI.gameObject.SetActive(true);
            }

            Debug.Log("Hide Hud");
            UIManager.Instance.GetView<Hud>().Hide();
            PlayerController.Instance.DisablePlay();
            
            RenderSettings.skybox = skyboxMaterial;
            RenderSettings.ambientSkyColor = Color.white;
            //Running Man
            /*var allMan = GameObject.FindGameObjectsWithTag("Man");
            Debug.Log(allMan.Length);
            for (int i = 0; i < allMan.Length; i++)
            {
                int chance = UnityEngine.Random.Range(0, 100);
                if (chance < 33)
                {
                    Vector3 position = allMan[i].transform.position;
                    Destroy(allMan[i].gameObject);
                    Instantiate(runningManList[0], position, Quaternion.identity);
                }
                else if (chance >= 33 && chance < 66)
                {
                    Vector3 position = allMan[i].transform.position;
                    Destroy(allMan[i].gameObject);
                    Instantiate(runningManList[1], position, Quaternion.identity);
                }
            }*/
        }

        /// <summary>
        /// Creates and instantiates the StartPrefab and EndPrefab defined inside
        /// the levelDefinition.
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that defines the start and end prefabs.
        /// </param>
        /// <param name="levelMarkersGameObject">
        /// A new GameObject that is created to be the parent of the start and end prefabs.
        /// </param>
        public static void PlaceLevelMarkers(LevelDefinition levelDefinition, ref GameObject levelMarkersGameObject)
        {
            if (levelMarkersGameObject != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(levelMarkersGameObject);
                }
                else
                {
                    DestroyImmediate(levelMarkersGameObject);
                }
            }

            levelMarkersGameObject = new GameObject("Level Markers");

            GameObject start = levelDefinition.StartPrefab;
            GameObject end = levelDefinition.EndPrefab;

            if (start != null)
            {
                GameObject go = GameObject.Instantiate(start,
                    new Vector3(start.transform.position.x, start.transform.position.y, 0.0f), Quaternion.identity);
                go.transform.SetParent(levelMarkersGameObject.transform);
            }

            if (end != null)
            {
                GameObject go = GameObject.Instantiate(end,
                    new Vector3(end.transform.position.x, end.transform.position.y, levelDefinition.LevelLength - 60f),
                    Quaternion.identity);
                go.transform.SetParent(levelMarkersGameObject.transform);
                //GameManager.Instance.SetEndObj(go);
            }
        }

        /*public void SetEndObj(GameObject obj)
        {
            endLineObject = obj;
        }*/

        /// <summary>
        /// Creates and instantiates a Terrain GameObject, built
        /// to the specifications saved in levelDefinition.
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that defines the terrain size.
        /// </param>
        /// <param name="terrainGameObject">
        /// A new GameObject that is created to hold the terrain.
        /// </param>
        public static void CreateTerrain(LevelDefinition levelDefinition, ref GameObject terrainGameObject)
        {
            TerrainGenerator.TerrainDimensions terrainDimensions = new TerrainGenerator.TerrainDimensions()
            {
                Width = levelDefinition.LevelWidth,
                Length = levelDefinition.LevelLength,
                StartBuffer = levelDefinition.LevelLengthBufferStart,
                EndBuffer = levelDefinition.LevelLengthBufferEnd,
                Thickness = levelDefinition.LevelThickness
            };
            //TerrainGenerator.CreateTerrain(terrainDimensions, levelDefinition.TerrainMaterial, ref terrainGameObject);
            TerrainGenerator.CreateTerrain2(terrainDimensions);
        }

        private bool isWin = false;

        public void Win()
        {
            if (!isWin)
            {
                Debug.Log("WIN!");
                //m_WinEvent.Raise();
                if (isInCastleMode)
                {
                    endGameEventCastle.Init();
                }
                else
                {
                    endGameEvent2.Init();
                }
/*#if UNITY_EDITOR
            if (m_LevelEditorMode)
            {
                ResetLevel();
            }
#endif*/
                isWin = true;
            }
        }

        public void Lose()
        {
            Debug.Log("LOSE!");
            PlayerController.Instance.OnLose();
            m_LoseEvent.Raise();

#if UNITY_EDITOR
            if (m_LevelEditorMode)
            {
                ResetLevel();
            }
#endif
        }

        private bool isInCastleMode = false;
        
        public void StartCastleMode()
        {
            isInCastleMode = true;
            var allLadder = GameObject.FindGameObjectsWithTag("Ladder");
            for (int i = 0; i < allLadder.Length; i++)
            {
                Destroy(allLadder[i].gameObject);
            }

            endGameEventCastle.StartGame();
            
            gameMainMenuUI.StartPlayBtn();
        }
    }
}