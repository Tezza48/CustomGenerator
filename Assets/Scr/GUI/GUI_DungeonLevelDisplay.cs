using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUI_DungeonLevelDisplay : MonoBehaviour
{
    
    public Generator Generator;

    private Text _Text;

    void Start ()
    {
        _Text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        _Text.text = "Level: " + Generator.Level.ToString();
	}
}
