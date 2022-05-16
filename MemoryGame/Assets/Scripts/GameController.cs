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

    //Sayaç ile ilgili olanlar
    public TextMeshProUGUI Counter;
    public float TotalTime;
    float Minute;
    float Second;
    bool Timer;

    // Eþleþmeler ile ilgili olanlar
    public int TargetSuccess; // Bu ikiliyi oyununu kazanýp kazanmadýgýný anlamak için hedef ve anlýk dogrularý saydýrcaz.
    int CurrentSuccess; // Bu ikiliyi oyununu kazanýp kazanmadýgýný anlamak için hedef ve anlýk dogrularý saydýrcaz.


    //Paneller
    public GameObject[] EndGamePanels;

    //Zaman
    public Slider TimeSlider;
    float ElapsedTime; // Oyun basladiktan itibaren geçen zaman

    //Grid islemleri
    public GameObject Grid;
    public GameObject ButtonPool;
    bool CreateSituation;
    int NumberOfCreate;
    int TotalButtons; // Toplam buton, sembol sayýsý

    void Start()
    {
        chosenNumber = 0;

        Timer = true;

        ElapsedTime = 0;

        TimeSlider.value = ElapsedTime; // hata olmasýn diye burada sýfýrladýk
        TimeSlider.maxValue = TotalTime; // bu satýr sayesinde slider ile level için verdiðimiz süreleri eþitledik, hata olmasýn diye

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

            if(TimeSlider.maxValue== TimeSlider.value) // süre bittiyse
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
            if(item != null) // dogru buldugumuz zaman objeler siliniyor, bunlarý dikkate almamak için bu kodu yazdýk
            {
                item.GetComponent<Image>().raycastTarget = situation;

            }


        }
    }


    IEnumerator Check(int incomingValue)
    {
        SituationButtons(false); // buton durumunu false yaptýk
        yield return new WaitForSeconds(1); //belli bir saniyede iþlemi yapmak için

        if (chosenNumber == incomingValue)
        {

            CurrentSuccess++; // dogru bildikce artar

            chosenButton.GetComponent<Image>().enabled = false;
           // chosenButton.GetComponent<Button>().enabled = false;

            orjinalButton.GetComponent<Image>().enabled = false;
           // orjinalButton.GetComponent<Button>().enabled = false;

           
            chosenNumber = 0;
            chosenButton = null;

            SituationButtons(true); // butonlarýn durumunu tekrar aktif yaptýk


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

            SituationButtons(true); // butonlarýn durumunu tekrar aktif yaptýk

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

        Time.timeScale = 0; //  Oyun zamanýný durdurur.

    }



    // SAHNE GECÝSLERÝ ÝCÝN BUTONLAR
    public void MainMenu() // Menü sahnesine git
    {
        SceneManager.LoadScene(0);
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Ayný sahneyi geri çaðýrýr

    } // Ayný leveli tekrar et
    public void NextLevel() // 2. levele geç
    {
        SceneManager.LoadScene(2);

        Time.timeScale = 1; //  Oyun zamanýný aktifleþtirir.
    }
    public void NextLevel2() // 3. levele geç
    {
        SceneManager.LoadScene(3); // 3.level geç

        Time.timeScale = 1; //  Oyun zamanýný aktifleþtirir.
    }



    //PAUSE ÝSLEMLERÝ
    public void GameStop()
    {
        EndGamePanels[2].SetActive(true);

        Time.timeScale = 0; //  Oyun zamanýný durdurur.

    }

    public void GameContinue()
    {
        EndGamePanels[2].SetActive(false);

        Time.timeScale = 1; //  Oyun zamanýný aktifleþtirir.

    }



    //GRID Islemleri havuzdan buttonlarý gride cekmek
    IEnumerator Create()
    {
        yield return new WaitForSeconds(.1f);

        while(CreateSituation)
        {
            //Random sayý olusturup bu sayýyla havuz icinden butonu alýp grid'e cocuk obje olarak atadým
            int randomNumber = Random.Range(0, ButtonPool.transform.childCount - 1); // indis numarasý olayýndan -1 yazdýk

            if(ButtonPool.transform.GetChild(randomNumber).gameObject != null) //indis numarasý gelen obje varsa, yoksa hata alýrýz zaten
            {
                ButtonPool.transform.GetChild(randomNumber).transform.SetParent(Grid.transform);
                NumberOfCreate++;

                if(NumberOfCreate == TotalButtons) // 36 tane sembol,buton var 
                {
                    CreateSituation = false; // Olusturma islemi bitmiþtir false yaptýk
                    Destroy(ButtonPool); // Atama iþlemleri bittikten sonra havuzu kaldýrdýk
                }

            }



        }
    }

}
