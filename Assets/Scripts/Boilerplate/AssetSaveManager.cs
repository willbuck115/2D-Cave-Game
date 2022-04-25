using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class AssetSaveManager : MonoBehaviour
{
    // All savable arrays & objects

    // Classes to redirect loaded information to
    [SerializeField] private PlayerBase playerBaseClass;
    [SerializeField] private WorldGeneration worldGeneratorClass;

    private readonly string floatMapPath = "/TileMap";
    private readonly string playerSavePath = "/PlayerSaveInformation";

    [HideInInspector] public int[,] loadedTileMap = null;
    [HideInInspector] public SaveablePlayerInformation loadedPlayerInformation;

    public void Save(int[,] tileMap = null, SaveablePlayerInformation saveablePlayerInformation = null) {
        if(tileMap != null) {
            FileStream fs = File.Create(Application.persistentDataPath + "/TileMap");
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, tileMap);
            fs.Close();
        } 
        if(saveablePlayerInformation != null) {
            FileStream fs = File.Create(Application.persistentDataPath + "/PlayerSaveInformation");
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, saveablePlayerInformation);
            fs.Close();
        }
    }

    public bool InitaliseLoad(string p) {
        string path = Application.persistentDataPath + p;
        if (File.Exists(path)) {
            FileStream fs = File.Open(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            if(p == floatMapPath) {
                LoadFloatMap(fs, bf);
            } else {
                LoadPlayerInformation(fs, bf);
            }
            return true;
        } else {
            return false;
        }
    }

    private void LoadFloatMap(FileStream fs, BinaryFormatter bf) {
        loadedTileMap = (int[,])bf.Deserialize(fs);
        fs.Close();
    }
    private void LoadPlayerInformation(FileStream fs, BinaryFormatter bf) {
        loadedPlayerInformation = (SaveablePlayerInformation)bf.Deserialize(fs);
        fs.Close();
    }
}

[System.Serializable]
public class SaveablePlayerInformation {
    // public decimal bank
    public int dayCount;
    public List<InventoryItem> inventoryItems;

    // figure a way to save owned and current pickaxes
    // maybe an array of booleans?

    // save updgrades when added.
}
