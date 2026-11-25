using UnityEngine;
using UnityEngine.UI;

//choose the word that is from a different group
public class OddWordGame : Minigame
{
    //groups of words: animals, fruits, colours and furniture
    private string[][] groups =
    {
        new string[] { "perro", "gato", "caballo", "vaca", "pato", "oveja" },
        new string[] { "manzana", "pera", "plátano", "uva", "fresa", "kiwi" },
        new string[] { "rojo", "azul", "verde", "amarillo", "rosa", "negro" },
        new string[] { "mesa", "silla", "sofá", "cama", "armario", "lámpara" }
    };

    protected override void Create()
    {
        title = "¿Qué palabra no encaja?";
        timeLimit = 5f;

        //we choose a group and a different group for the odd word
        int group = Random.Range(0, groups.Length);
        int otherGroup = (group + Random.Range(1, groups.Length)) % groups.Length;

        //two different words of the first group and one word of the other group
        int first = Random.Range(0, 6);
        int second = (first + Random.Range(1, 6)) % 6;
        string[] words = { groups[group][first], groups[group][second] };
        string oddWord = groups[otherGroup][Random.Range(0, 6)];

        int oddPosition = Random.Range(0, 3);
        int wordIndex = 0;

        for (int i = 0; i < 3; i++)
        {
            Vector2 position = new Vector2(-210f + 210f * i, -20f);

            if (i == oddPosition)
            {
                Button button = CreateButton(oddWord, new Vector2(190f, 80f), position, buttonColor);
                button.onClick.AddListener(Win);
            }
            else
            {
                Button button = CreateButton(words[wordIndex], new Vector2(190f, 80f), position, buttonColor);
                button.onClick.AddListener(Lose);
                wordIndex++;
            }
        }
    }
}
