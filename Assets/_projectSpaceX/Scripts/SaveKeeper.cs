using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveKeeper
{
    private int _hiScoreSavedValue;
    private readonly string _savePath;
    private bool _isRunningSaveProcess;
    
    public SaveKeeper () {
        _savePath = Application.persistentDataPath + "somethingInSpace.save";
        LoadHiScore();
    }

    public void UpdateHiScore(int score)
    {
        _hiScoreSavedValue = score;
    }

    public int GetHiScore()
    {
        return _hiScoreSavedValue;
    }
    
    public bool LoadHiScore () {
        FileStream fileStream = new FileStream(_savePath, FileMode.OpenOrCreate);

        try {
            if(fileStream.Length == 0) {
                _hiScoreSavedValue = 0;
                Debug.Log ("Created new save");
            } else {
                BinaryFormatter formatter = new BinaryFormatter ();
                _hiScoreSavedValue = (int) formatter.Deserialize (fileStream);
                Debug.Log ("Loaded hi score");
            }

        } catch (Exception ex) {
            Debug.LogError ($"Error during Loading = {ex}");
        } finally
        {
            fileStream.Close ();
        }
        return true;
    }
    
    public bool SaveHiScore (int newHiScore) {
        if (_isRunningSaveProcess) {
            Debug.Log ("Saving in progress");
            return false;
        }

        _isRunningSaveProcess = true;
        FileStream file = null;

        try {
            BinaryFormatter formatter = new BinaryFormatter ();
            file = File.Create (_savePath);

            if (newHiScore != 0) {
                formatter.Serialize (file, newHiScore);
            }

        } catch (Exception) {
            Debug.LogError ("Error during saving");
        } finally {
            file?.Close ();
            _isRunningSaveProcess = false;
            Debug.Log ($"Saved data box, path = {_savePath}");
        }

        return true;
    }
}
