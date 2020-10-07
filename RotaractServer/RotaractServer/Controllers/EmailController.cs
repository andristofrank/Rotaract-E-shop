using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace RotaractServer.Controllers.API
{
    public class EmailController : Controller
    {

        public static void SendEmailAboutStatusOfOrder(int orderId,string city, string UserEmail, string Status,bool New)
        {
            //string UserEmail, string Status
            
            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            m.From = new MailAddress("rotaractserver@gmail.com");
            m.To.Add(UserEmail);
            if (New)
            {
                m.Subject = "Your Order(ID:"+orderId+") is " + Status.ToLower();
                m.Body =
                    "All was quiet today until the…………..warehouse intercom crackled to life:\r\n\r\n“Listen up, people! We’ve got an order for our new friend from  "+city+"  …….”\r\n\r\n….but no one could hear the rest of the announcement over the thunderous roar of applause. \r\n\r\nSparkling Water bottles were popped. Tears of joy were shed. “Don’t Stop Believing” rang from every speaker. Even Alan our lead packer smiled—and Alan never smiles.\r\n\r\nSimply put, your order caused pandemonium and everyone is thrilled you’re now a customer. Thank you!\r\n\r\nOnce we clean up our celebration mess, we’ll be working to get your order packaged, shipped and on it’s way to you ASAP. \r\n\r\nIf you have any questions or problems, you can reply to this email. We’ll follow up with tracking information as soon as your package ships so you’ll know exactly when to expect delivery.\r\n\r\nTalk Soon,\r\n\r\n \r\n\r\nAdam Kounis\r\n\r\nRotaract Europe\r\n\r\nVolunteer Programmer";
            }
            else
            {
                m.Subject = "Your Order is on the way";
                m.Body = "Your Order(ID:" + orderId + ") status has been changed to " + Status.ToLower();
            }

            sc.Host = "smtp.gmail.com";
            try
            { 
                sc.Port = 587;
                sc.Credentials = new System.Net.NetworkCredential("rotaractserver", "RCaae2020");
                sc.EnableSsl = true;
                sc.Send(m);
            }
            catch (Exception ex)
            {
            }
        }
      
    }
}