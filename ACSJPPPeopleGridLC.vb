Imports System.Drawing
Imports System.Windows.Forms
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.WindowsControls

Public Class ACSJPPPeopleGridLC
    Inherits FormTemplateLayout
    Private m_oAppObj As New Aptify.Framework.Application.AptifyApplication
    Private m_oDA As New DataAction
    Private WithEvents StagingPeopleELV As FormComponent
    Private WithEvents ProcessedPeopleELV As FormComponent


    Protected Overrides Sub OnFormTemplateLoaded(ByVal e As FormTemplateLoadedEventArgs)
        Try

            FindControls()
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub

    Protected Overridable Sub FindControls()
        Try

            If StagingPeopleELV Is Nothing OrElse StagingPeopleELV.IsDisposed = True Then
                StagingPeopleELV = TryCast(GetFormComponent(Me, "ACSJPPApplication Contacts.Entity List View.1"), FormComponent)
            End If
            If ProcessedPeopleELV Is Nothing OrElse ProcessedPeopleELV.IsDisposed = True Then
                ProcessedPeopleELV = TryCast(GetFormComponent(Me, "ACSJPPApplication Contacts.Entity List View.2"), FormComponent)
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub

End Class
