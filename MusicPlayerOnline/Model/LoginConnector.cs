using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MusicPlayerOnline.ViewModel;
using BaseUserClassDll;
using LibraryAuthorizationDll;
using System.Windows;

namespace MusicPlayerOnline.View
{
  
    public class StubRequest
    {
        public bool Origin { get; set; }
        public bool Stub { get; set; }
        public DateTime Date { get; set; }
    }

    public  class LoginConnector 
    {
        IAuthentificationContract channel;
        ChannelFactory<IAuthentificationContract> factory;
            public LoginConnector()
            { }

        public void ConnectToServer()
        {
            try
            {             
                Uri adress = new Uri("net.tcp://localhost:4550/musicplayer");
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                EndpointAddress endpoint = new EndpointAddress(adress);

               factory = new ChannelFactory<IAuthentificationContract>(binding, endpoint);
                channel = factory.CreateChannel();    
            }
            catch (Exception ex)
            {            
                MessageBox.Show(ex.Message);                   
            }
        }

        public void CloseLogin()
        {
             factory.Abort();
        }

        public StubRequest CheckUserInDB(string login, string password)
        {
            StubRequest stub = new StubRequest();

            try
            {     
                stub.Origin= channel.ChekUserInDataBase(login, password);
                stub.Stub = true;
                return stub;
            }
            catch (Exception ex)
            {              
                MessageBox.Show(ex.Message);
        
                stub.Origin = false;
                stub.Stub = false;
              
                return stub;
            }
        }

        public StubRequest CheckEmail(string email)
        {
            StubRequest stub = new StubRequest();

            try
            {
                stub.Origin = channel.ChekUserEmail(email);
                stub.Stub = true;
                return stub;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                stub.Origin = false;
                stub.Stub = false;

                return stub;
            }
        }

        public UserPlayer GetRgisteredUser(string login, string password)
        {
            try
            {
                return channel.GetRegisteredUser(login, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }


        public bool CreateNewPassword(string login, string password)
        {
            try
            {
                return channel.CreateNewPassword(login, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public UserPlayer RegisterNewUser(string login, string email, string password)
        {
            try
            {
              return  channel.RegisterNewUser(login, email, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                    return null;
            }
        }


        public StubRequest CheckEmailInBuffer(string email)
        {
            StubRequest stub = new StubRequest();
            try
            {
                DateTime result= channel.CheckEmailInBuffer(email);
                if(result==new DateTime())
                {               
                    stub.Origin = true;
                    stub.Stub = true;
                    return stub;
                }
                else
                {
                    stub.Origin = true;
                    stub.Stub = false;
                    stub.Date = result;
                    return stub;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                stub.Origin = false;
                stub.Stub = false;
                return stub;
            }         
        }
    }
}
