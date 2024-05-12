using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Transform spawnTransform;

    [SerializeField] private GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(enemy, transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            Instantiate(enemy, transform);
        }
    }
}
