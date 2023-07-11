using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HyperCasual.Core;
using HyperCasual.Gameplay;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class used to control a player in a Runner
    /// game. Includes logic for player movement as well as 
    /// other gameplay logic.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <summary> Returns the PlayerController. </summary>
        public static PlayerController Instance => s_Instance;
        static PlayerController s_Instance;

        [SerializeField] private List<GameObject> zombiePrefabs;
        public GameObject selectedZombiePrefab;
        public List<GameObject> runningManSeaPrefabs;
        [SerializeField] private ParticleSystem zombieIncreaseEffect;
        [SerializeField] private ParticleSystem zombieLostEffect;
        [SerializeField] private ParticleSystem zombieScratchEffect;
        [SerializeField] private AudioSource runAudioSource;
        [SerializeField] private List<GameObject> smokeTrailEffects;
        
        private PlayerFollower _playerFollower;
        public GameObject playerFollowePrefab;
        public List<Zombie> zombieList;
        public List<Transform> spawnPointList;
        public AudioSource deathZomAudioSource;
        public List<AudioClip> deathZomSoundList;
        /*[SerializeField]
        List<Animator> m_Animator;*/

        [SerializeField]
        SkinnedMeshRenderer m_SkinnedMeshRenderer;

        [SerializeField]
        PlayerSpeedPreset m_PlayerSpeed = PlayerSpeedPreset.Medium;

        [SerializeField]
        float m_CustomPlayerSpeed = 10.0f;

        [SerializeField]
        float m_AccelerationSpeed = 10.0f;

        [SerializeField]
        float m_DecelerationSpeed = 20.0f;

        [SerializeField]
        float m_HorizontalSpeedFactor = 0.5f;

        [SerializeField]
        float m_ScaleVelocity = 2.0f;

        [SerializeField]
        bool m_AutoMoveForward = true;

        Vector3 m_LastPosition;
        float m_StartHeight;

        private bool isFinishRun = false;
        public bool isInMenu = true;

        const float k_MinimumScale = 0.1f;
        static readonly string s_Speed = "Speed";
        static readonly string s_MotionSpeed = "MotionSpeed";

        enum PlayerSpeedPreset
        {
            Slow,
            Medium,
            Fast,
            Custom
        }

        public Transform m_Transform;
        Vector3 m_StartPosition;
        bool m_HasInput;
        float m_MaxXPosition;
        float m_XPos;
        float m_ZPos;
        float m_TargetPosition;
        float m_Speed;
        float m_TargetSpeed;
        Vector3 m_Scale;
        Vector3 m_TargetScale;
        Vector3 m_DefaultScale;

        const float k_HalfWidth = 0.5f;

        /// <summary> The player's root Transform component. </summary>
        public Transform Transform => m_Transform;

        /// <summary> The player's current speed. </summary>
        public float Speed => m_Speed;

        /// <summary> The player's target speed. </summary>
        public float TargetSpeed => m_TargetSpeed;

        /// <summary> The player's minimum possible local scale. </summary>
        public float MinimumScale => k_MinimumScale;

        /// <summary> The player's current local scale. </summary>
        public Vector3 Scale => m_Scale;

        /// <summary> The player's target local scale. </summary>
        public Vector3 TargetScale => m_TargetScale;

        /// <summary> The player's default local scale. </summary>
        public Vector3 DefaultScale => m_DefaultScale;

        /// <summary> The player's default local height. </summary>
        public float StartHeight => m_StartHeight;

        /// <summary> The player's default local height. </summary>
        public float TargetPosition => m_TargetPosition;

        /// <summary> The player's maximum X position. </summary>
        public float MaxXPosition => m_MaxXPosition;

        void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;

            Initialize();
        }

        private void Start()
        {
            Light directionLight = FindObjectOfType<Light>();
            RenderSettings.sun = directionLight;
            RenderSettings.ambientSkyColor = new Color(0.92f, 0.92f, 0.92f, 1);
            
            isInMenu = true;
            

            int levelIndex = GameManager.Instance.m_CurrentLevel.LevelIndex;
            Debug.Log("Current level:" + levelIndex);
            if (levelIndex > 10 && levelIndex <= 20)
            {
                Instantiate(GameManager.Instance.seaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                var allRunningManInScene = GameObject.FindObjectsByType<RunningMan>(FindObjectsSortMode.None);
                for (int i = 0; i < allRunningManInScene.Length; i++)
                {
                    int rand = Random.Range(0, 100);
                    if (rand > 50)
                    {
                        Instantiate(runningManSeaPrefabs[Random.Range(0, runningManSeaPrefabs.Count)], allRunningManInScene[i].transform.position, allRunningManInScene[i].transform.rotation);
                        Destroy(allRunningManInScene[i].gameObject);
                    }
                }
            }
            else
            {
                Instantiate(GameManager.Instance.groundPrefab, new Vector3(0, -1, 205), Quaternion.identity);

                int numberOfBuildings = (int)(400f / 50f) + 1;
                //Debug.Log(numberOfBuildings);
                //Debug.Log(GameManager.Instance);
                for (int i = 0; i <= numberOfBuildings; i++)
                {
                    var buildObj1 = Instantiate(GameManager.Instance.leftBuildingPrefabs[Random.Range(0, 3)]);
                    var buildObj2 = Instantiate(GameManager.Instance.rightBuildingPrefabs[Random.Range(0, 3)]);
                    //buildObj1.transform.localScale = new Vector3(3, 3, 3);
                    //buildObj2.transform.localScale = new Vector3(3, 3, 3);
                    buildObj1.transform.position = new Vector3(-28f, 1, 50 * i);
                    buildObj2.transform.position = new Vector3(28f, 1, 50 * i);
                    //buildObj1.transform.rotation = Quaternion.Euler(-90f, 90, 0);
                    //buildObj2.transform.rotation = Quaternion.Euler(-90f, -90, 0);
                    //buildObj1.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
                    //buildObj2.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
                }
            }            

            GameManager.Instance.gameMainMenuUI.levelTxt.SetText("LEVEL " + levelIndex);
            //CameraParent.Instance.Setup(this);
        }

        /// <summary>
        /// Set up all necessary values for the PlayerController.
        /// </summary>
        public void Initialize()
        {
            isInMenu = true;
            selectedZombiePrefab = zombiePrefabs[GameData.SelectZombieSkin];
            _playerFollower =
                Instantiate(playerFollowePrefab,
                    new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z),
                    Quaternion.identity).GetComponent<PlayerFollower>();
            _playerFollower.Setup(this.transform);
            TogglePlayerFollower(true);

            ToggleZombieScratchEffect(false);
                
            for (int i = 1; i <= GameData.StartZombie; i++)
            {
                AddToFormation(false);
            }
            
            m_SkinnedMeshRenderer = zombieList[0].GetComponentInChildren<SkinnedMeshRenderer>();

            m_Transform = transform;
            m_StartPosition = m_Transform.position;
            m_DefaultScale = m_Transform.localScale;
            m_Scale = m_DefaultScale;
            m_TargetScale = m_Scale;

            if (m_SkinnedMeshRenderer != null)
            {
                m_StartHeight = m_SkinnedMeshRenderer.bounds.size.y;
            }
            else 
            {
                m_StartHeight = 1.0f;
            }

            ResetSpeed();
        }

        public void ResetZombies()
        {
            for (int i = 1; i <= GameData.StartZombie; i++)
            {
                AddToFormation();
            }
        }

        /// <summary>
        /// Returns the current default speed based on the currently
        /// selected PlayerSpeed preset.
        /// </summary>
        public float GetDefaultSpeed()
        {
            switch (m_PlayerSpeed)
            {
                case PlayerSpeedPreset.Slow:
                    return 5.0f;

                case PlayerSpeedPreset.Medium:
                    return 10.0f;

                case PlayerSpeedPreset.Fast:
                    return 20.0f;
            }

            return m_CustomPlayerSpeed;
        }

        /// <summary>
        /// Adjust the player's current speed
        /// </summary>
        public void AdjustSpeed(float speed)
        {
            m_TargetSpeed += speed;
            m_TargetSpeed = Mathf.Max(0.0f, m_TargetSpeed);
        }

        /// <summary>
        /// Reset the player's current speed to their default speed
        /// </summary>
        public void ResetSpeed()
        {
            m_Speed = 0.0f;
            m_TargetSpeed = GetDefaultSpeed();
        }

        /// <summary>
        /// Adjust the player's current scale
        /// </summary>
        public void AdjustScale(float scale)
        {
            m_TargetScale += Vector3.one * scale;
            m_TargetScale = Vector3.Max(m_TargetScale, Vector3.one * k_MinimumScale);
        }

        /// <summary>
        /// Reset the player's current speed to their default speed
        /// </summary>
        public void ResetScale()
        {
            m_Scale = m_DefaultScale;
            m_TargetScale = m_DefaultScale;
        }

        /// <summary>
        /// Returns the player's transform component
        /// </summary>
        public Vector3 GetPlayerTop()
        {
            return m_Transform.position + Vector3.up * (m_StartHeight * m_Scale.y - m_StartHeight);
        }

        /// <summary>
        /// Sets the target X position of the player
        /// </summary>
        public void SetDeltaPosition(float normalizedDeltaPosition)
        {
            if (m_MaxXPosition == 0.0f)
            {
                Debug.LogError("Player cannot move because SetMaxXPosition has never been called or Level Width is 0. If you are in the LevelEditor scene, ensure a level has been loaded in the LevelEditor Window!");
            }

            float fullWidth = m_MaxXPosition * 2.0f;
            m_TargetPosition = m_TargetPosition + fullWidth * normalizedDeltaPosition;
            m_TargetPosition = Mathf.Clamp(m_TargetPosition, -m_MaxXPosition, m_MaxXPosition);
            m_HasInput = true;
        }

        /// <summary>
        /// Stops player movement
        /// </summary>
        public void CancelMovement()
        {
            m_HasInput = false;
        }

        /// <summary>
        /// Set the level width to keep the player constrained
        /// </summary>
        public void SetMaxXPosition(float levelWidth)
        {
            // Level is centered at X = 0, so the maximum player
            // X position is half of the level width
            m_MaxXPosition = levelWidth * k_HalfWidth;
        }

        /// <summary>
        /// Returns player to their starting position
        /// </summary>
        public void ResetPlayer()
        {
            m_Transform.position = m_StartPosition;
            m_XPos = 0.0f;
            m_ZPos = m_StartPosition.z;
            m_TargetPosition = 0.0f;

            m_LastPosition = m_Transform.position;

            m_HasInput = false;

            ResetSpeed();
            ResetScale();
        }

        void Update()
        {
            if (isInMenu)
            {
                return;
            }
            if (isFinishRun) return;
            float deltaTime = Time.deltaTime;

            // Update Scale

            if (!Approximately(m_Transform.localScale, m_TargetScale))
            {
                m_Scale = Vector3.Lerp(m_Scale, m_TargetScale, deltaTime * m_ScaleVelocity);
                m_Transform.localScale = m_Scale;
            }

            // Update Speed

            if (!m_AutoMoveForward && !m_HasInput)
            {
                Decelerate(deltaTime, 0.0f);
            }
            else if (m_TargetSpeed < m_Speed)
            {
                Decelerate(deltaTime, m_TargetSpeed);
            }
            else if (m_TargetSpeed > m_Speed)
            {
                Accelerate(deltaTime, m_TargetSpeed);
            }

            float speed = m_Speed * deltaTime;

            // Update position

            m_ZPos += speed;

            if (m_HasInput)
            {
                float horizontalSpeed = speed * m_HorizontalSpeedFactor;

                float newPositionTarget = Mathf.Lerp(m_XPos, m_TargetPosition, horizontalSpeed);
                float newPositionDifference = newPositionTarget - m_XPos;

                newPositionDifference = Mathf.Clamp(newPositionDifference, -horizontalSpeed, horizontalSpeed);

                m_XPos += newPositionDifference;
            }

            /*float zomLeftMost = 100f, zomRightMost = -100f;
            foreach (var zom in zombieList)
            {
                if (zom.transform.position.x < zomLeftMost)
                {
                    zomLeftMost = zom.transform.position.x;
                }
                if (zom.transform.position.x > zomRightMost)
                {
                    zomRightMost = zom.transform.position.x;
                }
            }*/
            m_Transform.position = new Vector3(m_XPos, m_Transform.position.y, m_ZPos);

            foreach (var zombie in zombieList)
            {
                Animator animator = zombie.GetComponent<Animator>();
                if (animator != null && deltaTime > 0.0f)
                {
                    float distanceTravelledSinceLastFrame = (m_Transform.position - m_LastPosition).magnitude;
                    float distancePerSecond = distanceTravelledSinceLastFrame / deltaTime;

                    animator.SetFloat(s_Speed, distancePerSecond);
                    animator.SetFloat(s_MotionSpeed, Random.Range(0.9f, 1.1f));
                }
            }

            if (m_Transform.position != m_LastPosition)
            {
                m_Transform.forward = Vector3.Lerp(m_Transform.forward, (m_Transform.position - m_LastPosition).normalized, speed);
            }

            foreach (var zombie in zombieList)
            {
                zombie.transform.position = m_Transform.position + zombie.offsetPosToOriginal;
                zombie.transform.forward = m_Transform.forward;
            }
            m_LastPosition = m_Transform.position;
        }

        void Accelerate(float deltaTime, float targetSpeed)
        {
            m_Speed += deltaTime * m_AccelerationSpeed;
            m_Speed = Mathf.Min(m_Speed, targetSpeed);
        }

        void Decelerate(float deltaTime, float targetSpeed)
        {
            m_Speed -= deltaTime * m_DecelerationSpeed;
            m_Speed = Mathf.Max(m_Speed, targetSpeed);
        }

        bool Approximately(Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
        }

        private bool isRememberIndexEnable = false;
        private int rememberIndex = 0;
        
        public void AddToFormation(bool hasEffect = true)
        {
            //Debug.Log("Add to formation");
            int lastIndex = 0;
            if (!isRememberIndexEnable)
            {
                if (zombieList.Count == 0)
                {
                    lastIndex = 0;
                }
                else
                {
                    lastIndex = zombieList.Count;
                }
            }
            else
            {
                lastIndex = rememberIndex;
                isRememberIndexEnable = false;
            }
            
            lastIndex = Mathf.Clamp(lastIndex, 0, spawnPointList.Count - 1);

            Vector3 position = spawnPointList[lastIndex].position;
            
            //Debug.Log(spawnPointList[lastIndex].position);
            var zombieObj = Instantiate(selectedZombiePrefab, spawnPointList[lastIndex].position, Quaternion.identity);
            if (hasEffect)
            {
                /*var effect = Instantiate(zombieIncreaseEffect);
                effect.transform.position = spawnPointList[lastIndex].position + new Vector3(0, 0.5f, 0);*/
            }
            Zombie zombieComponent = zombieObj.GetComponent<Zombie>();
            zombieComponent.offsetPosToOriginal = spawnPointList[lastIndex].localPosition;
            zombieComponent.indexInSpawn = lastIndex;
            //Debug.Log(zombieComponent.offsetPosToOriginal);
            
            Animator zombieAnimator = zombieObj.GetComponent<Animator>();
            zombieList.Add(zombieComponent);

            if (_playerFollower != null)
            {
                _playerFollower.numberZombiesTMP.SetText(zombieList.Count.ToString());
            }
            //AudioManager.Instance.PlayEffect(SoundID.ZombieAdd);
            if (zombieList.Count > 1)
            {
                smokeTrailEffects[1].gameObject.SetActive(true);
            }
            if (zombieList.Count > 2)
            {
                smokeTrailEffects[2].gameObject.SetActive(true);
            }
        }

        public void RemoveFromFormation()
        {
            Zombie lastZom = zombieList[zombieList.Count - 1];
            /*var effect = Instantiate(zombieLostEffect);
            effect.transform.position = lastZom.transform.position + new Vector3(0, 0.5f, 0);*/
            zombieList.Remove(lastZom);
            Destroy(lastZom.gameObject);
            
            if (zombieList.Count == 0 && !isInMenu)
            {
                GameManager.Instance.Lose();
            }
            
            if (_playerFollower != null)
            {
                _playerFollower.numberZombiesTMP.SetText(zombieList.Count.ToString());
            }
            
            if (zombieList.Count < 3)
            {
                smokeTrailEffects[2].gameObject.SetActive(false);
            }
            if (zombieList.Count < 2)
            {
                smokeTrailEffects[1].gameObject.SetActive(false);
            }
        }
        
        public void RemoveFromFormation(Zombie selectedZom, int index)
        {
            /*var effect = Instantiate(zombieLostEffect);
            effect.transform.position = selectedZom.transform.position + new Vector3(0, 0.5f, 0);*/
            zombieList.Remove(selectedZom);
            Destroy(selectedZom.gameObject);

            isRememberIndexEnable = true;
            rememberIndex = index;

            if (zombieList.Count == 0 && !isInMenu)
            {
                GameManager.Instance.Lose();
            }
            
            if (_playerFollower != null)
            {
                _playerFollower.numberZombiesTMP.SetText(zombieList.Count.ToString());
            }
            
            if (zombieList.Count < 3)
            {
                smokeTrailEffects[2].gameObject.SetActive(false);
            }
            if (zombieList.Count < 2)
            {
                smokeTrailEffects[1].gameObject.SetActive(false);
            }
        }

        public void DestroyAllZombie()
        {
            int maxCount = 100;
            while (zombieList.Count > 0)
            {
                RemoveFromFormation();
                maxCount--;
                if (maxCount < 0)
                {
                    break;
                }
            }
        }

        public void ZombieFinishRun()
        {
            isFinishRun = true;
            foreach (var zombie in zombieList)
            {
                zombie.isFinishRun = true;
            }

            var allSpawnables = GameObject.FindObjectsByType<Spawnable>(FindObjectsSortMode.None);
            for (int i = 0; i < allSpawnables.Length; i++)
            {
                Destroy(allSpawnables[i].gameObject);
            }

            TogglePlayerFollower(false);
            ToggleRunSound(false);
            ToggleSmokeTrailEffects(false);
        }

        public void EnablePlay()
        {
            isInMenu = false;
            /*var allCar = FindObjectsByType<GunCar>(FindObjectsSortMode.None);
            for (int i = 0; i < allCar.Length; i++)
            {
                allCar[i].StartCar();
            }*/
            var allTrapBalls = FindObjectsByType<TrapBall>(FindObjectsSortMode.None);
            for (int i = 0; i < allTrapBalls.Length; i++)
            {
                allTrapBalls[i].enablePlay = true;
            }
            var allPolices = FindObjectsByType<PoliceShooting>(FindObjectsSortMode.None);
            for (int i = 0; i < allPolices.Length; i++)
            {
                allPolices[i].enablePlay = true;
            }
            ToggleRunSound(true);
            ToggleSmokeTrailEffects(true);
            if (zombieList.Count < 3)
            {
                smokeTrailEffects[2].gameObject.SetActive(false);
            }
            if (zombieList.Count < 2)
            {
                smokeTrailEffects[1].gameObject.SetActive(false);
            }
            Debug.Log("show hud");
            UIManager.Instance.Show<Hud>();
        }

        public void DisablePlay()
        {
            isInMenu = true;
        }

        public void ChangeZombieSkin()
        {
            selectedZombiePrefab = zombiePrefabs[GameData.SelectZombieSkin];
            DestroyAllZombie();
            for (int i = 1; i <= GameData.StartZombie; i++)
            {
                AddToFormation(false);
            }
        }
        
        public void ChangeZombieStat()
        {
            DestroyAllZombie();
            for (int i = 1; i <= GameData.StartZombie; i++)
            {
                AddToFormation(false);
            }
        }

        public void TogglePlayerFollower(bool value)
        {
            _playerFollower.gameObject.SetActive(value);
        }

        public void ToggleZombieScratchEffect(bool value, Transform target = null)
        {
            if (value && target != null)
            {
                zombieScratchEffect.gameObject.SetActive(true);
                zombieScratchEffect.transform.position = target.position;
                zombieScratchEffect.Play();
            }

            if (!value)
            {
                zombieScratchEffect.Stop();
                zombieScratchEffect.gameObject.SetActive(false);
            }
        }

        public void ToggleRunSound(bool value)
        {
            if (value)
            {
                runAudioSource.Play();
            }
            else
            {
                runAudioSource.Stop();
            }
        }

        public void ToggleSmokeTrailEffects(bool value)
        {
            if (value)
            {
                foreach (var smokeTrailObj in smokeTrailEffects)
                {
                    smokeTrailObj.SetActive(true);
                }
            }
            else
            {
                foreach (var smokeTrailObj in smokeTrailEffects)
                {
                    smokeTrailObj.SetActive(false);
                }
            }
        }

        public void PlayDeathZomSound(SoundID soundID)
        {
            Debug.Log("zom death: " + soundID);
            switch (soundID)
            {
                case SoundID.MaleDie:
                {
                    deathZomAudioSource.clip = deathZomSoundList[0];
                    break;
                }
                case SoundID.WomanDie:
                {
                    deathZomAudioSource.clip = deathZomSoundList[1];
                    break;
                }
                case SoundID.CrowdDie:
                {
                    deathZomAudioSource.clip = deathZomSoundList[2];
                    break;
                }
            }
            deathZomAudioSource.Play();
        }

        public void OnLose()
        {
            isFinishRun = true;
            foreach (var zombie in zombieList)
            {
                zombie.isFinishRun = true;
                zombie.animator.SetFloat("Speed", 0);
                zombie.Damage(9999);
            }

            isInMenu = true;
        }
    }
}