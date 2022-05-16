using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSound : MonoBehaviour
{

    private static GameObject instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Objenin sahneler aras� kaybolmamas� icin bu komut var

        if (instance == null) // Yukar�da yapt�g�m�z i�lemin hatas�n� �nledik ayn� objeden �st �ste olusturmas�n
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
