using System.Collections.Generic;
using System.Linq;
using APISystem;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [FormerlySerializedAs("_apiController")] [SerializeField] private APIController apiController;
        [FormerlySerializedAs("SpawnedUnits")] public List<GameObject> spawnedUnits;

        public async void Load()
        {
            Debug.Log("Loading Assets, please wait...");
            if (apiController.GetAssetURL() != null)
            {
                var gltf = new GLTFast.GltfImport();
                var success = await gltf.Load(apiController.GetAssetURL());

                if (success)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        var spawnedObject = new GameObject();
                        spawnedObject.transform.position = new Vector3((i % 4) * 3, 0, (int)(i / 4) * 2);
                        var rngX = Random.Range(0f, 1f);
                        var rngZ = Random.Range(0f, 1f);
                        spawnedObject.transform.forward = new Vector3(rngX, 0, rngZ);
                        await gltf.InstantiateMainSceneAsync(spawnedObject.transform);
                        spawnedUnits.Add(spawnedObject);
                        var spawnedObjectAnimation = spawnedObject.GetComponent<Animation>();
                        List<AnimationState> clips = new List<AnimationState>();
                        foreach (AnimationState clip in spawnedObjectAnimation)
                        {
                            clips.Add(clip);
                        }

                        spawnedObjectAnimation.Play(clips.ElementAt(i).name);

                    }
                    Debug.Log("Assets loading finished!");
                }
                else
                    Debug.LogError("Loading glTF failed!");
            }
            else
                Debug.LogError("Failed to get Asset URL");
        }

    }
}