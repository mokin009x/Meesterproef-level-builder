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
        
        //build blocks
        saveData.levelBlocksIds = LevelBuildAndPlayManager.Instance.levelBlocksIds;
        saveData.buildBlockGridIds = LevelBuildAndPlayManager.Instance.buildBlockGridIds;
        //build block rotation
        saveData.blockRotationX = LevelBuildAndPlayManager.Instance.blockRotationX;
        saveData.blockRotationY = LevelBuildAndPlayManager.Instance.blockRotationY;
        saveData.blockRotationZ = LevelBuildAndPlayManager.Instance.blockRotationZ;
        //special blocks
        saveData.specialBlockIds = LevelBuildAndPlayManager.Instance.specialBlockIds;
        saveData.specialBlockGridIds = LevelBuildAndPlayManager.Instance.specialGridId;
        //special block rotation
        saveData.specialRotationX = LevelBuildAndPlayManager.Instance.specialRotationX;
        saveData.specialRotationY = LevelBuildAndPlayManager.Instance.specialRotationY;
        saveData.specialRotationZ = LevelBuildAndPlayManager.Instance.specialRotationZ;
        
        //misc tools
        saveData.monsterPath = LevelBuildAndPlayManager.Instance.monsterPathPosId;
        saveData.buildArea = LevelBuildAndPlayManager.Instance.buildArea;
        
        

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
            
            // build blocks
            LevelBuildAndPlayManager.Instance.levelBlocksIds = saveData.levelBlocksIds;
            LevelBuildAndPlayManager.Instance.buildBlockGridIds = saveData.buildBlockGridIds;
            //build block rotation 
            LevelBuildAndPlayManager.Instance.blockRotationX = saveData.blockRotationX;
            LevelBuildAndPlayManager.Instance.blockRotationY = saveData.blockRotationY;
            LevelBuildAndPlayManager.Instance.blockRotationZ = saveData.blockRotationZ;
            
            //special blocks
            LevelBuildAndPlayManager.Instance.specialBlockIds = saveData.specialBlockIds;
            LevelBuildAndPlayManager.Instance.specialGridId = saveData.specialBlockGridIds;
            //special block rotation
             LevelBuildAndPlayManager.Instance.specialRotationX = saveData.specialRotationX;
             LevelBuildAndPlayManager.Instance.specialRotationY = saveData.specialRotationY;
             LevelBuildAndPlayManager.Instance.specialRotationZ = saveData.specialRotationZ;
            
            //misc tools
            LevelBuildAndPlayManager.Instance.monsterPathPosId = saveData.monsterPath;
            LevelBuildAndPlayManager.Instance.buildArea = saveData.buildArea;
            
            

            Debug.Log("Loaded File: " + file.Name);
            file.Close();
            return true;
        }
        return false;
    }
    
    public static void SaveDemo()
    {
        // Saving method

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/DemoLevel.dat", FileMode.OpenOrCreate);
        SaveData saveData = new SaveData();
        
        //build blocks
        saveData.levelBlocksIds = LevelBuildAndPlayManager.Instance.levelBlocksIds;
        saveData.buildBlockGridIds = LevelBuildAndPlayManager.Instance.buildBlockGridIds;
        //build block rotation
        saveData.blockRotationX = LevelBuildAndPlayManager.Instance.blockRotationX;
        saveData.blockRotationY = LevelBuildAndPlayManager.Instance.blockRotationY;
        saveData.blockRotationZ = LevelBuildAndPlayManager.Instance.blockRotationZ;
        //special blocks
        saveData.specialBlockIds = LevelBuildAndPlayManager.Instance.specialBlockIds;
        saveData.specialBlockGridIds = LevelBuildAndPlayManager.Instance.specialGridId;
        //special block rotation
        saveData.specialRotationX = LevelBuildAndPlayManager.Instance.specialRotationX;
        saveData.specialRotationY = LevelBuildAndPlayManager.Instance.specialRotationY;
        saveData.specialRotationZ = LevelBuildAndPlayManager.Instance.specialRotationZ;
        
        //misc tools
        saveData.monsterPath = LevelBuildAndPlayManager.Instance.monsterPathPosId;
        saveData.buildArea = LevelBuildAndPlayManager.Instance.buildArea;
        
        

        bf.Serialize(file, saveData);
        Debug.Log("Saved File: " + file.Name);
        file.Close();
    }
    
    public static bool LoadDemo()
    {
        // Loading method

        if (File.Exists(Application.persistentDataPath + "/DemoLevel.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/DemoLevel.dat", FileMode.Open);
            SaveData saveData = (SaveData) bf.Deserialize(file);
            
            // build blocks
            LevelBuildAndPlayManager.Instance.levelBlocksIds = saveData.levelBlocksIds;
            LevelBuildAndPlayManager.Instance.buildBlockGridIds = saveData.buildBlockGridIds;
            //build block rotation 
            LevelBuildAndPlayManager.Instance.blockRotationX = saveData.blockRotationX;
            LevelBuildAndPlayManager.Instance.blockRotationY = saveData.blockRotationY;
            LevelBuildAndPlayManager.Instance.blockRotationZ = saveData.blockRotationZ;
            
            //special blocks
            LevelBuildAndPlayManager.Instance.specialBlockIds = saveData.specialBlockIds;
            LevelBuildAndPlayManager.Instance.specialGridId = saveData.specialBlockGridIds;
            //special block rotation
            LevelBuildAndPlayManager.Instance.specialRotationX = saveData.specialRotationX;
            LevelBuildAndPlayManager.Instance.specialRotationY = saveData.specialRotationY;
            LevelBuildAndPlayManager.Instance.specialRotationZ = saveData.specialRotationZ;
            
            //misc tools
            LevelBuildAndPlayManager.Instance.monsterPathPosId = saveData.monsterPath;
            LevelBuildAndPlayManager.Instance.buildArea = saveData.buildArea;
            
            

            Debug.Log("Loaded File: " + file.Name);
            file.Close();
            return true;
        }
        return false;
    }
}