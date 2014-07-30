using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SurveyList
/// </summary>
public class SurveyListFromGizmo
{
    public SurveyListFromGizmo()
	{
	}

    private string _SurveyName;
    private DateTime _CreatedDate;
    private string _status;
    private long _Sid;
    private int _Partial;
    private int _Disqualified;
    private int _Deleted;
    private int _Complete;
    private int _TotalPages;

    public String SurveyName
    {
        get { return this._SurveyName; }
        set { this._SurveyName = value; }
    }

    public DateTime CreatedDate
    {
        get { return this._CreatedDate; }
        set { this._CreatedDate = value; }
    }

    public String Status
    {
        get { return this._status; }
        set { this._status = value; }
    }

    public int Partial
    {
        get { return this._Partial; }
        set { this._Partial = value; }
    }

    public int Complete
    {
        get { return this._Complete; }
        set { this._Complete = value; }
    }

    public int Disqualified
    {
        get { return this._Disqualified; }
        set { this._Disqualified = value; }
    }

    public int Deleted
    {
        get { return this._Deleted; }
        set { this._Deleted = value; }
    }

    public long Sid
    {
        get { return this._Sid; }
        set { this._Sid = value; }
    }

    public int TotalPages
    {
        get { return this._TotalPages; }
        set { this._TotalPages = value; }
    }
}