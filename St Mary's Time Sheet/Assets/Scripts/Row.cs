using System;
using UnityEngine;
using UnityEditor;
using TMPro;

public enum LocationType
{
	None,
	Home,
	HomeOrgan,
	HomeVoice,
	HomeMentoring,
	StMarys,
	StMarysOrgan,
	VoiceStudio,
	SacredHeart,
	ParentsHouse,
	StMarysFuneral,
	OrganLesson,
	WeddingRehearsal,
	Wedding,
	MemorialMass,
	ImmaculateConception,
	ChristmasEve,
	Christmas,
	MaryMotherOfGod,
	HolyThursday,
	GoodFriday,
	EasterVigil,
	Sick,
	SacredHeartFuneral,
	AllSaintsDay,
	Vacation,
	Sizzler
}

[Serializable]
public class PracticeSession
{
	public LocationType Location;
	public int StartingHour;
	public int StartingMinute;
	public bool StartingAM;
	public int EndingHour;
	public int EndingMinute;
	public bool EndingAM;
}

[CustomEditor(typeof(Row))]
public class RowEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		Row rowData = (Row)target;
		rowData.PopulatePracticeData();
	}
}

public class Row : MonoBehaviour
{
	public bool OverrideDate;
	public PracticeSession[] Sessions;

	public TextMeshProUGUI Day;
	public TextMeshProUGUI Date;
	public TextMeshProUGUI On1;
	public TextMeshProUGUI Off1;
	public TextMeshProUGUI On2;
	public TextMeshProUGUI Off2;
	public TextMeshProUGUI Location;
	public TextMeshProUGUI Hours;

	public void PopulatePracticeData()
	{
		PracticeData practiceData = FindObjectOfType<PracticeData>();
		practiceData.Populate();
	}

	public void Populate()
	{
		PopulateDate();
		Hours.text = GetHoursPracticed().ToString();
		PopulateSession(Sessions[0], On1, Off1);

		if (Sessions[1].Location == LocationType.None)
		{
			On2.text = "";
			Off2.text = "";
			Location.text = GetLocationText(Sessions[0].Location);
		}
		else
		{
			PopulateSession(Sessions[1], On2, Off2);
			Location.text = GetLocationText(Sessions[0].Location) + " / " + GetLocationText(Sessions[1].Location);
		}

		EditorUtility.SetDirty(this);
	}

    public void PopulateDate()
	{
		if (OverrideDate) return;
		Day.text = PracticeData.Date.DayOfWeek.ToString();
		Date.text = PracticeData.Date.ToShortDateString();
	}

	private void PopulateSession(PracticeSession session, TextMeshProUGUI on, TextMeshProUGUI off)
	{
		string am = session.StartingAM ? "am" : "pm";
		string minutes = session.StartingMinute.ToString("00");
		on.text = session.StartingHour.ToString() + ":" + minutes + am;
		am = session.EndingAM ? "am" : "pm";
		minutes = session.EndingMinute.ToString("00");
		off.text = session.EndingHour.ToString() + ":" + minutes + am;
	}

	public float GetHoursPracticed()
	{
		float hoursPracticed = Sessions[1].Location == LocationType.None ? GetHoursForSession(Sessions[0]) : GetHoursForSession(Sessions[0]) + GetHoursForSession(Sessions[1]);
		return hoursPracticed;
	}

	private float GetHoursForSession(PracticeSession session)
	{
		if (OverrideDate) return 0;
		DateTime dateTimeStart = session.StartingAM == true ? new DateTime(1990, 7, 11, session.StartingHour, session.StartingMinute, 0) : new DateTime(1990, 7, 11, session.StartingHour + 12, session.StartingMinute, 0);
		DateTime dateTimeEnd = session.EndingAM == true ? new DateTime(1990, 7, 11, session.EndingHour, session.EndingMinute, 0) : new DateTime(1990, 7, 11, session.EndingHour + 12, session.EndingMinute, 0);
		TimeSpan timeSpan = dateTimeEnd - dateTimeStart;
		return (float)timeSpan.TotalHours;
	}

	private string GetLocationText(LocationType location)
	{
		switch (location)
		{
			case LocationType.Home:
				return "Home";
			case LocationType.HomeOrgan:
				return "Home (Organ Lesson)";
			case LocationType.HomeVoice:
				return "Home (Voice Lesson)";
			case LocationType.HomeMentoring:
				return "Home (Mentoring)";
			case LocationType.StMarys:
				return "St. Mary's";
			case LocationType.StMarysOrgan:
				return "St. Mary's (Organ Lesson)";
			case LocationType.VoiceStudio:
				return "Voice Studio";
			case LocationType.SacredHeart:
				return "Sacred Heart";
			case LocationType.ParentsHouse:
				return "Parents' House";
			case LocationType.StMarysFuneral:
				return "St. Mary's Funeral";
			case LocationType.WeddingRehearsal:
				return "Wedding Rehearsal";
			case LocationType.Wedding:
				return "Wedding";
			case LocationType.OrganLesson:
				return "Organ Lesson";
			case LocationType.MemorialMass:
				return "Memorial Mass";
			case LocationType.ImmaculateConception:
				return "Immaculate Conception";
			case LocationType.ChristmasEve:
				return "Christmas Eve";
			case LocationType.Christmas:
				return "Christmas";
			case LocationType.MaryMotherOfGod:
				return "Mary, Mother of God";
			case LocationType.HolyThursday:
				return "Holy Thursday";
			case LocationType.GoodFriday:
				return "Good Friday";
			case LocationType.EasterVigil:
				return "Easter Vigil";
			case LocationType.Sick:
				return "SICK DAY";
			case LocationType.SacredHeartFuneral:
				return "Sacred Heart Funeral";
			case LocationType.Vacation:
				return "Vacation";
			case LocationType.Sizzler:
				return "Sizzler";
			case LocationType.AllSaintsDay:
				return "All Saints Day";
			default:
				return "";
		}
	}
}