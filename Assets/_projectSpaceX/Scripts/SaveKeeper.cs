using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveKeeper
{
    private SaveDataParameters _saveParameters;
    private readonly string _savePath;
    private bool _isRunningSaveProcess;

    public SaveKeeper()
    {
        _savePath = Application.persistentDataPath + "/save";
        LoadSavedParameters();
    }

    public void UpdateHiScore(int score)
    {
        _saveParameters.hiScore = score;
        SaveParameters();
    }

    public void UpdatePlayerColor(Color newColor)
    {

        ColorValues colorValues = new ColorValues
        {
            r = newColor.r,
            g = newColor.g,
            b = newColor.b, 
            a = newColor.a
        };
        _saveParameters.playerColor = colorValues;
        SaveParameters();
    }

    public int GetHiScore()
    {
        return _saveParameters.hiScore;
    }
    public Color GetPlayerColor()
    {
        var savedColor = _saveParameters.playerColor;
        var playerColor = new Color(savedColor.r, savedColor.g, savedColor.b, savedColor.a);
        return playerColor;
    }

    public SaveDataParameters GetSaveParams()
    {
        return _saveParameters;
    }

    public void LoadSavedParameters()
    {
        FileStream fileStream = new FileStream(_savePath, FileMode.OpenOrCreate);

        try
        {
            if (fileStream.Length == 0)
            {
                _saveParameters = new SaveDataParameters();
                ColorValues values = new ColorValues
                {
                    r = Color.green.r,
                    g = Color.green.g,
                    b = Color.green.b, 
                    a = Color.green.a
                };
                _saveParameters.playerColor = values;
                Debug.Log("Created new save");
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                _saveParameters = (SaveDataParameters)formatter.Deserialize(fileStream);
                Debug.Log("Loaded hi score");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during Loading = {ex}");
        }
        finally
        {
            fileStream.Close();
        }
    }

    public void SaveParameters()
    {
        if (_isRunningSaveProcess)
        {
            Debug.Log("Saving in progress");
            return;
        }

        _isRunningSaveProcess = true;
        FileStream file = null;

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            file = File.Create(_savePath);

            if (_saveParameters != null)
            {
                SaveDataParameters newSave = new SaveDataParameters
                {
                    hiScore = _saveParameters.hiScore,
                    playerColor = _saveParameters.playerColor
                };
                formatter.Serialize(file, newSave);
            }
        }
        catch (Exception)
        {
            Debug.LogError("Error during saving");
        }
        finally
        {
            file?.Close();
            _isRunningSaveProcess = false;
            Debug.Log($"Saved data box, path = {_savePath}");
        }
    }

    [Serializable]
    public class SaveDataParameters
    {
        public int hiScore;
        public ColorValues playerColor;
    }

    [Serializable]
    public class ColorValues
    {
        public float r;
        public float g;
        public float b;
        public float a;
    }
}