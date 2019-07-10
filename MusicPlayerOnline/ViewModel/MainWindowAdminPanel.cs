using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MusicPlayerOnline.Model;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using MusicPlayerOnline.View;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BaseUserClassDll;
using LibraryMusicContactDll;
using AdminContractDll;

namespace MusicPlayerOnline.ViewModel
{

    partial class MainWindowViewModel : ViewModelBase
    {
        private string _pathToFile;

        public string PathToFile
        {
            get { return _pathToFile; }
            set
            {
                _pathToFile = value;
                OnPropertyChanged();
            }
        }


        private string _genreForAdd;

        public string GenreForAdd
        {
            get { return _genreForAdd; }
            set
            {
                _genreForAdd = value;
                OnPropertyChanged();
            }
        }

        private ICommand _openFolder;
        public ICommand OpenFolder
        {
            get
            {
                if (_openFolder == null)
                    _openFolder = new RelayCommand(OpenFolderClick);
                return _openFolder;
            }
            set
            {
                _openFolder = value;
            }
        }
        void OpenFolderClick(object param)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "mp3 files (*.mp3)|*.mp3";
            bool flag = true;
            while (flag == true)
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(openFileDialog.FileName);
                    if (fi.Length > 10000000)
                    {
                        MessageBox.Show("File size Error");
                        continue;
                    }
                    break;
                }
                else
                {
                    flag = false;
                }
            }
          
                PathToFile =flag? openFileDialog.FileName:String.Empty;           

        }

        private ICommand _addOneNewSong;
        public ICommand AddOneNewSong
        {
            get
            {
                if (_addOneNewSong == null)
                    _addOneNewSong = new RelayCommand(AddOneNewSongClick, CanExecuteAddOneNewSongClick);
                return _addOneNewSong;
            }
            set
            {
                _addOneNewSong = value;
            }
        }
        void AddOneNewSongClick(object param)
        {

            FileInfo info = new FileInfo(PathToFile);
            MessageBox.Show(info.FullName);

            StreamReader sourceStream = new StreamReader(PathToFile);
            byte[] fileContents = File.ReadAllBytes(PathToFile);// = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());

            sourceStream.Close();


            SongData data = new MusicID3Tag().DoParseFile(PathToFile);

            if (data != null)
            {
                ParametrSong ps = new ParametrSong();
                ps.User = User_client;
                ps.Genre = GenreForAdd;
                ps.NameArtist = data.NameArtist;
                ps.NameSong = data.NameSong;
                ps.FileName = info.Name;
                ps.File = fileContents;

                adminConnector.UploadFileToFtp(ps);
            }
        }
        bool CanExecuteAddOneNewSongClick(object param)
        {
            return !String.IsNullOrWhiteSpace(PathToFile);
        }


        private ICommand _removeSong;
        public ICommand RemoveSong
        {
            get
            {
                if (_removeSong == null)
                    _removeSong = new RelayCommand(RemoveSongClick);
                return _removeSong;
            }
            set
            {
                _removeSong = value;
            }
        }
        void RemoveSongClick(object param)
        {
            if(User_client.ID_Producer!=(param as Song).ID_Producer)
            {
                MessageBox.Show("У Вас нет прав на удаление!");
            }
            else
            {
                ParametrSong ps = new ParametrSong();
                ps.User = User_client;
                ps.NameArtist = (param as Song).Name_Artist;
                ps.NameSong = (param as Song).Name_Song;

                adminConnector.RemoveSong(ps);

                AllSong.Remove(param as Song);

            }
           
        }

    }
}
