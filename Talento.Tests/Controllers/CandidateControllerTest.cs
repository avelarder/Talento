using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Principal;
using System.Web.Mvc;
using Talento.Core;
using Talento.Entities;
using Talento.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talento.Models;
using System.Net.Mail;

namespace Talento.Tests.Controllers
{
    //[TestClass]
    //public class CandidateControllerTest
    //{
    //    [TestMethod]
    //    public void SendEmail()
    //    {
    //        Mock<SmtpClient> smtpClientMock = new Mock<SmtpClient>();

    //        smtpClientMock.SetupProperty(p => p.Port == 465).SetupProperty(p => p.Credentials, new System.Net.NetworkCredential("apikey", "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")).
    //            SetupProperty(p => p.UseDefaultCredentials, false).SetupProperty(p => p.DeliveryMethod, SmtpDeliveryMethod.Network).SetupProperty(p => p.EnableSsl, true);

    //        Mock<MailMessage> mailMesageMock = new Mock<MailMessage>();
    //        mailMesageMock.SetupProperty(p => p.Subject, "subject").SetupProperty(p => p.Body, "body");

    //        Assert.IsNotNull(smtpClientMock, "");
    //    }


    //}
}
