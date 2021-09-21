'Option Explicit On
'Option Strict On

Imports System.Drawing
Imports System.Windows.Forms
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.WindowsControls

Public Class ACSStagingContactsPersonLinkLC
    Inherits FormTemplateLayout
    Private m_oAppObj As New Aptify.Framework.Application.AptifyApplication
    Private m_oDA As New DataAction
    Private bAdded As Boolean = False
    Private lGridID As Long = -1

    Private WithEvents grdPeopleSearch As DataGridView
    Protected WithEvents lblSearch As CultureLabel
    Public WithEvents btnCreate As AptifyActiveButton
    Public WithEvents btnUpdate As AptifyActiveButton
    Public WithEvents btnLinkPerson As AptifyActiveButton

    Private WithEvents lbPerson As AptifyLinkBox
    Private WithEvents lbState As AptifyLinkBox
    Private WithEvents dcbCountry As AptifyDataComboBox
    Private WithEvents lbFunction As AptifyLinkBox
    Private WithEvents lbCompany As AptifyLinkBox
    Private WithEvents lbEntityId As AptifyLinkBox

    Private WithEvents txtFirstName As AptifyTextBox
    Private WithEvents txtLastName As AptifyTextBox
    Private WithEvents txtSuffix As AptifyTextBox
    Private WithEvents txtTitle As AptifyTextBox
    Private WithEvents txtPhoneCountryCode As AptifyTextBox
    Private WithEvents txtPhoneAreaCode As AptifyTextBox
    Private WithEvents txtPhone As AptifyTextBox
    Private WithEvents txtPhoneExtension As AptifyTextBox
    Private WithEvents txtEmail As AptifyTextBox

    Private WithEvents cbBillingContact As AptifyCheckBox
    Private WithEvents txtDateProcessed As AptifyTextBox
    Private WithEvents txtSearchFirstName As AptifyTextBox
    Private WithEvents txtSearchLastName As AptifyTextBox
    Private WithEvents txtAddressLine1 As AptifyTextBox
    Private WithEvents txtAddressLine2 As AptifyTextBox
    Private _sCheckPersonName As String = Nothing
    Private _sCheckPersonType As String = Nothing
    Private _sCheckAddress1 As String = Nothing
    Private _sCheckAddress2 As String = Nothing
    Private _sCheckAddress3 As String = Nothing
    Private _sCheckAddress4 As String = Nothing
    Private _sCheckCountryCode As String = Nothing
    Private _sCheckCity As String = Nothing
    Private _sCheckState As String = Nothing
    Private _sCheckZipCode As String = Nothing
    Private _sCheckWebsite As String = Nothing
    Dim dtSearch As DataTable
    Dim Sql As String
    Dim FirstName As String
    Dim LastName As String
    Dim EmailAddress As String
    Dim currentDate As DateTime = Now
    Dim partnerPersonId As Long
    Dim acsacadNominationEntityId As Long
    Dim acsJPPAppEntityId As Long
    Dim stagingContactsId As Long
    Dim ACSEntityRecordId As Long
    Dim PersonId As Long

    Dim sPersonFirstName As String = ""
    Dim sPersonLastName As String = ""
    Dim sPersonEmail As String = ""

    Dim lID As Long = -1
    Dim oPerson As AptifyGenericEntity
    Protected Overrides Sub OnFormTemplateLoaded(ByVal e As FormTemplateLoadedEventArgs)
        Try
            'Me.AutoScroll = True
            FindControls()
            If grdPeopleSearch Is Nothing Then
                grdPeopleSearch = CreateGrid()
            End If

            If Me.DataAction.UserCredentials.Server.ToLower = "aptify" Then
                'production
                acsacadNominationEntityId = 2824
                acsJPPAppEntityId = 2824

            End If
            If Me.DataAction.UserCredentials.Server.ToLower = "stagingaptify2" Then
                acsacadNominationEntityId = 2841
                acsJPPAppEntityId = 2873
            End If

            If Me.DataAction.UserCredentials.Server.ToLower = "testaptifydb" Then
                acsacadNominationEntityId = 2947
                acsJPPAppEntityId = 2980
            End If

            If Me.DataAction.UserCredentials.Server.ToLower = "testaptify610" Then
                acsacadNominationEntityId = 2824
                acsJPPAppEntityId = 2928
            End If

            If Me.DataAction.UserCredentials.Server.ToLower = "stagingaptify61" Then
                acsacadNominationEntityId = 2824
                acsJPPAppEntityId = 2928
            End If

            Dim lTypeID As Long = -1
            'If IsNumeric(ddCompanyType.Value) Then
            '    lTypeID = CLng(ddCompanyType.Value)
            'End If
            stagingContactsId = Me.FormTemplateContext.GE.GetValue("id")

            'txtSearchFirstName.Value = Me.FormTemplateContext.GE.GetValue("FirstName")
            'txtSearchLastName.Value = Me.FormTemplateContext.GE.GetValue("LastName")
            sPersonFirstName = FormTemplateContext.GE.GetValue("FirstName").ToString
            sPersonLastName = FormTemplateContext.GE.GetValue("LastName").ToString
            FirstName = Me.FormTemplateContext.GE.GetValue("FirstName")
            LastName = Me.FormTemplateContext.GE.GetValue("LastName")
            'EmailAddress = Me.FormTemplateContext.GE.GetValue("Email")
            PersonLookup(sPersonFirstName, sPersonLastName, "")

        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
        'MyBase.OnFormTemplateLoaded(e)
    End Sub
    Protected Overridable Sub FindControls()
        Try
            If lblSearch Is Nothing OrElse lblSearch.IsDisposed = True Then
                lblSearch = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.Culture Label.3"), CultureLabel)
            End If

            If btnCreate Is Nothing OrElse btnCreate.IsDisposed = True Then
                btnCreate = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.Active Button.1"), AptifyActiveButton)
            End If
            If btnUpdate Is Nothing OrElse btnUpdate.IsDisposed = True Then
                btnUpdate = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.Active Button.2"), AptifyActiveButton)
            End If
            If btnLinkPerson Is Nothing OrElse btnLinkPerson.IsDisposed = True Then
                btnLinkPerson = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.Active Button.3"), AptifyActiveButton)
            End If
            If lbPerson Is Nothing OrElse lbPerson.IsDisposed = True Then
                lbPerson = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.PersonId"), AptifyLinkBox)
            End If
            If lbState Is Nothing OrElse lbState.IsDisposed = True Then
                lbState = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.StateId"), AptifyLinkBox)
            End If
            If dcbCountry Is Nothing OrElse dcbCountry.IsDisposed = True Then
                dcbCountry = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.CountryId"), AptifyDataComboBox)
            End If
            If lbCompany Is Nothing OrElse lbCompany.IsDisposed = True Then
                lbCompany = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.CompanyId"), AptifyLinkBox)
            End If
            If lbFunction Is Nothing OrElse lbFunction.IsDisposed = True Then
                lbFunction = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.FunctionId"), AptifyLinkBox)
            End If
            If txtFirstName Is Nothing OrElse txtFirstName.IsDisposed = True Then
                txtFirstName = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.FirstName"), AptifyTextBox)
            End If
            If txtLastName Is Nothing OrElse txtLastName.IsDisposed = True Then
                txtLastName = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.LasttName"), AptifyTextBox)
            End If
            If txtSuffix Is Nothing OrElse txtSuffix.IsDisposed = True Then
                txtSuffix = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Credentials"), AptifyTextBox)
            End If
            If txtTitle Is Nothing OrElse txtTitle.IsDisposed = True Then
                txtTitle = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Title"), AptifyTextBox)
            End If
            If txtPhoneCountryCode Is Nothing OrElse txtPhoneCountryCode.IsDisposed = True Then
                txtPhoneCountryCode = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.PhoneCountryCode"), AptifyTextBox)
            End If
            If txtPhoneAreaCode Is Nothing OrElse txtPhoneAreaCode.IsDisposed = True Then
                txtPhoneAreaCode = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.PhoneAreaCode"), AptifyTextBox)
            End If
            If txtPhone Is Nothing OrElse txtPhone.IsDisposed = True Then
                txtPhone = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.PhoneNumber"), AptifyTextBox)
            End If
            If txtPhoneExtension Is Nothing OrElse txtPhoneExtension.IsDisposed = True Then
                txtPhoneExtension = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.PhoneExtension"), AptifyTextBox)
            End If
            If txtEmail Is Nothing OrElse txtEmail.IsDisposed = True Then
                txtEmail = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Email"), AptifyTextBox)
            End If
            If cbBillingContact Is Nothing OrElse cbBillingContact.IsDisposed = True Then
                cbBillingContact = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.BillingContact"), AptifyCheckBox)
            End If
            If txtDateProcessed Is Nothing OrElse txtDateProcessed.IsDisposed = True Then
                txtDateProcessed = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.DateProcessed"), AptifyTextBox)
            End If
            If txtSearchFirstName Is Nothing OrElse txtSearchFirstName.IsDisposed = True Then
                txtSearchFirstName = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.SearchForFirstName"), AptifyTextBox)
            End If
            If txtSearchLastName Is Nothing OrElse txtSearchLastName.IsDisposed = True Then
                txtSearchLastName = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.SearchForLastName"), AptifyTextBox)
            End If
            If txtAddressLine1 Is Nothing OrElse txtAddressLine1.IsDisposed = True Then
                txtAddressLine1 = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.SearchForFirstName"), AptifyTextBox)
            End If
            If txtSearchLastName Is Nothing OrElse txtSearchLastName.IsDisposed = True Then
                txtSearchLastName = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.SearchForLastName"), AptifyTextBox)
            End If
            If lbEntityId Is Nothing OrElse lbEntityId.IsDisposed = True Then
                lbEntityId = TryCast(GetFormComponent(Me, "ACS.ACSStagingContacts.Tabs.General.StagingContactAppId"), AptifyLinkBox)
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub
    Public Sub CheckComLB()
        If Not lbPerson Is Nothing AndAlso CInt(lbPerson.Value) > 0 Then
            grdPeopleSearch.Hide()
            lblSearch.Hide()
            txtSearchFirstName.Hide()
            txtSearchLastName.Hide()
            btnUpdate.Visible = False
            btnCreate.Visible = False
            btnLinkPerson.Visible = False
        ElseIf dtSearch.Rows.Count > 0 Then
            btnUpdate.Visible = True
            btnCreate.Visible = True
            btnLinkPerson.Visible = True
        Else
            btnUpdate.Visible = False
            btnCreate.Visible = True
            btnLinkPerson.Visible = False
        End If
    End Sub

    Private Function CreateGrid() As DataGridView
        Try
            Dim grdReturn As DataGridView
            'Dim gridtop = lCompanyLinkbox.Top + lCompanyLinkbox.Height + 10
            grdReturn = New DataGridView
            grdReturn.Name = "grdPeopleSearch"
            grdReturn.Size = New Drawing.Size(800, 300)
            grdReturn.Location = New Drawing.Point(75, 500)
            Controls.Add(grdReturn)
            grdReturn.Visible = True
            Return grdReturn
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return Nothing
        End Try
    End Function

    Private Sub txtSearchFirstName_ValueChanged(ByVal sender As Object, ByVal OldValue As Object, ByVal NewValue As Object) Handles txtSearchFirstName.ValueChanged
        Dim lTypeID As Long = -1

        FirstName = NewValue.ToString()
        LastName = txtSearchLastName.Value
        PersonLookup(FirstName, LastName, "")
    End Sub


    Private Sub txtSearchLastName_ValueChanged(ByVal sender As Object, ByVal OldValue As Object, ByVal NewValue As Object) Handles txtSearchLastName.ValueChanged
        Dim lTypeID As Long = -1

        LastName = NewValue.ToString()
        FirstName = txtSearchFirstName.Value
        PersonLookup(FirstName, LastName, "")
    End Sub

    Private Sub PersonLookup(ByVal FirstName As String, ByVal LastName As String, ByVal Message As String)
        Try
            'Dim dtSearch As DataTable
            'lblSearch.Text = ""

            If Not FirstName Is Nothing AndAlso Not FirstName.ToString = "" Or Not LastName Is Nothing AndAlso Not LastName.ToString = "" Then
                Sql = "select ID, FirstName,LastName,Email,ACSMemberClassID_Name,AddressLine1,City,State,ZipCode from aptify..vwpersons where Firstname like " & "'%" & FirstName.ToString & "%'" & "and Lastname like " & "'%" & LastName.ToString & "%' "
                'Sql = "select ID, FirstName,LastName,email from aptify..vwpersons where Email like '%" & txtEmail.Text & "%'"
                dtSearch = m_oDA.GetDataTable(Sql)

                If dtSearch.Rows.Count > 0 Then
                    'btnUpdate.Visible = True
                    'lblSearch.Text = Message 
                    grdPeopleSearch.DataSource = dtSearch
                    grdPeopleSearch.Columns(0).ReadOnly = False
                    'Check to see if the CheckBox Column Exists
                    If grdPeopleSearch.Columns(0).CellType.ToString = "System.Windows.Forms.DataGridViewCheckBoxCell" Then
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

                        grdPeopleSearch.Columns.Insert(0, AddColumn)

                        For i As Integer = 1 To grdPeopleSearch.ColumnCount - 1
                            grdPeopleSearch.Columns(i).ReadOnly = True
                        Next

                        bAdded = True
                    End If
                    lblSearch.Text = "Please choose one of the following records and click link."
                    lblSearch.ForeColor = Color.Blue
                Else
                    If LTrim(RTrim(FirstName)) = "" OrElse LTrim(RTrim(FirstName.ToString())) = "" Then

                        lblSearch.Text = "No match found as Person Name is blank."
                        lblSearch.ForeColor = Color.Red
                    Else
                        lblSearch.Text = "No match found. Please try a different Person Name or click Create New Person."
                        lblSearch.ForeColor = Color.Red
                    End If
                    If lbPerson.Value > 0 Then
                        lblSearch.Text = "User Processing has been completed."
                        lblSearch.ForeColor = Color.Blue
                    End If
                    lblSearch.Refresh()
                    grdPeopleSearch.DataSource = dtSearch
                    grdPeopleSearch.Columns(0).ReadOnly = True
                End If
            Else

                If LTrim(RTrim(FirstName)) = "" OrElse LTrim(RTrim(FirstName.ToString())) = "" Then

                    lblSearch.Text = "No match found as Person Name is blank."
                    lblSearch.ForeColor = Color.Red
                Else
                    lblSearch.Text = "No match found. Please try a different Person Name or click Create New Person."
                    lblSearch.ForeColor = Color.Red
                End If
                If lbPerson.Value > 0 Then
                    lblSearch.Text = "User Processing has been completed."
                    lblSearch.ForeColor = Color.Blue
                End If
                lblSearch.Refresh()
                dtSearch = m_oDA.GetDataTable("select ID = '', FirstName = '', LastName = '', Email1 = ''", IAptifyDataAction.DSLCacheSetting.BypassCache)
                dtSearch.Clear()
                grdPeopleSearch.DataSource = dtSearch
                grdPeopleSearch.Columns(0).ReadOnly = True

            End If
            CheckComLB()
            grdPeopleSearch.AllowUserToAddRows = False
            grdPeopleSearch.Refresh()
            lGridID = -1
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub

    Private Sub grdPeopleSearch_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdPeopleSearch.CellValueChanged
        Try
            Dim bFound As Boolean = False
            If e.ColumnIndex = 0 Then
                For i As Integer = 0 To grdPeopleSearch.RowCount - 1
                    If Not grdPeopleSearch.Item(0, i).Value Is Nothing _
                           AndAlso grdPeopleSearch.Item(0, i).Value.ToString.ToUpper = "TRUE" Then
                        lGridID = CLng(grdPeopleSearch.Item("ID", i).Value)
                        bFound = True
                    End If
                Next
            End If
            If bFound = False Then
                lGridID = -1
            End If
            grdPeopleSearch.Invalidate()

        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub

    Private Sub grdPersonSearch_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdPeopleSearch.CellClick
        Try
            If e.ColumnIndex = 0 Then
                For i As Integer = 0 To grdPeopleSearch.RowCount - 1
                    If Not grdPeopleSearch.Item(0, i).Value Is Nothing _
                        AndAlso Not grdPeopleSearch.Item(0, i).Value.ToString.ToUpper = "FALSE" Then
                        grdPeopleSearch.Item(0, i).Value = False

                    End If
                Next
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub

    Private Sub grdPeopleSearch_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdPeopleSearch.CurrentCellDirtyStateChanged
        If grdPeopleSearch.IsCurrentCellDirty Then
            grdPeopleSearch.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        Try

            If Not lbPerson Is Nothing Then
                If IsNumeric(lbPerson.RecordID) AndAlso lbPerson.RecordID > 0 Then
                    If lbPerson.RecordName = lbPerson.Text Then
                        MsgBox("You cannot create a duplicate Person " & lbPerson.RecordName & ".")
                        Exit Sub
                    End If
                End If
            End If
            lID = -1
            GetPersonObject()
            Select Case MsgBox("Are you sure you want to create " & sPersonFirstName & " " & sPersonLastName & " ?", MsgBoxStyle.YesNo, "Create Person")
                Case MsgBoxResult.Yes

                    UpdatePerson()

                    ProcessCoFunctions()

                    ProcessDate()

                    PersonLookup(sPersonFirstName, sPersonLastName, "")

            End Select
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try

    End Sub
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        Try

            If lGridID > 0 Then
                lID = lGridID
                GetPersonObject()
                Select Case MsgBox("Are you sure you want to update " & oPerson.GetValue("FirstName").ToString & " " & oPerson.GetValue("LastName").ToString & " ?", MsgBoxStyle.YesNo, "Person Update")
                    Case MsgBoxResult.Yes

                        UpdatePerson()

                        ProcessCoFunctions()

                        ProcessDate()

                        PersonLookup(sPersonFirstName, sPersonLastName, "")

                End Select

            Else
                MsgBox("Please select one matching People from the list above.")
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try

    End Sub
    Private Sub btnLinkPerson_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLinkPerson.Click

        Try
            If lGridID > 0 Then
                lID = lGridID
                GetPersonObject()
                Select Case MsgBox("Are you sure you want to link " & oPerson.GetValue("FirstName").ToString & " " & oPerson.GetValue("LastName").ToString & " ?", MsgBoxStyle.YesNo, "Person Update")
                    Case MsgBoxResult.Yes

                        LinkPerson()

                        ProcessDate()

                        PersonLookup(sPersonFirstName, sPersonLastName, "")

                End Select

            Else
                MsgBox("Please select one matching People from the list above.")
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try

    End Sub
    Private Sub ProcessCoFunctions()
        If CInt(FormTemplateContext.GE.GetValue("CompanyId")) > 0 AndAlso CInt(FormTemplateContext.GE.GetValue("FunctionID")) > 0 Then
            With oPerson.SubTypes("PersonCompanies").Add()
                .SetValue("CompanyID", FormTemplateContext.GE.GetValue("CompanyID"))
                .SetValue("PrimaryFunctionID", FormTemplateContext.GE.GetValue("FunctionID"))
                With .SubTypes("PersonCompanyFunctions").Add()
                    .SetValue("FunctionID", FormTemplateContext.GE.GetValue("FunctionID"))
                End With
            End With
        End If

    End Sub

    Private Sub ProcessDate()
        If Not txtDateProcessed Is Nothing AndAlso Not txtDateProcessed.ToString = "" Then
            txtDateProcessed.Enabled = False
        Else
            txtDateProcessed.Value = currentDate
        End If
    End Sub
    Private Sub GetPersonObject()
        oPerson = CType(m_oAppObj.GetEntityObject("Persons", lID), AptifyGenericEntity)
        PersonId = oPerson.RecordID
    End Sub
    Private Sub LinkPerson()
        Dim ACSStagingContacts As AptifyGenericEntity

        ACSStagingContacts = CType(m_oAppObj.GetEntityObject("ACSStagingContacts", stagingContactsId), AptifyGenericEntity)
        Dim ACSEntityRecordId As Long = Me.FormTemplateContext.GE.GetValue("RecordID")
        lbPerson.Value = PersonId
        If CLng(lbEntityId.Value) = acsacadNominationEntityId Then


            Dim ACSAcadNomination As AptifyGenericEntity
            ACSAcadNomination = CType(m_oAppObj.GetEntityObject("ACSAcadNomination", ACSEntityRecordId), AptifyGenericEntity)

            With ACSAcadNomination
                .SetValue("NomineePersonId", PersonId)
            End With
            ACSAcadNomination.Save()
            ACSAcadNomination.RefreshLinkedFields(FormTemplateContext.GE.GetValue("NomineePersonId"))
        End If

        If CLng(lbEntityId.Value) = acsJPPAppEntityId Then
            Dim ACSJPPApp As AptifyGenericEntity
            ACSJPPApp = CType(m_oAppObj.GetEntityObject("ACSJPPApplication", ACSEntityRecordId), AptifyGenericEntity)

            With ACSJPPApp
                .SetValue("ApplicantId", PersonId)
            End With
            ACSJPPApp.Save()
            ACSJPPApp.RefreshLinkedFields(FormTemplateContext.GE.GetValue("ApplicantId"))
        End If
    End Sub

    Private Sub UpdatePerson()

        'oPerson = CType(m_oAppObj.GetEntityObject("Persons", lID), AptifyGenericEntity)

        If oPerson.RecordID > 0 Then
            ' lbPerson.Value = oPerson.RecordID
            ' PersonId = oPerson.RecordID
            sPersonFirstName = oPerson.GetValue("FirstName").ToString
            sPersonLastName = oPerson.GetValue("LastName").ToString
        Else
            sPersonFirstName = FormTemplateContext.GE.GetValue("FirstName").ToString
            sPersonLastName = FormTemplateContext.GE.GetValue("LastName").ToString
        End If



        If lID = -1 Then
            With oPerson
                '    'Need to make sure that the values on the Form are updating the GE
                '    'UpdateGE()
                .SetValue("FirstName", sPersonFirstName)
                .SetValue("LastName", sPersonLastName)
                .SetValue("NameTitle", txtTitle.Text)
                .SetValue("Suffix", txtSuffix.Text)
                .SetValue("AddressLine1", FormTemplateContext.GE.GetValue("AddressLine1"))
                .SetValue("AddressLine2", FormTemplateContext.GE.GetValue("AddressLine2"))
                .SetValue("AddressLine3", FormTemplateContext.GE.GetValue("AddressLine3"))
                .SetValue("AddressLine4", FormTemplateContext.GE.GetValue("AddressLine4"))
                .SetValue("CountryCodeID", FormTemplateContext.GE.GetValue("CountryId"))
                .SetValue("City", FormTemplateContext.GE.GetValue("City"))
                .SetValue("State", FormTemplateContext.GE.GetValue("StateId"))
                .SetValue("ZipCode", FormTemplateContext.GE.GetValue("ZipCode"))
                .SetValue("Website", FormTemplateContext.GE.GetValue("Website"))
                .SetValue("PhoneCountryCode", FormTemplateContext.GE.GetValue("PhoneCountryCode"))
                .SetValue("PhoneAreaCode", FormTemplateContext.GE.GetValue("PhoneAreaCode"))
                .SetValue("Phone", FormTemplateContext.GE.GetValue("PhoneNumber"))
                .SetValue("PhoneExtension", FormTemplateContext.GE.GetValue("PhoneExtension"))
                .SetValue("CompanyID", FormTemplateContext.GE.GetValue("CompanyID"))

                sPersonEmail = FormTemplateContext.GE.GetValue("Email")
                '.SetValue("ID", lID)
                If oPerson.GetValue("Email") Is "" Then
                    .SetValue("Email1", sPersonEmail)
                End If

            End With

            Dim sErr As String = ""
            oPerson.Save(sErr)

            If sErr.Length > 0 Then
                ShowMessage("There was a problem creating the person record " & sPersonFirstName & " " & sPersonLastName & ". Please refer to the Aptify exception log.", True)
            Else
                lblSearch.Text = "Person " & sPersonFirstName & " " & sPersonLastName & " updated successfully!"
            End If

            If oPerson.RecordID > 0 Then
                PersonId = oPerson.RecordID
                LinkPerson()
            End If

        End If
            If lID > 0 Then
            LinkPerson()
        End If




    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'ACSStagingContactsPersonLinkLC
        '
        Me.Name = "ACSStagingContactsPersonLinkLC"
        Me.Size = New System.Drawing.Size(1237, 707)
        Me.ResumeLayout(False)

    End Sub

    Private Sub ShowMessage(ByVal Message As String, Optional ByVal IsError As Boolean = False)
        lblSearch.Text = Message
        lblSearch.ForeColor = Color.Black
        If IsError Then
            lblSearch.ForeColor = Color.Red
        End If
    End Sub

End Class