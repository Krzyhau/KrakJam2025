using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Monke.KrakJam2025
{
    public class GameOverSystem : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] legModels;

        private bool ending = false;

        public void StartGameOver()
        {
            if(ending)
            {
                return;
            }
            ending = true;

            EndingTask().Forget();
        }

        private async UniTaskVoid EndingTask()
        {
            foreach(var model in legModels)
            {
                model.material.DOFloat(100f, "_DisplacementSpeed", 1);
                model.material.DOFloat(0.2f, "_DisplacementScale", 1);
                model.material.DOVector(new Vector3(2, 2, 2), "_DisplacementForce", 1);
            }

            await UniTask.Delay(1000);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}