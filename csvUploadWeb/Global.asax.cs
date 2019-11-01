using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace csvUploadWeb
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }


        protected void Application_Errors(object sender, EventArgs e)
        {
    

        }



    }


}


/*
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.Mail
Public Class Global
Inherits System.Web.HttpApplication
Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
Dim mail As New MailMessage
Dim ErrorMessage = "The error description is as follows : "
& Server.GetLastError.ToStringmail.To = administrator @domain.com
mail.Subject = "Error in the Site"
mail.Priority = MailPriority.High
mail.BodyFormat = MailFormat.Text
mail.Body = ErrorMessage
SmtpMail.Send(mail)
End Sub
End Class
*/