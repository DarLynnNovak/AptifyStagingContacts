Imports System.Drawing
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.WindowsControls
Imports System.Windows.Forms
Imports Aptify.Framework.DataServices

Public Class ACSStagingContactsCompanyLinkLC
    Inherits FormTemplateLayout

    Private m_oAppObj As New Aptify.Framework.Application.AptifyApplication
    Private m_oDA As New DataAction
    Private bAdded As Boolean = False
    Private lGridID As Long = -1

    Private WithEvents grdCompanySearch As DataGridView

    Protected WithEvents lblSearch As CultureLabel
    Private WithEvents btnCreate As AptifyActiveButton
    Private WithEvents btnUpdate As AptifyActiveButton

    Private WithEvents lCompanyLinkbox As AptifyLinkBox
    Private WithEvents txtCompanyName As AptifyTextBox
    Private WithEvents ddCompanyType As AptifyLinkBox

    Private WithEvents txtAddress1 As AptifyTextBox
    Private WithEvents txtAddress2 As AptifyTextBox
    Private WithEvents txtAddress3 As AptifyTextBox
    Private WithEvents txtAddress4 As AptifyTextBox
    Protected WithEvents txtCity As AptifyTextBox
    Private WithEvents lState As AptifyLinkBox
    Private WithEvents cmbCountryCodeId As AptifyLinkBox
    Private WithEvents txtZipCode As AptifyTextBox
    Private WithEvents txtWebsite As AptifyTextBox
    Private WithEvents txtDateProcessed As AptifyTextBox
    Private WithEvents txtSearchFor As AptifyTextBox
    Private WithEvents lbEntityId As AptifyLinkBox

    Dim acsJPPAppEntityId As Long
    Dim cName As String

    Private _sCheckCompanyName As String = ""
    Private _sCheckCompanyType As String = ""
    Private _sCheckAddress1 As String = ""
    Private _sCheckAddress2 As String = ""
    Private _sCheckAddress3 As String = ""
    Private _sCheckAddress4 As String = ""
    Private _sCheckCountryCode As String = ""
    Private _sCheckCity As String = ""
    Private _sCheckState As String = ""
    Private _sCheckZipCode As String = ""
    Private _sCheckWebsite As String = ""
    Dim Sql As String
    Dim dtSearch As DataTable
    Dim companyName As String
    Dim currentDate As DateTime = Now
    Private CoStagingForm As ACSJPPCompanyGridLC
    Public result As String
    Public Shared acsjpprecordid As Long
    Public Shared companyId As Long
    Public Shared OpenTemplateId As Long

    Protected Overrides Sub OnFormTemplateLoaded(ByVal e As FormTemplateLoadedEventArgs)
        Try

            'Me.AutoScroll = True

            FindControls()
            If grdCompanySearch Is Nothing Then
                grdCompanySearch = CreateGrid()
            End If
            'AssignBaseFieldsForChecking()
            'lblSearch.Text = ""

            'Dim lTypeID As Long = -1
            txtSearchFor.Value = Me.FormTemplateContext.GE.GetValue("Name")
            Dim companyName As String = Me.FormTemplateContext.GE.GetValue("Name")
            cName = Me.FormTemplateContext.GE.GetValue("Name")
            CompanyLookup(companyName, "")


            If Me.DataAction.UserCredentials.Server.ToLower = "aptify" Then
                'production
                acsJPPAppEntityId = 2824

            End If
            If Me.DataAction.UserCredentials.Server.ToLower = "stagingaptify2" Then
                acsJPPAppEntityId = 2873
            End If

            If Me.DataAction.UserCredentials.Server.ToLower = "testaptifydb" Then
                acsJPPAppEntityId = 2980
                OpenTemplateId = 28175
            End If
            acsjpprecordid = Me.FormTemplateContext.GE.GetValue("RecordID")
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
        'MyBase.OnFormTemplateLoaded(e)
    End Sub

    Protected Overridable Sub FindControls()
        Try

            If lblSearch Is Nothing OrElse lblSearch.IsDisposed = True Then
                lblSearch = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.Tabs.General.Culture Label.1"), CultureLabel)
            End If

            If btnUpdate Is Nothing OrElse btnUpdate.IsDisposed = True Then
                btnUpdate = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.Tabs.General.Active Button.2"), AptifyActiveButton)
            End If

            If btnCreate Is Nothing OrElse btnCreate.IsDisposed = True Then
                btnCreate = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.Tabs.General.Active Button.1"), AptifyActiveButton)
            End If
            If lCompanyLinkbox Is Nothing OrElse lCompanyLinkbox.IsDisposed = True Then
                lCompanyLinkbox = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.CompanyId"), AptifyLinkBox)
            End If
            If txtCompanyName Is Nothing OrElse txtCompanyName.IsDisposed = True Then
                txtCompanyName = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.Tabs.General.Name"), AptifyTextBox)
            End If
            If txtAddress1 Is Nothing OrElse txtAddress1.IsDisposed = True Then
                txtAddress1 = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.AddressLine1"), AptifyTextBox)
            End If
            If txtAddress2 Is Nothing OrElse txtAddress2.IsDisposed = True Then
                txtAddress2 = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.AddressLine2"), AptifyTextBox)
            End If
            If txtAddress3 Is Nothing OrElse txtAddress3.IsDisposed = True Then
                txtAddress3 = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.AddressLine3"), AptifyTextBox)
            End If
            'If txtAddress4 Is Nothing OrElse txtAddress4.IsDisposed = True Then
            '    txtAddress4 = TryCast(GetFormComponent(Me, "ACS.ACSJPPAppStagingCompany.AddressLine4"), AptifyTextBox)
            'End If
            If txtCity Is Nothing OrElse txtCity.IsDisposed = True Then
                txtCity = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.Tabs.General.City"), AptifyTextBox)
            End If
            If lState Is Nothing OrElse lState.IsDisposed = True Then
                lState = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.State"), AptifyLinkBox)
            End If
            If cmbCountryCodeId Is Nothing OrElse cmbCountryCodeId.IsDisposed = True Then
                cmbCountryCodeId = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.CountryId"), AptifyLinkBox)
            End If
            If txtZipCode Is Nothing OrElse txtZipCode.IsDisposed = True Then
                txtZipCode = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.ZipCode"), AptifyTextBox)
            End If
            If txtWebsite Is Nothing OrElse txtWebsite.IsDisposed = True Then
                txtWebsite = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.WebSiteURL"), AptifyTextBox)
            End If
            If txtDateProcessed Is Nothing OrElse txtDateProcessed.IsDisposed = True Then
                txtDateProcessed = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.DateProcessed"), AptifyTextBox)
            End If

            If txtSearchFor Is Nothing OrElse txtSearchFor.IsDisposed = True Then
                txtSearchFor = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.Tabs.General.SearchCompany"), AptifyTextBox)
            End If
            If lbEntityId Is Nothing OrElse lbEntityId.IsDisposed = True Then
                lbEntityId = TryCast(GetFormComponent(Me, "ACS.ACSStagingCompanys.Tabs.General.EntityId"), AptifyLinkBox)
            End If

        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub
    Private Sub AssignBaseFieldsForChecking()
        Try
            _sCheckCompanyName = txtCompanyName.Text
            _sCheckAddress1 = txtAddress1.Text
            _sCheckAddress2 = txtAddress2.Text
            _sCheckAddress3 = txtAddress3.Text
            _sCheckAddress4 = txtAddress4.Text
            _sCheckCity = txtCity.Text
            _sCheckState = lState.Text
            _sCheckCountryCode = cmbCountryCodeId.Text
            _sCheckZipCode = txtZipCode.Text
            _sCheckWebsite = txtWebsite.Text
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub
    Public Sub CheckComLB()
        If Not lCompanyLinkbox Is Nothing AndAlso CInt(lCompanyLinkbox.Value) > 0 Then
            grdCompanySearch.Hide()
            lblSearch.Hide()
            txtSearchFor.Hide()
            btnUpdate.Visible = False
            btnCreate.Visible = False

        ElseIf dtSearch.Rows.Count > 0 Then
            btnUpdate.Visible = True
            btnCreate.Visible = False

        Else
            btnUpdate.Visible = False
            btnCreate.Visible = True

        End If
    End Sub

    Private Function CreateGrid() As DataGridView
        Try
            Dim grdReturn As DataGridView
            'Dim gridtop = lCompanyLinkbox.Top + lCompanyLinkbox.Height + 10 
            grdReturn = New DataGridView
            grdReturn.Name = "grdCompanySearch"
            grdReturn.Size = New Drawing.Size(600, 200)
            grdReturn.Location = New Drawing.Point(175, 500)
            Controls.Add(grdReturn)
            grdReturn.Visible = True
            Return grdReturn
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return Nothing
        End Try
    End Function

    Private Sub txtSearchFor_ValueChanged(ByVal sender As Object, ByVal OldValue As Object, ByVal NewValue As Object) Handles txtSearchFor.ValueChanged
        'Dim lTypeID As Long = -1
        'If IsNumeric(ddCompanyType.Value) Then
        '    ' lTypeID = CLng(ddCompanyType.Value)
        'End If

        companyName = NewValue.ToString()
        CompanyLookup(companyName, "")
    End Sub

    Private Sub CompanyLookup(ByVal companyName As String, ByVal Message As String)
        Try
            'Dim dtSearch As DataTable
            Dim companyNameNew As String = companyName.Replace("'", "''")
            'lblSearch.Text = ""
            If Not companyName Is Nothing AndAlso Not companyName.ToString = "" Then
                Sql = "select ID, Name,City,State from aptify..vwcompanies where name like " & "'%" & companyNameNew & "%'"
                dtSearch = m_oDA.GetDataTable(Sql)

                If dtSearch.Rows.Count > 0 Then
                    'btnUpdate.Visible = True
                    'lblSearch.Text = Message
                    grdCompanySearch.DataSource = dtSearch
                    grdCompanySearch.Columns(0).ReadOnly = False
                    'Check to see if the CheckBox Column Exists
                    If grdCompanySearch.Columns(0).CellType.ToString = "System.Windows.Forms.DataGridViewCheckBoxCell" Then
                        bAdded = True
                    Else
                        bAdded = False
                    End If
                    If bAdded = False Then
                        Dim AddColumn As New DataGridViewCheckBoxColumn

                        With AddColumn
                            .HeaderText = ""
                            .Name = "grdChecked"
                            .Width = 21
                        End With

                        grdCompanySearch.Columns.Insert(0, AddColumn)

                        For i As Integer = 1 To grdCompanySearch.ColumnCount - 1
                            grdCompanySearch.Columns(i).ReadOnly = True
                        Next

                        bAdded = True
                    End If
                    lblSearch.Text = "Please choose one of the following records and click update."
                    lblSearch.ForeColor = Color.Blue
                Else
                    If LTrim(RTrim(companyName)) = "" OrElse LTrim(RTrim(companyName.ToString)) = "" Then
                        dtSearch.Clear()
                        lblSearch.Text = "No match found as Company Name is blank."
                        lblSearch.ForeColor = Color.Red
                    Else
                        lblSearch.Text = "No match found. Please try a different Company Name or click Create New Company."
                        lblSearch.ForeColor = Color.Red
                    End If
                    If lCompanyLinkbox.Value > 0 Then
                        lblSearch.Text = "Processing has been completed."
                        lblSearch.ForeColor = Color.Blue
                    End If
                    lblSearch.Refresh()
                    grdCompanySearch.DataSource = dtSearch
                    grdCompanySearch.Columns(0).ReadOnly = True
                End If
            Else

                If LTrim(RTrim(companyName)) = "" OrElse LTrim(RTrim(companyName.ToString)) = "" Then
                    lblSearch.Text = "No match found as Company Name is blank."
                    lblSearch.ForeColor = Color.Red
                Else
                    lblSearch.Text = "No match found. Please try a different Company Name or click Create New Company."
                    lblSearch.ForeColor = Color.Red
                End If
                If lCompanyLinkbox.Value > 0 Then
                    lblSearch.Text = "Processing has been completed."
                    lblSearch.ForeColor = Color.Blue
                End If
                lblSearch.Refresh()
                dtSearch = m_oDA.GetDataTable("select ID = '', Name = '', City = '', State = '',Country = ''", IAptifyDataAction.DSLCacheSetting.BypassCache)
                dtSearch.Clear()
                grdCompanySearch.DataSource = dtSearch
                grdCompanySearch.Columns(0).ReadOnly = True
            End If
            CheckComLB()
            grdCompanySearch.AllowUserToAddRows = False
            grdCompanySearch.Refresh()
            lGridID = -1
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub

    Private Sub grdCompanySearch_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdCompanySearch.CellValueChanged
        Try
            Dim bFound As Boolean = False
            If e.ColumnIndex = 0 Then
                For i As Integer = 0 To grdCompanySearch.RowCount - 1
                    If Not grdCompanySearch.Item(0, i).Value Is Nothing _
                           AndAlso grdCompanySearch.Item(0, i).Value.ToString.ToUpper = "TRUE" Then
                        lGridID = CLng(grdCompanySearch.Item("ID", i).Value)
                        bFound = True
                    End If
                Next
            End If
            If bFound = False Then
                lGridID = -1
            End If
            grdCompanySearch.Invalidate()

        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub

    Private Sub grdCompanySearch_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdCompanySearch.CellClick
        Try
            If e.ColumnIndex = 0 Then
                For i As Integer = 0 To grdCompanySearch.RowCount - 1
                    If Not grdCompanySearch.Item(0, i).Value Is Nothing _
                        AndAlso Not grdCompanySearch.Item(0, i).Value.ToString.ToUpper = "FALSE" Then
                        grdCompanySearch.Item(0, i).Value = False

                    End If
                Next
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub

    Private Sub grdCompanySearch_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdCompanySearch.CurrentCellDirtyStateChanged
        If grdCompanySearch.IsCurrentCellDirty Then
            grdCompanySearch.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub



    Private Function CheckIfTextFieldsChanged() As Boolean
        Try
            Dim bChanged As Boolean = False
            If Not _sCheckCompanyName = txtCompanyName.Text OrElse
                Not _sCheckCompanyType = ddCompanyType.Value.ToString OrElse
                Not _sCheckAddress1 = txtAddress1.Text OrElse
                Not _sCheckAddress2 = txtAddress2.Text OrElse
                Not _sCheckAddress3 = txtAddress3.Text OrElse
                Not _sCheckAddress4 = txtAddress4.Text OrElse
                Not _sCheckCity = txtCity.Text OrElse
                Not _sCheckState = lState.Text OrElse
                Not _sCheckCountryCode = cmbCountryCodeId.Value.ToString OrElse
                Not _sCheckZipCode = txtZipCode.Text OrElse
                Not _sCheckWebsite = txtWebsite.Text Then

                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return True
        End Try
    End Function


    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'ACSOGBCompanyLinkLC
        '
        Me.Name = "ACSStagingContactsCompanyLinkLC"
        Me.Size = New System.Drawing.Size(1237, 707)
        Me.ResumeLayout(False)

    End Sub

    'Private Sub ddCompanyType_ValueChanged(sender As Object, OldValue As Object, NewValue As Object) Handles ddCompanyType.ValueChanged
    '    If Not NewValue Is Nothing Then
    '        If IsNumeric(NewValue) AndAlso CLng(NewValue) > 0 AndAlso Len(txtCompanyName.Text) > 0 Then
    '            CompanyLookup(txtCompanyName.Value.ToString, CLng(NewValue), lblSearch.Text)
    '        End If
    '    End If
    'End Sub
    Private Sub ShowMessage(ByVal Message As String, Optional ByVal IsError As Boolean = False)
        If Not lblSearch.Text Is Nothing AndAlso Not lblSearch.Text = "" Then
            lblSearch.Text = Message
        Else
            lblSearch.Text = ""
        End If
        lblSearch.Text = Message
        lblSearch.ForeColor = Color.Black
        If IsError Then
            lblSearch.ForeColor = Color.Red
        End If
    End Sub
    Private Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        Try

            Dim sCompanyName As String = ""
            sCompanyName = txtCompanyName.Value.ToString

            If Not lCompanyLinkbox Is Nothing Then
                If IsNumeric(lCompanyLinkbox.RecordID) AndAlso lCompanyLinkbox.RecordID > 0 Then
                    If lCompanyLinkbox.RecordName = txtCompanyName.Text Then
                        MsgBox("You cannot create a duplicate company and assign to the pre-application which currently is linked to " & lCompanyLinkbox.RecordName & ".")
                        Exit Sub
                    End If
                End If
            End If

            Select Case MsgBox("Are you sure you want to create " & sCompanyName & " ?", MsgBoxStyle.YesNo, "Company Create")
                Case MsgBoxResult.Yes

                    'If CheckIfTextFieldsChanged() Then
                    '    MsgBox("One or more values have changed after the form loaded, please save the record before creating a company.")
                    '    Exit Sub
                    'End If

                    Dim oCompany As AptifyGenericEntity
                    oCompany = CType(m_oAppObj.GetEntityObject("Companies", -1), AptifyGenericEntity)
                    With oCompany
                        '    '    Dim sCompanyTypeSQL As String = "Select ID From Aptify.dbo.vwCompanyTypes Where Name = (Select Name From Aptify.dbo.vwACSMBSPreAppCompanyTypes Where ID = " & ddCompanyType.Text & ")"
                        '    '    Dim dtCompanyType As DataTable = Nothing
                        '    '    dtCompanyType = m_oDA.GetDataTable(sCompanyTypeSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                        '    '    If dtCompanyType.Rows.Count > 0 Then
                        '    '        For Each dr As DataRow In dtCompanyType.Rows
                        '    '            .SetValue("CompanyTypeID", dtCompanyType.ToString)
                        '    '        Next
                        '    '    End If

                        '    'Need to make sure that the values on the Form are updating the GE
                        'UpdateGE()

                        .SetValue("Name", FormTemplateContext.GE.GetValue("Name"))
                        .SetValue("AddressLine1", FormTemplateContext.GE.GetValue("AddressLine1"))
                        .SetValue("AddressLine2", FormTemplateContext.GE.GetValue("AddressLine2"))
                        .SetValue("AddressLine3", FormTemplateContext.GE.GetValue("AddressLine3"))
                        .SetValue("AddressLine4", FormTemplateContext.GE.GetValue("AddressLine4"))
                        '.SetValue("CountryCodeId", FormTemplateContext.GE.GetValue("CountryId"))
                        .SetValue("City", FormTemplateContext.GE.GetValue("City"))
                        .SetValue("State", FormTemplateContext.GE.GetValue("State"))
                        .SetValue("ZipCode", FormTemplateContext.GE.GetValue("ZipCode"))
                        .SetValue("CountryCodeID", FormTemplateContext.GE.GetValue("CountryId"))
                        .SetValue("Website", FormTemplateContext.GE.GetValue("WebsiteURL"))

                    End With

                    Dim sErr As String = ""
                    oCompany.Save(sErr)

                    If sErr.Length > 0 Then
                        ShowMessage("There was a problem updating " & sCompanyName & ". Please refer to the Aptify exception log.", True)
                    Else
                        lblSearch.Text = "Company " & sCompanyName & " updated successfully!"
                        result = "Success"
                        'ACSJPPCompanyGridLC.refreshgrids(result)
                    End If

                    Dim ACSStagingCompany As AptifyGenericEntity
                    Dim oACSStagingCompanyId As Long = FormTemplateContext.GE.RecordID

                    ACSStagingCompany = CType(m_oAppObj.GetEntityObject("ACSStagingCompanys", oACSStagingCompanyId), AptifyGenericEntity)
                    Dim acsjpprecordid = Me.FormTemplateContext.GE.GetValue("RecordID")

                    If CLng(lbEntityId.Value) = acsJPPAppEntityId AndAlso result = "Success" Then


                        ACSJPPCompanyGridLC.refreshgrids(result, acsjpprecordid, oCompany.RecordID)

                    End If

                    If Not txtDateProcessed Is Nothing AndAlso Not txtDateProcessed.ToString = "" Then
                        txtDateProcessed.Enabled = False
                    Else
                        txtDateProcessed.Value = currentDate
                    End If

                    If oCompany.RecordID > 0 Then
                        lCompanyLinkbox.Value = oCompany.RecordID
                        oCompany.Save()
                    End If




                    'Dim lTypeID As Long = -1
                    'If IsNumeric(ddCompanyType.Value) Then
                    '    lTypeID = CLng(ddCompanyType.Value)
                    'End If
                    CompanyLookup(txtCompanyName.Value.ToString, lblSearch.Text)

            End Select

            'Facility Former Name ()
            'ID, ACSMBSPreAppID, Sequence, ACSMBSAQIPFormerName

            'goes to

            'Also Known As (ACSAlsoKnownAs)
            'ID, ACSCompanyID, ACSAKAName

        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try

    End Sub
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        Try
            'If Not lCompanyLinkbox.RecordID > 0 Then
            '    MsgBox("Please enter a Facility prior to processing the person information.")
            '    Exit Sub
            'End If

            Dim sCompanyName As String = ""
            Dim lID As Long = -1
            If lGridID > 0 Then
                lID = lGridID
                Dim oCompany As AptifyGenericEntity
                oCompany = CType(m_oAppObj.GetEntityObject("Companies", lID), AptifyGenericEntity)
                sCompanyName = oCompany.GetValue("Name").ToString

                Select Case MsgBox("Are you sure you want to update " & oCompany.GetValue("Name").ToString & " ?", MsgBoxStyle.YesNo, "Company Update")
                    Case MsgBoxResult.Yes

                        'With oCompany
                        '    'Need to make sure that the values on the Form are updating the GE
                        '    'UpdateGE()

                        '    .SetValue("Name", FormTemplateContext.GE.GetValue("Name"))
                        '    '.SetValue("ACSPreviousName", FormTemplateContext.GE.GetValue("ACSMBSAQIPCenterName"))
                        '    .SetValue("AddressLine1", FormTemplateContext.GE.GetValue("AddressLine1"))
                        '    .SetValue("AddressLine2", FormTemplateContext.GE.GetValue("AddressLine2"))
                        '    .SetValue("AddressLine3", FormTemplateContext.GE.GetValue("AddressLine3"))
                        '    .SetValue("AddressLine4", FormTemplateContext.GE.GetValue("AddressLine4"))
                        '    .SetValue("CountryCodeId", FormTemplateContext.GE.GetValue("CountryId"))
                        '    .SetValue("City", FormTemplateContext.GE.GetValue("City"))
                        '    .SetValue("State", FormTemplateContext.GE.GetValue("StateProvinceId"))
                        '    .SetValue("ZipCode", FormTemplateContext.GE.GetValue("PostalCode"))
                        '    '.SetValue("CountryCodeID", FormTemplateContext.GE.GetValue("CompanyId"))
                        '    .SetValue("Website", FormTemplateContext.GE.GetValue("Website"))

                        'End With
                        Dim sErr As String = ""
                        oCompany.Save(sErr)

                        If sErr.Length > 0 Then
                            ShowMessage("There was a problem updating " & sCompanyName & ". Please refer to the Aptify exception log.", True)
                        Else
                            lblSearch.Text = "Company " & sCompanyName & " updated successfully!"

                            result = "Success"
                        End If
                        ''''''''''''''''''''''

                        FormTemplateContext.GE.Save()


                        Dim lTypeID As Long = -1

                        'If IsNumeric(ddCompanyType.Value) Then
                        '    lTypeID = CLng(ddCompanyType.Value)
                        'End If

                        Dim ACSStagingCompany As AptifyGenericEntity
                        Dim oACSStagingCompanyId As Long = FormTemplateContext.GE.RecordID

                        ACSStagingCompany = CType(m_oAppObj.GetEntityObject("ACSStagingCompanys", oACSStagingCompanyId), AptifyGenericEntity)
                        Dim acsjpprecordid As Long = Me.FormTemplateContext.GE.GetValue("RecordID")

                        If CLng(lbEntityId.Value) = acsJPPAppEntityId Then


                            result = "Success"

                            ACSJPPCompanyGridLC.refreshgrids(result, acsjpprecordid, oCompany.RecordID)

                        End If


                        If Not txtDateProcessed Is Nothing AndAlso Not txtDateProcessed.ToString = "" Then
                            txtDateProcessed.Value = currentDate
                            txtDateProcessed.Enabled = False
                        Else
                            txtDateProcessed.Enabled = False
                            'txtDateProcessed.Value = currentDate
                        End If

                        If oCompany.RecordID > 0 Then

                            lCompanyLinkbox.Value = oCompany.RecordID

                        End If

                        CompanyLookup(txtCompanyName.Value.ToString, lblSearch.Text)

                        ''''''''''''''''''''''
                        'lCompanyLinkbox.Value = oCompany.RecordID


                        oCompany.Save()
                        ''CheckComLB()


                End Select
            Else
                MsgBox("Please select one matching Company from the list above.")
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try

    End Sub


    Private Sub UpdateGE()
        Try
            If txtCompanyName.Text <> FormTemplateContext.GE.GetValue("Name").ToString Then
                FormTemplateContext.GE.SetValue("Name", txtCompanyName.Text)
            End If
            If txtAddress1.Text <> FormTemplateContext.GE.GetValue("AddressLine1").ToString Then
                FormTemplateContext.GE.SetValue("AddressLine1", txtAddress1.Text)
            End If
            If txtAddress2.Text <> FormTemplateContext.GE.GetValue("AddressLine2").ToString Then
                FormTemplateContext.GE.SetValue("AddressLine2", txtAddress2.Text)
            End If
            If txtAddress3.Text <> FormTemplateContext.GE.GetValue("AddressLine3").ToString Then
                FormTemplateContext.GE.SetValue("AddressLine3", txtAddress3.Text)
            End If
            If txtAddress4.Text <> FormTemplateContext.GE.GetValue("AddressLine4").ToString Then
                FormTemplateContext.GE.SetValue("AddressLine4", txtAddress4.Text)
            End If
            If txtCity.Text <> FormTemplateContext.GE.GetValue("City").ToString Then
                FormTemplateContext.GE.SetValue("City", txtCity.Text)
            End If
            If lState.Text <> FormTemplateContext.GE.GetValue("StateProvinceId").ToString Then
                FormTemplateContext.GE.SetValue("StateProvinceId", lState.Text)
            End If
            If cmbCountryCodeId.Text <> FormTemplateContext.GE.GetValue("CountryId").ToString Then
                FormTemplateContext.GE.SetValue("CountryId", cmbCountryCodeId.Text)
            End If
            If txtZipCode.Text <> FormTemplateContext.GE.GetValue("PostalCode").ToString Then
                FormTemplateContext.GE.SetValue("PostalCode", txtZipCode.Text)
            End If
            If txtWebsite.Text <> FormTemplateContext.GE.GetValue("Website").ToString Then
                FormTemplateContext.GE.SetValue("Website", txtWebsite.Text)
            End If

        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub


    Private Sub checkAKA(ByVal lCompanyID As Long)
        Try
            Dim sSQL As String = "Select ID, AKAName From Aptify.dbo.vwACSCompanyAlsoKnownAsView Where CompanyID = " & lCompanyID.ToString
            Dim dtResult As DataTable = m_oDA.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
            If dtResult.Rows.Count > 0 Then
                Dim bMatch As Boolean = False
                Dim lAKAID As Long
                Dim sName As String = ""
                For Each oSub As AptifyGenericEntity In FormTemplateContext.GE.SubTypes("ACSMBSPreAppFacilityFormerName")
                    bMatch = False
                    lAKAID = -1
                    sName = ""
                    For Each oRow As DataRow In dtResult.Rows
                        lAKAID = CLng(oRow("ID"))
                        sName = oRow("AKAName").ToString
                        If LTrim(RTrim(oSub.GetValue("ACSMBSAQIPFormerName").ToString)) = LTrim(RTrim(sName)) Then
                            bMatch = True
                            Exit For
                        End If
                    Next
                    If bMatch = False Then
                        updateAKA(lCompanyID, sName)
                    End If
                Next
            Else
                For Each oSub As AptifyGenericEntity In FormTemplateContext.GE.SubTypes("ACSMBSPreAppFacilityFormerName")
                    updateAKA(lCompanyID, oSub.GetValue("ACSMBSAQIPFormerName").ToString)
                Next
            End If

        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub
    Private Sub updateAKA(ByVal lCompanyID As Long, ByVal sName As String)
        Try
            Dim oAKA As AptifyGenericEntity
            oAKA = CType(m_oAppObj.GetEntityObject("ACSCompanyAlsoKnownAs", -1), AptifyGenericEntity)
            With oAKA
                .SetValue("CompanyID", lCompanyID)
                .SetValue("AKAName", sName)
            End With
            oAKA.Save()
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub

    Private Sub ACSOGBCompanyLinkLC_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class



