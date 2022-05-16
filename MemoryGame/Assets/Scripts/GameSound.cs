using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSound : MonoBehaviour
{

    private static GameObject instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Objenin sahneler arasý kaybolmamasý icin bu komut var

        if (instance == null) // Yukarýda yaptýgýmýz iþlemin hatasýný önledik ayný objeden üst üste olusturmasýn
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
