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
using BaseUserClassDll;
using LibraryMusicContactDll;

namespace MusicPlayerOnline.ViewModel
{
    partial class MainWindowViewModel : ViewModelBase
    {

        private int _countMySong;

        public int CountMySong
        {
            get { return _countMySong; }
            set
            {
                _countMySong = value;
                OnPropertyChanged();
            }
        }


        private void MyClient_MyMusicCountPropertyChanged(object sender, EventArgs e)
        {
            CountMySong = myClient.MyMusicCount;
        }


        //Коллекция My Play List  пользователя
        ObservableCollection<Song> _mySong;
        public ObservableCollection<Song> MySong
        {
            get
            {
                return _mySong;
            }
            set
            {
                _mySong = value;
                OnPropertyChanged();
            }
        }


        private void MyClient_MyMusicPropertyChanged(object sender, EventArgs e)
        {
            MySong = new ObservableCollection<Song>(myClient.MyMusic as List<Song>);
        }


        private int _myskip;

        public int MySkip
        {
            get { return _myskip; }
            set
            {
                _myskip = value;
                OnPropertyChanged();
            }
        }

        private int _mytake;

        public int MyTake
        {
            get { return _mytake; }
            set
            {
                _mytake = value;
                OnPropertyChanged();
            }
        }

        private Song _oneSong;

        public Song OneSong
        {
            get { return _oneSong; }
            set
            {
                _oneSong = value;
                OnPropertyChanged();
            }
        }


        private ICommand _getSong;
        public ICommand GetSong
        {
            get
            {
                if (_getSong == null)
                    _getSong = new RelayCommand(GetSongClick);
                return _getSong;
            }
            set
            {
                _getSong = value;
            }
        }
        void GetSongClick(object param)
        {
        
            OneSong = (param as Song);

            connector.AddOneSongToMyPlayList(User_client, OneSong);

            GetMyMusicByFilter();
        }


        private ICommand _leftRemove;
        public ICommand LeftRemove
        {
            get
            {
                if (_leftRemove == null)
                    _leftRemove = new RelayCommand(LeftRemoveClick);
                return _leftRemove;
            }
            set
            {
                _leftRemove = value;
            }
        }
        void LeftRemoveClick(object param)
        {
            OneSong = (param as Song);

            string result_remove = connector.RemoveLeftPanelSong(User_client, OneSong);

            if (result_remove != null)
            {
                MessageBox.Show(result_remove);

                MessageBoxResult result = MessageBox.Show("Try connect one more?", "Music Player", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    connector.ConnectToServer();
                }

            }

            GetMyMusicByFilter();
        }


        //свойства для фильтров MY MUSIC
        private string _selectedMyGenre;

        public string SelectedMyGenre
        {
            get { return _selectedMyGenre; }
            set
            {
                _selectedMyGenre = value;

                MySkip = 0;
                MyTake = 10;
                GetMyMusicByFilter();
                OnPropertyChanged();//("SelectedItem");
            }
        }

        private string _selectedMyProducer;

        public string SelectedMyProducer
        {
            get { return _selectedMyProducer; }
            set
            {
                _selectedMyProducer = value;
                MySkip = 0;
                MyTake = 10;
                GetMyMusicByFilter();
                OnPropertyChanged();//("SelectedItem");
            }
        }


        //обработчик фильтров MY MUSIC
        private void GetMyMusicByFilter()
        {
            Filter filter = new Filter() { Producer = SelectedMyProducer, Genre = SelectedMyGenre };

            try
            {
                MySong = new ObservableCollection<Song>(connector.GetMySongByFilter(User_client, filter, Skip, Take) as List<Song>);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                CountMySong = connector.GetMyCountSelectedMusic();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private ICommand _prevMyCommand;
        public ICommand PrevMyCommand
        {
            get
            {
                if (_prevMyCommand == null)
                    _prevMyCommand = new RelayCommand(PrevMyClick);
                return _prevMyCommand;
            }
            set
            {
                _prevMyCommand = value;
            }
        }
        void PrevMyClick(object param)
        {
            MySkip -= MyTake;
            if (MySkip < 0)
            {
                MySkip = 0;
            }

            if (MySkip >= 0)
            {
                GetMyMusicByFilter();
            }
        }


        private ICommand _nextMyCommand;
        public ICommand NextMyCommand
        {
            get
            {
                if (_nextMyCommand == null)
                    _nextMyCommand = new RelayCommand(NextMyClick);
                return _nextMyCommand;
            }
            set
            {
                _nextMyCommand = value;
            }
        }
        void NextMyClick(object param)
        {
            MySkip += MyTake;

            if (MySkip > CountMySong)
            {
                MySkip = (MySkip - MyTake);
            }

            if (MySkip == CountMySong)
            {
                MySkip = CountMySong - MyTake;
            }


            if (MySkip < CountMySong)
            {
                GetMyMusicByFilter();
            }
        }

    }
}
