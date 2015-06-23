using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
	
	private Slider slider;
	//private int counter;
	
	public int MaxProgress = 0;
	public Image Fill;  // assign in the editor the "Fill"
	public Color MaxHealthColor = Color.red;
	public Color MinHealthColor = Color.green;
	
	private void Awake() {
		slider = gameObject.GetComponent<Slider>();
	}
	
	private void Start() {
		slider.wholeNumbers = true;
		slider.minValue = 0f;
		slider.maxValue = 100f;
		slider.value = 0;
	}
	
	public void UpdateProgressBar(int val) {
		slider.value = (float)val / (float)MaxProgress * 100;
        Fill.color = Color.Lerp(MinHealthColor, MaxHealthColor, (float)val / MaxProgress);
	}

    public void SetMaxAwnsers(int totalAwnsers) { MaxProgress = totalAwnsers;}
}

