using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{


    public void Restart()
    {

        SceneManager.LoadScene("SampleScene");
    }
    

    public int[] shuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++) {
            int r = Random.Range(0, newArray.Length);
            int tmp = newArray[i];
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    
    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;
    
    public bool canReveal
    {
        get { return _secondRevealed==null; }
    }

    public void Cardrevealed (MemoryCard card){
        if (_firstRevealed == null)
            _firstRevealed = card;
        else if(_secondRevealed == null)
        {
            _secondRevealed = card;         
            Debug.Log("match result:"+ (_firstRevealed.id == _secondRevealed.id ? "true" : "false"));
            StartCoroutine(checkMatch());
        }

    }




    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;
   


    private int _score = 100;
    public const int gridRows = 2;
    public const int gridCols = 4;
    public const float offsetX = 2f;
    public const float offsetY = 2.5f;






    // Start is called before the first frame update
    void Start()
    {
    int[] numbers = { 0,0,1,1, 2,2,3,3 };
        numbers = shuffleArray(numbers);
        Vector3 startPos = originalCard.transform.position; // getting world space coords
        for(int i=0 ; i < gridCols; i++)
        {
            for(int j = 0; j< gridRows; j++)
            {
                MemoryCard card;
                if (i == 0 && j == 0)
                    card = originalCard;
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;

                }

                int index = j * gridCols + i;

                int id = numbers[index];
            card.SetCard(id, images[id]);

            float posX = offsetX * i + startPos.x;
            float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }


        }

    }

    [SerializeField] private TextMesh scorelable1;

    // setting up a new thread
    private IEnumerator checkMatch()
    {
        if (_firstRevealed.id == _secondRevealed.id)
        {
           

            scorelable1.text = "score: " + _score;
            //Debug.Log("current score: " + _score);
        }
        else
        {
            _score -= 20;
            yield return new WaitForSeconds(2f);

            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
            _firstRevealed = null;
            _secondRevealed = null;
        }
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
