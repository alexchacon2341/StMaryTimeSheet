using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PracticeData))]
public class PracticeDataEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		PracticeData practiceData = (PracticeData)target;
		practiceData.Populate();
	}
}

public class PracticeData : MonoBehaviour
{
	public static DateTime Date;

	public float TotalHoursPracticed;
	public int StartingMonth;
	public int StartingDay;
	public int StartingYear;

	public void OnGUI()
	{
		Populate();
	}

	public void Populate()
	{
		float totalHours = 0;
		float week1Hours = 0;
		float week2Hours = 0;
		Row[] rows = GetComponentsInChildren<Row>();
		Date = new DateTime(StartingYear, StartingMonth, StartingDay);

		for (int i = 0; i < rows.Length; i++)
		{
			rows[i].Populate();
			Date = Date.AddDays(1);
			float hoursPracticed = rows[i].GetHoursPracticed();
			totalHours += hoursPracticed;

			if (i < 7)
			{
				week1Hours += hoursPracticed;
			}
			else
			{
				week2Hours += hoursPracticed;
			}
		}

		Debug.Log("Total Hours: " + totalHours + " | Week 1 Hours: " + week1Hours + " | Week 2 Hours: " + week2Hours);
		EditorUtility.SetDirty(this);
	}
}