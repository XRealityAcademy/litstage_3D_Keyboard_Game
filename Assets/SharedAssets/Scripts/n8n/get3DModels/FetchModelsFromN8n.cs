using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Siccity.GLTFUtility;

[System.Serializable]
public class Model
{
    public string name;
    public string modelUrl;
}

[System.Serializable]
public class ModelsList
{
    public List<Model> models;
}

public class FetchModelsFromN8n : MonoBehaviour
{
    public string endpoint = "http://localhost:5678/webhook/get-3d-models";
    private const string EVENT_NAME = "FETCH_3D_MODELS";
    private HashSet<string> loadedModelNames = new HashSet<string>();
    private Coroutine fetchLoop;

    private Vector3 basePosition = new Vector3(-10.81163f, 4.06f, 4.87f);
    private float xOffsetPerModel = 4f;

    void OnEnable()
    {
        EventManager.Subscribe(EVENT_NAME, OnFetchTriggered);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(EVENT_NAME, OnFetchTriggered);
    }

    void OnFetchTriggered()
    {
        if (fetchLoop == null)
        {
            fetchLoop = StartCoroutine(FetchLoop());
        }
    }

    IEnumerator FetchLoop()
    {
        while (true)
        {
            yield return GetModels();
            yield return new WaitForSeconds(30f);
        }
    }

    IEnumerator GetModels()
    {
        using UnityWebRequest request = UnityWebRequest.Get(endpoint);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            ModelsList modelsList = JsonUtility.FromJson<ModelsList>(jsonResponse);

            for (int i = 0; i < modelsList.models.Count; i++)
            {
                var model = modelsList.models[i];
                if (!loadedModelNames.Contains(model.name))
                {
                    Vector3 position = basePosition + new Vector3(i * xOffsetPerModel, 0f, 0f);
                    StartCoroutine(DownloadAndImportModel(model, position));
                }
            }
        }
        else
        {
            Debug.LogError($"❌ UnityWebRequest failed: {request.error}");
        }
    }

    IEnumerator DownloadAndImportModel(Model model, Vector3 position)
    {
        using UnityWebRequest req = UnityWebRequest.Get(model.modelUrl);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            byte[] glbData = req.downloadHandler.data;
            GameObject loadedModel = Importer.LoadFromBytes(glbData);
            loadedModel.name = model.name;
            loadedModel.transform.position = position;
            loadedModel.transform.localScale = Vector3.one * 2f;

            loadedModelNames.Add(model.name);

            Debug.Log($"✅ Loaded and placed: {model.name}");
        }
        else
        {
            Debug.LogError($"❌ Could not download model: {model.name}");
        }
    }
}