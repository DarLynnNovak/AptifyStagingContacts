
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices


Public Class ACSStagingContactsPush
    Implements IProcessComponent
    Private m_oApp As New AptifyApplication
    Private m_oProps As New AptifyProperties
    Private m_oGE As AptifyGenericEntityBase
    Private m_oda As DataAction
    Private m_sResult As String = "SUCCESS"

    Public Sub Config(ByVal ApplicationObject As Aptify.Framework.Application.AptifyApplication) Implements Aptify.Framework.BusinessLogic.ProcessPipeline.IProcessComponent.Config
        Try
            Me.m_oApp = ApplicationObject
            Me.m_oda = New Aptify.Framework.DataServices.DataAction(Me.m_oApp.UserCredentials)
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try
    End Sub

    Public ReadOnly Property Properties() As Aptify.Framework.Application.AptifyProperties Implements Aptify.Framework.BusinessLogic.ProcessPipeline.IProcessComponent.Properties
        Get
            If m_oProps Is Nothing Then
                m_oProps = New Aptify.Framework.Application.AptifyProperties
            End If
            Return m_oProps
        End Get
    End Property

    Public Function Run() As String Implements Aptify.Framework.BusinessLogic.ProcessPipeline.IProcessComponent.Run
        Try
            m_sResult = "Success"
            Dim result As ProcessFlowResult = Nothing, lProcessFlowID As Long = -1

            Dim da As New DataAction
            Dim boolSuccess As Boolean = False
            Dim procFlowSql As String
            Dim thisGE As AptifyGenericEntity
            Dim stagingContactsGE As AptifyGenericEntity
            Dim Id As Long
            Dim EntityId As Long
            Dim EntityNameSql As String
            Dim FirstName As String
            Dim LastName As String
            Dim Email As String
            Dim Credentials As String
            Dim Title As String
            Dim AddressLine1 As String
            Dim AddressLine2 As String
            Dim AddressLine3 As String
            Dim AddressLine4 As String
            Dim City As String
            Dim StateId As Long
            Dim ZipCode As String
            Dim CountryId As Long
            Dim CountryId_Name As String
            Dim PhoneCountryCode As Long
            Dim PhoneAreaCode As Long
            Dim PhoneNumber As Long
            Dim PhoneExtension As Long
            Dim FaxCountryCode As Long
            Dim FaxAreaCode As Long
            Dim FaxNumber As Long
            Dim Status As String
            Dim DateCreated As DateTime
            Dim DateProcessed As DateTime
            Dim FunctionId As Long
            Dim PersonId As Long
            Dim CompanyId As Long
            Dim FTEPercent As String
            Dim UseFacilityAddress As Boolean
            Dim BillingContact As Boolean
            Dim PrimaryContact As Boolean
            Dim ApplicationId As Long

            'If m_oda.UserCredentials.Server.ToLower = "aptify" Then
            'End If
            'If m_oda.UserCredentials.Server.ToLower = "stagingaptify2" Then
            'End If

            'If m_oda.UserCredentials.Server.ToLower = "testaptifydb" Then
            'End If

            thisGE = CType(Me.m_oProps.GetProperty("thisGE"), AptifyGenericEntity)
            Id = thisGE.GetValue("ID")

            EntityId = m_oApp.GetEntityID(thisGE.EntityName)

            If thisGE.EntityName Is "ACSAccreditationStagingContacts" Then
                FirstName = thisGE.GetValue("FirstName")
                LastName = thisGE.GetValue("LastName")
                Email = thisGE.GetValue("Email")
                FunctionId = thisGE.GetValue("FunctionTypeId")
                ApplicationId = thisGE.GetValue("PreApplicationId")
                CountryId = thisGE.GetValue("CountryId")
                PhoneNumber = thisGE.GetValue("PhoneNumber")
                Credentials = thisGE.GetValue("Credentials")
            End If
            If thisGE.EntityName Is "ACSMBSStagingContacts" Then
                FirstName = thisGE.GetValue("FirstName")
                LastName = thisGE.GetValue("LastName")
                Email = thisGE.GetValue("Email")
                FunctionId = thisGE.GetValue("FunctionTypeId")
                ApplicationId = thisGE.GetValue("PreApplicationId")
                CountryId = thisGE.GetValue("CountryCodeId")
                PhoneNumber = thisGE.GetValue("PhoneNumber")
                Credentials = thisGE.GetValue("Credentials")
            End If
            If thisGE.EntityName Is "ACSGroupBillingStagingContacts" Then
                FirstName = thisGE.GetValue("FirstName")
                LastName = thisGE.GetValue("LastName")
                Email = thisGE.GetValue("Email")
                FunctionId = thisGE.GetValue("FunctionTypeId")
                ApplicationId = thisGE.GetValue("PreApplicationId")
                CountryId = thisGE.GetValue("CountryId")
                PhoneNumber = thisGE.GetValue("PhoneNumber")
                Credentials = thisGE.GetValue("Credentials")
            End If
            If thisGE.EntityName Is "AcsOgbPartnerPersonSubType" Then
                FirstName = thisGE.GetValue("FirstName")
                LastName = thisGE.GetValue("LastName")
                Email = thisGE.GetValue("Email")
                PrimaryContact = thisGE.GetValue("IsPrimary")
                ApplicationId = thisGE.GetValue("OGBPartnerId")
                CountryId = thisGE.GetValue("CountryId")
                PhoneNumber = thisGE.GetValue("Phone")
                Credentials = thisGE.GetValue("Suffix")
            End If
            If thisGE.EntityName Is "ACSAcadNominator" Or EntityId = 2946 Then
                FirstName = thisGE.GetValue("PersonId_FirstName")
                LastName = thisGE.GetValue("PersonId_LastName")
                Email = thisGE.GetValue("PersonId_Email")
                PrimaryContact = thisGE.GetValue("IsPrimary")
                ApplicationId = thisGE.GetValue("ID")
                CountryId = thisGE.GetValue("CountryId")
                PhoneNumber = thisGE.GetValue("Phone")
                Credentials = thisGE.GetValue("Suffix")
            End If

            If thisGE.EntityName Is "ACSSARPerson" Then
                FirstName = thisGE.GetValue("FirstName")
                LastName = thisGE.GetValue("LastName")
                Email = thisGE.GetValue("Email")
                FunctionId = thisGE.GetValue("FunctionId")
                'ApplicationId = thisGE.GetValue("PreApplicationId")
                'CountryId = thisGE.GetValue("CountryId")
                PhoneNumber = thisGE.GetValue("Phone")
                Credentials = thisGE.GetValue("Title")
            End If

            If Id > 0 Then
                stagingContactsGE = m_oApp.GetEntityObject("ACSStagingContacts", -1)

                stagingContactsGE.SetValue("FirstName", FirstName)
                stagingContactsGE.SetValue("LastName", LastName)
                stagingContactsGE.SetValue("FunctionId", FunctionId)
                stagingContactsGE.SetValue("Email", Email)
                stagingContactsGE.SetValue("Credentials", thisGE.GetValue("Credentials"))
                stagingContactsGE.SetValue("Title", thisGE.GetValue("Title"))
                stagingContactsGE.SetValue("AddressLine1", thisGE.GetValue("AddressLine1"))
                stagingContactsGE.SetValue("AddressLine2", thisGE.GetValue("AddressLine2"))
                stagingContactsGE.SetValue("AddressLine3", thisGE.GetValue("AddressLine3"))
                stagingContactsGE.SetValue("AddressLine4", thisGE.GetValue("AddressLine4"))
                stagingContactsGE.SetValue("City", thisGE.GetValue("City"))
                stagingContactsGE.SetValue("StateId", thisGE.GetValue("StateId"))
                stagingContactsGE.SetValue("ZipCode", thisGE.GetValue("ZipCode"))
                stagingContactsGE.SetValue("CountryId", CountryId)
                stagingContactsGE.SetValue("PhoneCountryCode", thisGE.GetValue("PhoneCountryCode"))
                stagingContactsGE.SetValue("PhoneAreaCode", thisGE.GetValue("PhoneAreaCode"))
                stagingContactsGE.SetValue("PhoneNumber", thisGE.GetValue("PhoneNumber"))
                stagingContactsGE.SetValue("PhoneExtension", thisGE.GetValue("PhoneExtension"))
                stagingContactsGE.SetValue("FaxCountryCode", thisGE.GetValue("FaxCountryCode"))
                stagingContactsGE.SetValue("FaxAreaCode", thisGE.GetValue("FaxAreaCode"))
                stagingContactsGE.SetValue("FaxNumber", thisGE.GetValue("FaxNumber"))
                stagingContactsGE.SetValue("UseFacilityAddress", thisGE.GetValue("UseFacilityAddress"))
                stagingContactsGE.SetValue("BillingContact", thisGE.GetValue("BillingContact"))
                stagingContactsGE.SetValue("Status", thisGE.GetValue("Status"))
                stagingContactsGE.SetValue("PrimaryContact", thisGE.GetValue("PrimaryContact"))
                stagingContactsGE.SetValue("FTEPercent", thisGE.GetValue("FTEPercent"))
                stagingContactsGE.SetValue("Entityid", EntityId)
                stagingContactsGE.SetValue("RecordId", ApplicationId)


                If stagingContactsGE.IsDirty Then
                    If Not stagingContactsGE.Save(False) Then
                        Throw New Exception("Problem Saving Staging Contacts Record: " & stagingContactsGE.RecordID)

                    Else
                        stagingContactsGE.Save(True)
                        m_sResult = "Success"
                    End If
                End If
            Else
                m_sResult = "No Record"
            End If


        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return "Failed"
        End Try
    End Function
End Class
