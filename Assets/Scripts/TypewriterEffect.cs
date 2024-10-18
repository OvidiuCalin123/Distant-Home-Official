using UnityEngine;
using System.Collections;
using TMPro;

public class TypewriterEffect: MonoBehaviour
{

	public TextMeshProUGUI txt1;
	public TextMeshProUGUI txt2;

	string story;
	string story2;

	public TextMeshProUGUI storedText1;
	public TextMeshProUGUI storedText2;

	public bool nextDialogue;

    private void OnDisable()
    {
		txt1.text = "";
		txt2.text = "";
	}

    private void OnEnable()
    {
		txt1.text = "";
		txt2.text = "";

	}

    public IEnumerator PlayText()
	{
		foreach (char c in story)
		{
            if (nextDialogue)
            {
				txt1.text = "";
				break;
            }
			txt1.text += c;
			yield return new WaitForSeconds(0.04f);
		}

		foreach (char c in story2)
		{
			if (nextDialogue)
            {
				txt2.text = "";
				break;
			}
			txt2.text += c;
			yield return new WaitForSeconds(0.04f);
		}

		nextDialogue = false;
	}

	public IEnumerator PlayText_(string story1_, string story2_)
	{

		txt1.text = "";
		txt2.text = "";

		foreach (char c in story1_)
		{
			txt1.text += c;
			yield return new WaitForSeconds(0.04f);
		}

		foreach (char c in story2_)
		{
			txt2.text += c;
			yield return new WaitForSeconds(0.04f);
		}

		nextDialogue = false;
	}

    private void Update()
    {

    }

}