Imports System.Drawing
Imports System.Windows.Forms
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.WindowsControls

Public Class ACSJPPCompanyGridLC
    Inherits FormTemplateLayout
    Public Shared m_oAppObj As New Aptify.Framework.Application.AptifyApplication
    Public m_oDA As New DataAction
    Public WithEvents StagingCompaniesELV As FormComponent
    Public WithEvents ProcessedCompaniesELV As FormComponent
    Public StagingCoELV As FormComponent
    Public ProcessedCoELV As FormComponent
    Public EntityId As Integer
    'Public result As String
    Public thisFormId As Integer
    Public Shared acsjpprecordid As Long
    Public Shared companyId As Long
    Public Shared OpenTemplateId As Long

    Protected Overrides Sub OnFormTemplateLoaded(ByVal e As FormTemplateLoadedEventArgs)
        Try
            If Me.DataAction.UserCredentials.Server.ToLower = "aptify" Then
                'production
                EntityId = 2824

            End If
            If Me.DataAction.UserCredentials.Server.ToLower = "stagingaptify2" Then
                EntityId = 2873
            End If

            If Me.DataAction.UserCredentials.Server.ToLower = "testaptifydb" Then
                EntityId = 2980
            End If

            If CLng(m_oAppObj.GetEntityID("ACSJPPApplication")) = EntityId Then

            End If
            FindControls()
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub


    Public Function FindControls()

        Try
            If StagingCompaniesELV Is Nothing OrElse StagingCompaniesELV.IsDisposed = True Then
                StagingCompaniesELV = TryCast(GetFormComponent(Me, "ACSJPPApplication Companies.Entity List View.1"), FormComponent)
            End If
            If ProcessedCompaniesELV Is Nothing OrElse ProcessedCompaniesELV.IsDisposed = True Then
                ProcessedCompaniesELV = TryCast(GetFormComponent(Me, "ACSJPPApplication Companies.Entity List View.2"), FormComponent)
            End If
            StagingCompaniesELV.Refresh()
            'ParentForm.Close()


        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Function
    Private Sub InitializeComponent()
        'Me.SuspendLayout()
        '
        'ACSOGBCompanyLinkLC
        '
        Me.Name = "ACSJPPCompanyGridLC"
        'Me.Size = New System.Drawing.Size(1237, 707)
        'Me.ResumeLayout(False)
        FindControls()
    End Sub

    Private Sub StagingCompaniesELV_ValueChanged(sender As Object, OldValue As Object, NewValue As Object) Handles StagingCompaniesELV.ValueChanged

        StagingCompaniesELV.Refresh()
    End Sub
    Private Sub ProcessedCompaniesELV_ValueChanged(sender As Object, OldValue As Object, NewValue As Object) Handles ProcessedCompaniesELV.ValueChanged
        ProcessedCompaniesELV.Refresh()

    End Sub
    Public Shared Sub refreshgrids(ByVal result As String, ByVal acsjpprecordid As Long, ByVal companyId As Long)


        If result = "Success" Then
            Dim ACSJPPApp As AptifyGenericEntity
            ACSJPPApp = CType(m_oAppObj.GetEntityObject("ACSJPPApplication", acsjpprecordid), AptifyGenericEntity)
            With ACSJPPApp
                .SetValue("CompanyId", companyId)
            End With
            ACSJPPApp.Save()
            MsgBox("Please refresh this screen to review the result.")

            m_oAppObj.DisplayEntityRecord("acsjppapplication", acsjpprecordid)

        End If


    End Sub


End Class
