using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BaseUserClassDll;
using AdminContractDll;
using System.Windows;

namespace MusicPlayerOnline.Model
{

    class AdminConnector
    {
        IAdminCintract adminChannel = null;

        ChannelFactory<IAdminCintract> factory;

        public AdminConnector()
        {
            try
            {
                Uri adress = new Uri("net.tcp://localhost:4570/musicplayer");
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                EndpointAddress endpoint = new EndpointAddress(adress);

                factory = new ChannelFactory<IAdminCintract>(binding, endpoint);
                adminChannel = factory.CreateChannel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      
       
        public async void UploadFileToFtp(ParametrSong parametrSong)
        {
            try
            {
                await Task.Run(() => adminChannel.UploadFileToFtp(parametrSong));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
   

        public async void RemoveSong(ParametrSong parametrSong)
        {
            try
            {
                await Task.Run(() => adminChannel.RemoveSong(parametrSong));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
