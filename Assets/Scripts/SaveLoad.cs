﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveLoad
{
    public static void Save() {     // Saving method

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.OpenOrCreate);
        SaveData saveData = new SaveData();

        //prefab id
        saveData.levelBlocksIds = LevelBuilderManager.Instance.levelBLocksIds;
        //position
        /*saveData.levelBlocksXAxis = LevelBuilderManager.Instance.levelBlocksXAxis;
        saveData.levelBlocksYAxis = LevelBuilderManager.Instance.levelBlocksYAxis;
        saveData.levelBlocksZAxis = LevelBuilderManager.Instance.levelBlocksZAxis;*/
        saveData.gridIds = LevelBuilderManager.Instance.gridIds;
        /*saveData.lvl = GameManager.instance.lvl;
        saveData.scores = GameManager.instance.scores;*/

        bf.Serialize(file, saveData);
        Debug.Log("Saved File: " + file.Name);
        file.Close();
    }

    public static bool Load() {     // Loading method

        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveData saveData = (SaveData)bf.Deserialize(file);

            //prefab id
            LevelBuilderManager.Instance.levelBLocksIds = saveData.levelBlocksIds;
            //position
            /*LevelBuilderManager.Instance.levelBlocksXAxis = saveData.levelBlocksXAxis;
            LevelBuilderManager.Instance.levelBlocksYAxis = saveData.levelBlocksYAxis;
            LevelBuilderManager.Instance.levelBlocksZAxis = saveData.levelBlocksZAxis;*/
            LevelBuilderManager.Instance.gridIds = saveData.gridIds;
            
            
            /*LevelBuilderManager.Instance.levelBlocks = saveData.levelBlocks;
            LevelBuilderManager.Instance.grdManager.levelGrid = saveData.levelGrid;*/
            /*GameManager.instance.lvl = saveData.lvl;
            GameManager.instance.scores = saveData.scores;*/

            Debug.Log("Loaded File: " + file.Name);
            file.Close();
            return true;
        }
        else {
            return false;
        }

    }

}