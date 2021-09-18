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
    }

    public void UpdatePlayerColor(Color newColor)
    {
        _saveParameters.playerColor = newColor;
    }

    public int GetHiScore()
    {
        return _saveParameters.hiScore;
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
                _saveParameters.playerColor = Color.green;
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
        public Color playerColor;
    }
}