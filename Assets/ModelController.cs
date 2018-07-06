using System.Collections.Generic;
using TriLib;
using UnityEngine;
using UnityEngine.UI;

public class ModelController : MonoBehaviour {

    [SerializeField] GameObject m_platformPivot;
    [SerializeField] GameObject m_activeModel;
    [SerializeField] Camera modelViewCamera;

    [SerializeField] AssetDownloader assetDownloader;

    Dictionary<int, string> modelURI = new Dictionary<int, string>() {
        { 1, "https://young-stream-69308.herokuapp.com/models/1" },
        { 2, "https://young-stream-69308.herokuapp.com/models/2" },
        { 3, "https://young-stream-69308.herokuapp.com/models/3" },
        { 4, "https://young-stream-69308.herokuapp.com/models/4" },
        { 5, "https://young-stream-69308.herokuapp.com/models/5" }
    };

    public void SetViewModel(Dropdown dropdown)
    {
        int index = dropdown.value + 1;

        if (!modelURI.ContainsKey(index)) { Debug.LogWarning("Model with key " + index.ToString() + " does not exist."); }

        var assetLoaderOptions = AssetLoaderOptions.CreateInstance();
        assetLoaderOptions.DontLoadCameras = false;
        assetLoaderOptions.DontLoadLights = false;
        assetLoaderOptions.UseCutoutMaterials = true;
        assetLoaderOptions.AddAssetUnloader = true;

        assetDownloader.DownloadAsset(modelURI[index], ".zip", OnAssetDownloaded, null, assetLoaderOptions, m_platformPivot);
    }

    public void SetModelRotationY(Slider slider)
    {
        if (!m_activeModel) { return; }

        m_activeModel.transform.localEulerAngles = new Vector3(
                m_activeModel.transform.localEulerAngles.x,
                slider.value,
                m_activeModel.transform.localEulerAngles.z
            );
    }

    public void SetCameraFOV(Slider slider)
    {
        if (!modelViewCamera) { return; }

        modelViewCamera.fieldOfView = slider.value;
    }

    void OnAssetDownloaded(GameObject loadedGameObject)
    {
        DownloadEvents.DownloadEnd();

        if (loadedGameObject)
        {
            if (m_activeModel) { Destroy(m_activeModel); }
            m_activeModel = loadedGameObject;

            SetLayerRecursively(m_activeModel, LayerMask.NameToLayer("3D"));
        }
    }

    public void SetLayerRecursively(GameObject go, int layerNumber)
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
