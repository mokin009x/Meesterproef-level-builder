using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveLoad
{
    public static void Save()
    {
        // Saving method

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.OpenOrCreate);
        SaveData saveData = new SaveData();
        
        
        saveData.levelBlocksIds = LevelBuildAndPlayManager.Instance.levelBLocksIds;
        saveData.gridIds = LevelBuildAndPlayManager.Instance.blockGridIds;
        saveData.monsterPath = LevelBuildAndPlayManager.Instance.monsterPath;
        saveData.buildArea = LevelBuildAndPlayManager.Instance.buildArea;
        saveData.blockRotationX = LevelBuildAndPlayManager.Instance.blockRotationX;
        saveData.blockRotationY = LevelBuildAndPlayManager.Instance.blockRotationY;
        saveData.blockRotationZ = LevelBuildAndPlayManager.Instance.blockRotationZ;

        bf.Serialize(file, saveData);
        Debug.Log("Saved File: " + file.Name);
        file.Close();
    }

    public static bool Load()
    {
        // Loading method

        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveData saveData = (SaveData) bf.Deserialize(file);


            LevelBuildAndPlayManager.Instance.levelBLocksIds = saveData.levelBlocksIds;
            LevelBuildAndPlayManager.Instance.blockGridIds = saveData.gridIds;
            LevelBuildAndPlayManager.Instance.monsterPath = saveData.monsterPath;
            LevelBuildAndPlayManager.Instance.buildArea = saveData.buildArea;
            LevelBuildAndPlayManager.Instance.blockRotationX = saveData.blockRotationX;
            LevelBuildAndPlayManager.Instance.blockRotationY = saveData.blockRotationY;
            LevelBuildAndPlayManager.Instance.blockRotationZ = saveData.blockRotationZ;



            Debug.Log("Loaded File: " + file.Name);
            file.Close();
            return true;
        }
        return false;
    }
}