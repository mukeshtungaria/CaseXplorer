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
/// Summary description for CustomBoundField
/// </summary>
public class CustomBoundField : DataControlField
{
    public CustomBoundField()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public CustomBoundField(string strDataField, bool boolVisible, bool boolEditable, bool boolShowCheckBox)
    {
        this.DataField = strDataField;
        this.Visible = boolVisible;
        this.Editable = boolEditable;
        this.ShowCheckBox = boolShowCheckBox;
    }

    public CustomBoundField(string strDataField, string strHeader, bool boolVisible, bool boolEditable, bool boolShowCheckBox)
    {
        this.DataField = strDataField;
        this.HeaderText = strHeader;
        this.Visible = boolVisible;
        this.Editable = boolEditable;
        this.ShowCheckBox = boolShowCheckBox;
    }


    #region Public Properties

    /// <summary>
    /// This property describe weather the column should be an editable column or non editable column.
    /// </summary>

    public bool Editable
    {
        get
        {
            object value = base.ViewState["Editable"];

            if (value != null)
            {
                return Convert.ToBoolean(value);
            }

            else
            {
                return true;
            }
        }
        set
        {
            base.ViewState["Editable"] = value;
            this.OnFieldChanged();
        }
    }

    /// <summary>
    /// This property is to describe weather to display a check box or not. 
    /// This property works in association with Editable.
    /// </summary>
    public bool ShowCheckBox
    {
        get
        {
            object value = base.ViewState["ShowCheckBox"];

            if (value != null)
            {
                return Convert.ToBoolean(value);
            }
            else
            {
                return false;
            }
        }

        set
        {
            base.ViewState["ShowCheckBox"] = value;
            this.OnFieldChanged();
        }
    }

    /// <summary>
    /// This property describe column name, which acts as the primary data source for the column. 
    /// The data that is displayed in the column will be retreived from the given column name.
    /// </summary>
    public string DataField
    {
        get
        {
            object value = base.ViewState["DataField"];

            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        set
        {
            base.ViewState["DataField"] = value;
            this.OnFieldChanged();
        }

    }

    #endregion



    #region Overriden Life Cycle Methods

    /// <summary>

    /// Overriding the CreateField method is mandatory if you derive from the DataControlField.

    /// </summary>

    /// <returns></returns>

    protected override DataControlField CreateField()
    {

        return new BoundField();

    }



    /// <summary>

    /// Adds text controls to a cell's controls collection. Base method of DataControlField is

    /// called to import much of the logic that deals with header and footer rendering.

    /// </summary>

    /// <param name="cell">A reference to the cell</param>

    /// <param name="cellType">The type of the cell</param>

    /// <param name="rowState">State of the row being rendered</param>

    /// <param name="rowIndex">Index of the row being rendered</param>

    public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
    {

        //Call the base method.

        base.InitializeCell(cell, cellType, rowState, rowIndex);



        switch (cellType)
        {

            case DataControlCellType.DataCell:

                this.InitializeDataCell(cell, rowState);

                break;

            case DataControlCellType.Footer:

                this.InitializeFooterCell(cell, rowState);

                break;

            case DataControlCellType.Header:

                this.InitializeHeaderCell(cell, rowState);

                break;

        }

    }

    #endregion



    #region Custom Protected Methods

    /// <summary> 

    /// Determines which control to bind to data. In this a hyperlink control is bound regardless

    /// of the row state. The hyperlink control is then attached to a DataBinding event handler

    /// to actually retrieve and display data.

    /// 

    /// Note: This control was built with the assumption that it will not be used in a gridview

    /// control that uses inline editing. If you are building a custom data control field and 

    /// using this code for reference purposes key in mind that if your control needs to support

    /// inline editing you must determine which control to bind to data based on the row state.

    /// </summary>

    /// <param name="cell">A reference to the cell</param>

    /// <param name="rowState">State of the row being rendered</param>

    protected void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
    {

        //Check to see if the column is a editable and does not show the checkboxes.

        if (Editable & !ShowCheckBox)
        {

            string ID = Guid.NewGuid().ToString();

            TextBox txtBox = new TextBox();

            txtBox.Columns = 5;

            txtBox.ID = ID;

            txtBox.DataBinding += new EventHandler(txtBox_DataBinding);



            cell.Controls.Add(txtBox);

        }

        else
        {

            if (ShowCheckBox)
            {

                CheckBox chkBox = new CheckBox();

                cell.Controls.Add(chkBox);

            }

            else
            {

                Label lblText = new Label();

                lblText.DataBinding += new EventHandler(lblText_DataBinding);

                cell.Controls.Add(lblText);

            }

        }

    }



    void lblText_DataBinding(object sender, EventArgs e)
    {

        // get a reference to the control that raised the event

        Label target = (Label)sender;

        Control container = target.NamingContainer;



        // get a reference to the row object

        object dataItem = DataBinder.GetDataItem(container);



        // get the row's value for the named data field only use Eval when it is neccessary

        // to access child object values, otherwise use GetPropertyValue. GetPropertyValue

        // is faster because it does not use reflection

        object dataFieldValue = null;



        if (this.DataField.Contains("."))
        {

            dataFieldValue = DataBinder.Eval(dataItem, this.DataField);

        }

        else
        {

            dataFieldValue = DataBinder.GetPropertyValue(dataItem, this.DataField);

        }



        // set the table cell's text. check for null values to prevent ToString errors

        if (dataFieldValue != null)
        {

            target.Text = dataFieldValue.ToString();

        }

    }



    protected void InitializeFooterCell(DataControlFieldCell cell, DataControlRowState rowState)
    {

        CheckBox chkBox = new CheckBox();

        cell.Controls.Add(chkBox);

    }



    protected void InitializeHeaderCell(DataControlFieldCell cell, DataControlRowState rowState)
    {

        Label lbl = new Label();

        lbl.Text = this.DataField;

        cell.Controls.Add(lbl);

    }



    void txtBox_DataBinding(object sender, EventArgs e)
    {

        // get a reference to the control that raised the event

        TextBox target = (TextBox)sender;

        Control container = target.NamingContainer;



        // get a reference to the row object

        object dataItem = DataBinder.GetDataItem(container);



        // get the row's value for the named data field only use Eval when it is neccessary

        // to access child object values, otherwise use GetPropertyValue. GetPropertyValue

        // is faster because it does not use reflection

        object dataFieldValue = null;



        if (this.DataField.Contains("."))
        {

            dataFieldValue = DataBinder.Eval(dataItem, this.DataField);

        }

        else
        {

            dataFieldValue = DataBinder.GetPropertyValue(dataItem, this.DataField);

        }



        // set the table cell's text. check for null values to prevent ToString errors

        if (dataFieldValue != null)
        {

            target.Text = dataFieldValue.ToString();

        }

    }

    #endregion

}

