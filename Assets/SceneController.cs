using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public const int gridRows = 2;
    public const int gridColumns = 4;
    public const float offsetX = 3f;
    public const float offsetY = 4f;

    private MemoryCard firstRevealed;
    private MemoryCard secondRevealed;

    private static int score = 0;
    private static int numOfMisclicks = 0;

    [SerializeField] MemoryCard originalCard;
    [SerializeField] Sprite[] images;
    [SerializeField] TMP_Text scoreLabel;
    [SerializeField] Background originalBackground;
    [SerializeField] Sprite[] backgrounds;
    [SerializeField] TMP_Text misclicks;

    // Start is called before the first frame update
    void Start()
    {
        ChangeDisplay();
        SetBackground();
        Vector3 startPos = originalCard.transform.position;

        //int[] numberOfBackgrounds = { 0, 1, 2, 3, 4, 5, 6 }; //a simple array to decide how many backgrounds to use
        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 }; //declaring integer array to shuffle elements of array
        numbers = ShuffleArray(numbers);    //calling the function that will shuffle the array

        for(int i = 0; i <gridColumns; i++)
        {
            for(int j = 0; j <gridRows; j++)
            {
                MemoryCard card;
                if(i == 0 & j ==0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                //Now we retrieve IDs from the shuffled list.
                int index = j * gridColumns + i;
                int id = numbers[index];
                card.SetCard(id, images[id]);


                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
        
    }

    //Method for shuffling the array using the Knuth Shuffle Algorithm
    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for(int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }
    public bool canReveal
    {
        get { return secondRevealed == null; }
    }

    public void CardRevealed(MemoryCard card)
    {
        if (firstRevealed == null)
        {
            firstRevealed = card;
        }
        else
        {
            secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }
    private IEnumerator CheckMatch()
    {
        if(firstRevealed.ID == secondRevealed.ID)
        {
            score++;
            scoreLabel.text = "Score: " + score;
        }
        else { 
            yield return new WaitForSeconds(.5f);
            firstRevealed.Unreveal();
            secondRevealed.Unreveal();
            numOfMisclicks++;
            misclicks.text = "Misclicks: " + numOfMisclicks;
        }
        firstRevealed=null;
        secondRevealed=null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Reset() { 
        score = 0;
        numOfMisclicks = 0;
        ChangeDisplay();
    }

    public void ChangeDisplay()
    {
        scoreLabel.text = "Score: " + score;
        misclicks.text = "Misclicks: " + numOfMisclicks;
    }

    public void SetBackground()
    {
        Background background = originalBackground;
        var rnd = new System.Random();
        background.SetBackground(backgrounds[rnd.Next(6)]);
    }
}
