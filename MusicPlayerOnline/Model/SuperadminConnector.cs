using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using SuperadminLibraryDll;
using BaseUserClassDll;
using System.Windows;


namespace MusicPlayerOnline.Model
{
    class SuperadminConnector
    {
        ISuperadminContract superChannel = null;

        ChannelFactory<ISuperadminContract> factory;

        public SuperadminConnector()
        {
            try
            {
                Uri adress = new Uri("net.tcp://localhost:4560/musicplayer");
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                EndpointAddress endpoint = new EndpointAddress(adress);

                factory = new ChannelFactory<ISuperadminContract>(binding, endpoint);
                superChannel = factory.CreateChannel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<string> GetMusicCompany()
        {
            try
            {
                return superChannel.GetMusicCompany();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void AddCompany(string name, string descript)
        {
            try
            {
                superChannel.AddNewMusicCompany(name, descript);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public List<UserPlayer> GetListAdmin(string nameCompany)
        {
            try
            {
                return superChannel.GetListAdmin(nameCompany);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            } 
        }

        public void AddNewAdmin(string nameCompany, string login, string email)
        {
            try
            {
                superChannel.AddNewAdmin(nameCompany, login, email);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
     }
}
