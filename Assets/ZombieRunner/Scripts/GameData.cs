using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static int Coin
    {
        get
        {
            return 1;
        }
        set
        {
            
        }
    }

    public static int StartZombieCost
    {
        get
        {
            return PlayerPrefs.GetInt("StartZombieCost", 10);
        }
        set
        {
            PlayerPrefs.SetInt("StartZombieCost", value);
        }
    }
    
    public static int ZombieMaxHealthCost
    {
        get
        {
            return PlayerPrefs.GetInt("ZombieMaxHealthCost", 10);
        }
        set
        {
            PlayerPrefs.SetInt("ZombieMaxHealthCost", value);
        }
    }
    
    public static int LevelCoinMultiplierCost
    {
        get
        {
            return PlayerPrefs.GetInt("LevelCoinMultiplierCost", 10);
        }
        set
        {
            PlayerPrefs.SetInt("LevelCoinMultiplierCost", value);
        }
    }
    
    public static int StartZombieLevel
    {
        get
        {
            return PlayerPrefs.GetInt("StartZombieLevel", 0);
        }
        set
        {
            PlayerPrefs.SetInt("StartZombieLevel", value);
        }
    }
    public static int ZombieMaxHealthLevel
    {
        get
        {
            return PlayerPrefs.GetInt("ZombieMaxHealthLevel", 0);
        }
        set
        {
            PlayerPrefs.SetInt("ZombieMaxHealthLevel", value);
        }
    }
    
    public static int LevelCoinMultiplierLevel
    {
        get
        {
            return PlayerPrefs.GetInt("LevelCoinMultiplierLevel", 0);
        }
        set
        {
            PlayerPrefs.SetInt("LevelCoinMultiplierLevel", value);
        }
    }
    
    
    public static int StartZombie
    {
        get
        {
            return PlayerPrefs.GetInt("StartZombie", 1);
        }
        set
        {
            PlayerPrefs.SetInt("StartZombie", value);
        }
    }
    
    public static int ZombieMaxHealth
    {
        get
        {
            return PlayerPrefs.GetInt("ZombieMaxHealth", 5);
        }
        set
        {
            PlayerPrefs.SetInt("ZombieMaxHealth", value);
        }
    }

    public static float LevelCoinMultiplier
    {
        get
        {
            return PlayerPrefs.GetFloat("LevelCoinMultiplier", 1f);
        }
        set
        {
            PlayerPrefs.SetFloat("LevelCoinMultiplier", value);
        }
    }

    public static int LevelLadderLevel;
    
    public static int ManGoldAmount
    {
        get
        {
            return PlayerPrefs.GetInt("ManGoldAmount", 1);
        }
        set
        {
            PlayerPrefs.SetInt("ManGoldAmount", value);
        }
    }

    public List<LevelPlayData> LevelPlayDatas
    {
        get
        {
            //List<LevelPlayData> levelPlayDatas = JsonUtility.FromJson<LevelPlayData>(Application.persistentDataPath + "/LevelPlayData.json");
            return new List<LevelPlayData>();
        }
        set
        {
            
        }
    }
    
    public static int SelectZombieSkin
    {
        get
        {
            return PlayerPrefs.GetInt("SelectZombieSkin", 0);
        }
        set
        {
            PlayerPrefs.SetInt("SelectZombieSkin", value);
        }
    }
    
    public static int DailyDayIndex
    {
        get
        {
            return PlayerPrefs.GetInt("DailyDayIndex", 0);
        }
        set
        {
            PlayerPrefs.SetInt("DailyDayIndex", value);
        }
    }

    public static GameSkinsData GameSkinData
    {
        get
        {
            GameSkinsData defaultData = new GameSkinsData();
            
            List<SkinItemData> skinItemDatas = new List<SkinItemData>();
            for (int i = 0; i < 4; i++)
            {
                SkinItemData skinItemData = new SkinItemData();
                string name = "Purple Pant Zom";
                int cost = 0;
                switch (i)
                {
                    case 1:
                    {
                        name = "Red Hat Zom";
                        cost = 100;
                        break;
                    }
                    case 2:
                    {
                        name = "Astronaut Zom";
                        cost = 200;
                        break;
                    }
                    case 3:
                    {
                        name = "Skeleton Zom";
                        cost = 300;
                        break;
                    }
                }

                skinItemData.name = name;
                skinItemData.index = i;
                skinItemData.cost = cost;
                skinItemData.owned = false;
                if(i == 0) skinItemData.owned = true;
                skinItemDatas.Add(skinItemData);
            }

            defaultData.skinItemDatas = skinItemDatas;
            
            string convertedData = Newtonsoft.Json.JsonConvert.SerializeObject(defaultData);
            Debug.Log("Current:" + PlayerPrefs.GetString("GameSkinData", convertedData));
            var data = PlayerPrefs.GetString("GameSkinData", convertedData);
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<GameSkinsData>(data);
        }
        set
        {
            PlayerPrefs.SetString("GameSkinData", Newtonsoft.Json.JsonConvert.SerializeObject(value));
            PlayerPrefs.Save();
        }
    }
    
    public static DateTime GetDateTimeDailyReward()
    {
        return GetDateTime("DateTimeDailyReward", DateTime.MinValue);
    }
    
    public static void SetDateTimeDailyQuest(DateTime value)
    {
        PlayerPrefs.SetString("DateTimeDailyReward", value.ToBinary().ToString());
    }
    
    public static DateTime Castle1DateTime
    {
        get
        {
            return GetDateTime("Castle1DateTime", DateTime.MinValue);
        }
        set
        {
            PlayerPrefs.SetString("Castle1DateTime", value.ToBinary().ToString());
        }
    }
    
    public static DateTime Castle2DateTime
    {
        get
        {
            return GetDateTime("Castle2DateTime", DateTime.MinValue);
        }
        set
        {
            PlayerPrefs.SetString("Castle2DateTime", value.ToBinary().ToString());
        }
    }
    
    public static DateTime Castle3DateTime
    {
        get
        {
            return GetDateTime("Castle3DateTime", DateTime.MinValue);
        }
        set
        {
            PlayerPrefs.SetString("Castle3DateTime", value.ToBinary().ToString());
        }
    }
    
    public static DateTime Castle4DateTime
    {
        get
        {
            return GetDateTime("Castle4DateTime", DateTime.MinValue);
        }
        set
        {
            PlayerPrefs.SetString("Castle4DateTime", value.ToBinary().ToString());
        }
    }
    
    public static DateTime GetDateTime(string key, DateTime defaultValue)
    {
        string @string = PlayerPrefs.GetString(key);
        DateTime result = defaultValue;
        if (!string.IsNullOrEmpty(@string))
        {
            long dateData = Convert.ToInt64(@string);
            result = DateTime.FromBinary(dateData);
        }
        return result;
    }
    
    public static int CurrentCastleIndex
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentCastleIndex", -1);
        }
        set
        {
            PlayerPrefs.SetInt("CurrentCastleIndex", Mathf.Min(value, 3));
        }
    }
}

[Serializable]
public class LevelPlayData
{
    public int level;
    public bool isPlayed;
}

[Serializable]
public class GameSkinsData
{
    public List<SkinItemData> skinItemDatas;

    public void Init(List<SkinItemData> value)
    {
        skinItemDatas = value;
    }
}

[Serializable]
public class SkinItemData
{
    public string name;
    public int index;
    public bool owned;
    public int cost;
}
