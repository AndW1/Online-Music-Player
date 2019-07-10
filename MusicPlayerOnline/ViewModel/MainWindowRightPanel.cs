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
        //Коллекция всей доступной музыки
        ObservableCollection<Song> _allSong;
        public ObservableCollection<Song> AllSong
        {
            get
            {
                return _allSong;
            }
            set
            {
                _allSong = value;
                OnPropertyChanged();
            }
        }

        private void MyClient_AllMusicPropertyChanged(object sender, EventArgs e)
        {
            AllSong = new ObservableCollection<Song>(myClient.AllMusic as List<Song>);
        }


        private void MyClient_AllMusicCountPropertyChanged(object sender, EventArgs e)
        {
            CountAllSong = myClient.AllMusicCount;
        }

        //свойства для фильтров All MUSIC
        private string _selectedAllGenre;

        public string SelectedAllGenre
        {
            get { return _selectedAllGenre; }
            set
            {
                _selectedAllGenre = value;

                Skip = 0;
                Take = 10;
                GetAllMusicByFilter();
                OnPropertyChanged();//("SelectedItem");
            }
        }

        private string _selectedAllProducer;

        public string SelectedAllProducer
        {
            get { return _selectedAllProducer; }
            set
            {
                _selectedAllProducer = value;
                Skip = 0;
                Take = 10;
                GetAllMusicByFilter();
                OnPropertyChanged();//("SelectedItem");
            }
        }


        private int _countAllSong;

        public int CountAllSong
        {
            get { return _countAllSong; }
            set
            {
                _countAllSong = value;
                OnPropertyChanged();
            }
        }

        private bool _rightRemoveStatus;

        public bool RightRemoveStatus
        {
            get { return _rightRemoveStatus; }
            set
            {
                if (_rightRemoveStatus == value)
                    return;
                _rightRemoveStatus = value;
                OnPropertyChanged();
            }
        }


        private int _skip;

        public int Skip
        {
            get { return _skip; }
            set
            {
                _skip = value;
                OnPropertyChanged();
            }
        }

        private int _take;

        public int Take
        {
            get { return _take; }
            set
            {
                _take = value;
                OnPropertyChanged();
            }
        }


        //обработчик фильтров All MUSIC
        private void GetAllMusicByFilter()
        {
            Filter filter = new Filter() { Producer = SelectedAllProducer, Genre = SelectedAllGenre };

            try
            {
                AllSong = new ObservableCollection<Song>(connector.GetSongByFilter(filter, Skip, Take) as List<Song>);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                CountAllSong = connector.GetCountSelectedMusic();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


       


        private ICommand _prevAllCommand;
        public ICommand PrevAllCommand
        {
            get
            {
                if (_prevAllCommand == null)
                    _prevAllCommand = new RelayCommand(PrevAllClick);
                return _prevAllCommand;
            }
            set
            {
                _prevAllCommand = value;
            }
        }
        void PrevAllClick(object param)
        {
            Skip -= Take;
            if (Skip < 0)
            {
                Skip = 0;
            }

            if (Skip >= 0)
            {
              
                GetAllMusicByFilter();
            }
        }


        private ICommand _nextAllCommand;
        public ICommand NextAllCommand
        {
            get
            {
                if (_nextAllCommand == null)
                    _nextAllCommand = new RelayCommand(NextAllClick);
                return _nextAllCommand;
            }
            set
            {
                _nextAllCommand = value;
            }
        }
        void NextAllClick(object param)
        {
            Skip += Take;

            if (Skip > CountAllSong)
            {
                Skip = (Skip - Take);
            }

            if (Skip == CountAllSong)
            {
                Skip = CountAllSong - Take;
            }


            if (Skip < CountAllSong)
            {
              
                GetAllMusicByFilter();
            }
        }

    }
}
