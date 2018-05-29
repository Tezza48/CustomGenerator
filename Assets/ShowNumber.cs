using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowNumber : MonoBehaviour {

	public void SetNumber(Slider slider)
    {
        GetComponent<Text>().text = slider.value.ToString("N0");
    }
}
