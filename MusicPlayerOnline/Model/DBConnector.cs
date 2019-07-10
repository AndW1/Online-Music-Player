using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Windows;
using MusicPlayerOnline.ViewModel;
using System.Windows.Threading;
using BaseUserClassDll;
using LibraryMusicContactDll;
using System.IO;

namespace MusicPlayerOnline.Model
{
   public class DBConnector
    {
        DuplexChannelFactory<IMusicContract> Factory;
      
        IMusicContract service = null;

        ClientApp client;
        UserPlayer user;

        public DBConnector(UserPlayer user)
        {
            this.user = user;
            client = new ClientApp();     
        }


        public bool ConnectToServer()
        {
            try
            {
                Factory = new DuplexChannelFactory<IMusicContract>(new InstanceContext((object)(/*new ClientApp()*/client)), new NetTcpBinding(SecurityMode.None));

                service = Factory.CreateChannel(new EndpointAddress("net.tcp://localhost:4500/musicplayer"));          

                service.Connect(user);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public ClientApp GetClient { get { return client; } }

        public List<Song> GetSongByFilter(Filter filter, int skip, int take)
        {
            try
            {
                return service.GetAllMusicByFilter(filter, skip, take);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBoxResult result = MessageBox.Show("Try connect one more?", "Music Player", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    ConnectToServer();
                  
                }
                else
                {
                   // Application.Current.MainWindow.Close();
                   
                }
               
                return null;
            }

            //List<Song> tmp = service.GetAllMusicByFilter(filter);
            //return tmp;
        }

        public List<Song> GetMySongByFilter(UserPlayer user, Filter filter, int skip, int take)
        {
            return service.GetMyMusicByFilter(user, filter, skip, take);
        }

        public int GetMyCountSelectedMusic()
        {
            try
            {
                return service.GetCountMySelectedMusic();
            }
            catch (Exception)
            {
                throw;
            } 
        }

        public int GetCountSelectedMusic()
        {
            try
            {
                return service.GetCountSelectedMusic();
            }
            catch (Exception)
            {
                throw;
            }    
        }

        public void AddOneSongToMyPlayList(UserPlayer user, Song song)
        {
            try
            {
                service.AddSongToMyPlayList(user, song);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBoxResult result = MessageBox.Show("Try connect one more?", "Music Player", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    ConnectToServer();
                }

            }  
        }


        public string RemoveLeftPanelSong(UserPlayer user, Song song)
        {
            try
            {
                return service.RemoveFromMyPlayList(user, song);
            }
            catch (Exception ex)
            {
               //MessageBox.Show(ex.Message);
               //MessageBoxResult result = MessageBox.Show("Try connect one more?", "Music Player", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
               // if (result == MessageBoxResult.Yes)
               // {
               //     ConnectToServer();
               // }
                return ex.Message;
            }
        }

        public void UpdateCountListen(UserPlayer user, Song song)
        {
            try
            {
                service.UpdateCountListen(user, song);
            }
            catch (Exception)
            {
                throw;
            }
        }
       
        public void SaveMyPlayList(UserPlayer user, string nameAlbum, List<Song> listSong)
        {
            try
            {
                service.SaveUserSelectedAlbum(user, nameAlbum, listSong);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<Song>  GetSelectedAlbum(UserPlayer user, string nameAlbum)
        {
            try
            {
                return service.GetSelectedAlbum(user, nameAlbum);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }


        public  void DeleteUserAlbum(UserPlayer user, string nameAlbum)
        {
            try
            {
                service.DeleteUserAlbum(user, nameAlbum);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        //public void AddNewSong(UserPlayer user, string fileName, string genre, string artist, string nameSong,  byte[] fileContents)
        //{
        //    try
        //    {
        //        MessageBox.Show("connector111");
        //        MessageBox.Show(user.ID_user.ToString());
        //        MessageBox.Show(fileName);
        //        MessageBox.Show(genre);
        //        MessageBox.Show(artist);
        //        MessageBox.Show(nameSong);
        //        MessageBox.Show(fileContents.Length.ToString());

        //        service.AddOneSong(user, fileName, genre, artist, nameSong, fileContents);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        if (ex.InnerException != null)
        //            MessageBox.Show(ex.InnerException.ToString());
        //    }
        //}

        //    public async void AdminAddSong(ParametrSong parametrSong)
        //{
        //    try
        //    {
        //        await Task.Run(() => service.UploadFileToFtp(parametrSong));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        public void Disconnect()
        {
            try
            {
                service.Disconnect();
                Factory.Abort();
            }
            catch (Exception )
            {
                //MessageBox.Show(ex.Message);
            }  
        }
    }
}
