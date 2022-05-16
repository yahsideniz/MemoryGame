using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    int chosenNumber;
    //----------------------------------
    GameObject chosenButton;
    GameObject orjinalButton;
    //----------------------------------
    public Sprite defaultSprite;
    public AudioSource[] sounds;
    public GameObject[] Buttons;
    //----------------------------------

    //Saya� ile ilgili olanlar
    public TextMeshProUGUI Counter;
    public float TotalTime;
    float Minute;
    float Second;
    bool Timer;

    // E�le�meler ile ilgili olanlar
    public int TargetSuccess; // Bu ikiliyi oyununu kazan�p kazanmad�g�n� anlamak i�in hedef ve anl�k dogrular� sayd�rcaz.
    int CurrentSuccess; // Bu ikiliyi oyununu kazan�p kazanmad�g�n� anlamak i�in hedef ve anl�k dogrular� sayd�rcaz.


    //Paneller
    public GameObject[] EndGamePanels;

    //Zaman
    public Slider TimeSlider;
    float ElapsedTime; // Oyun basladiktan itibaren ge�en zaman

    //Grid islemleri
    public GameObject Grid;
    public GameObject ButtonPool;
    bool CreateSituation;
    int NumberOfCreate;
    int TotalButtons; // Toplam buton, sembol say�s�

    void Start()
    {
        chosenNumber = 0;

        Timer = true;

        ElapsedTime = 0;

        TimeSlider.value = ElapsedTime; // hata olmas�n diye burada s�f�rlad�k
        TimeSlider.maxValue = TotalTime; // bu sat�r sayesinde slider ile level i�in verdi�imiz s�releri e�itledik, hata olmas�n diye

        // Grid islemleri
        CreateSituation = true;
        NumberOfCreate = 0;
        TotalButtons = ButtonPool.transform.childCount;


        StartCoroutine(Create());

    }

    void Update()
    {
        if (Timer && ElapsedTime!=TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            TimeSlider.value = ElapsedTime;

            if(TimeSlider.maxValue== TimeSlider.value) // s�re bittiyse
            {
                Timer = false;
                GameOver();
            }

        }
   
       
    }

    public void ClickedButton(int value)
    {
        Control(value);
    }

    public void GiveObject(GameObject myObject)
    {
        orjinalButton = myObject;

        orjinalButton.GetComponent<Image>().sprite = orjinalButton.GetComponentInChildren<SpriteRenderer>().sprite;
        orjinalButton.GetComponent<Image>().raycastTarget = false;


        sounds[0].Play();
    }

    void Control(int incomingValue)
    {
        if (chosenNumber == 0)
        {
            chosenNumber = incomingValue;
            chosenButton = orjinalButton;
        }
        else
        {
            StartCoroutine(Check(incomingValue));
        }
    }

    void SituationButtons(bool situation)
    {
        foreach (var item in Buttons)
        {
            if(item != null) // dogru buldugumuz zaman objeler siliniyor, bunlar� dikkate almamak i�in bu kodu yazd�k
            {
                item.GetComponent<Image>().raycastTarget = situation;

            }


        }
    }


    IEnumerator Check(int incomingValue)
    {
        SituationButtons(false); // buton durumunu false yapt�k
        yield return new WaitForSeconds(1); //belli bir saniyede i�lemi yapmak i�in

        if (chosenNumber == incomingValue)
        {

            CurrentSuccess++; // dogru bildikce artar

            chosenButton.GetComponent<Image>().enabled = false;
           // chosenButton.GetComponent<Button>().enabled = false;

            orjinalButton.GetComponent<Image>().enabled = false;
           // orjinalButton.GetComponent<Button>().enabled = false;

           
            chosenNumber = 0;
            chosenButton = null;

            SituationButtons(true); // butonlar�n durumunu tekrar aktif yapt�k


            if(TargetSuccess==CurrentSuccess)
            {
                Win();
            }

        }
        else
        {
            sounds[1].Play();

            chosenButton.GetComponent<Image>().sprite = defaultSprite;
            orjinalButton.GetComponent<Image>().sprite = defaultSprite; 

            chosenNumber = 0;
            chosenButton = null;

            SituationButtons(true); // butonlar�n durumunu tekrar aktif yapt�k

        }
    }


    //PANELLER
    void GameOver() // kaybetme paneli
    {
        EndGamePanels[0].SetActive(true);
    }
    
    void Win() // kazanma paneli
    {
        EndGamePanels[1].SetActive(true);

        Time.timeScale = 0; //  Oyun zaman�n� durdurur.

    }



    // SAHNE GEC�SLER� �C�N BUTONLAR
    public void MainMenu() // Men� sahnesine git
    {
        SceneManager.LoadScene(0);
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Ayn� sahneyi geri �a��r�r

    } // Ayn� leveli tekrar et
    public void NextLevel() // 2. levele ge�
    {
        SceneManager.LoadScene(2);

        Time.timeScale = 1; //  Oyun zaman�n� aktifle�tirir.
    }
    public void NextLevel2() // 3. levele ge�
    {
        SceneManager.LoadScene(3); // 3.level ge�

        Time.timeScale = 1; //  Oyun zaman�n� aktifle�tirir.
    }



    //PAUSE �SLEMLER�
    public void GameStop()
    {
        EndGamePanels[2].SetActive(true);

        Time.timeScale = 0; //  Oyun zaman�n� durdurur.

    }

    public void GameContinue()
    {
        EndGamePanels[2].SetActive(false);

        Time.timeScale = 1; //  Oyun zaman�n� aktifle�tirir.

    }



    //GRID Islemleri havuzdan buttonlar� gride cekmek
    IEnumerator Create()
    {
        yield return new WaitForSeconds(.1f);

        while(CreateSituation)
        {
            //Random say� olusturup bu say�yla havuz icinden butonu al�p grid'e cocuk obje olarak atad�m
            int randomNumber = Random.Range(0, ButtonPool.transform.childCount - 1); // indis numaras� olay�ndan -1 yazd�k

            if(ButtonPool.transform.GetChild(randomNumber).gameObject != null) //indis numaras� gelen obje varsa, yoksa hata al�r�z zaten
            {
                ButtonPool.transform.GetChild(randomNumber).transform.SetParent(Grid.transform);
                NumberOfCreate++;

                if(NumberOfCreate == TotalButtons) // 36 tane sembol,buton var 
                {
                    CreateSituation = false; // Olusturma islemi bitmi�tir false yapt�k
                    Destroy(ButtonPool); // Atama i�lemleri bittikten sonra havuzu kald�rd�k
                }

            }



        }
    }

}
