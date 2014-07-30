using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


/// <summary>
/// Summary description for CustomPageChangeArgs
/// </summary>
public partial class CustomPageChangeArgs : EventArgs
{
    private int _currentPageNumber;
    public int CurrentPageNumber
    {
        get { return _currentPageNumber; }
        set { _currentPageNumber = value; }
    }

    private int _totalPages;
    public int TotalPages
    {
        get { return _totalPages; }
        set { _totalPages = value; }
    }

    private int _currentPageSize;
    public int CurrentPageSize
    {
        get { return _currentPageSize; }
        set { _currentPageSize = value; }
    }
}
