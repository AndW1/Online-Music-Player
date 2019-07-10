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
using SuperadminLibraryDll;
using AdminContractDll;


namespace MusicPlayerOnline.ViewModel
{

  partial  class MainWindowViewModel : ViewModelBase
    {
        MusicPlayerOnline.Model.DBConnector connector;
        MusicPlayerOnline.Model.ClientApp myClient;

        SuperadminConnector superConnector;

        AdminConnector adminConnector;

        UserPlayer User_client;

        Authointeficator winLogin;

        bool OpenWindow;

        bool connect = true;
        
        #region Конструктор, подключение к сервису, подписка на события, стартовые настройки MainWindowViewModel


        public MainWindowViewModel()
        {
            winLogin = new Authointeficator();
            winLogin.UserPropertyChanged += WinLogin_UserPropertyChanged;
            winLogin.MainWindowPropertyChanged += WinLogin_MainWindowPropertyChanged;
            winLogin.ShowDialog();

            if (!OpenWindow)
            {
                connect = false;
                Application.Current.MainWindow.Close();
            }

            if (connect)
            {
                connector = new Model.DBConnector(User_client);

                Skip = 0;
                Take = 10;

                MySkip = 0;
                MyTake = 10;
             
                StartConnection();
                SelectedCentralIndex = 0;
            }
        }

        private void WinLogin_UserPropertyChanged(object sender, EventArgs e)
        {
            User_client = winLogin.UserPlayer;
            
            switch (User_client.ID_status)
            {
                case 1:
                    RightRemoveStatus = false;
                    Application.Current.MainWindow.Height = 625;
                    Application.Current.MainWindow.Width = 900;
                    WidthA = 0;
                    WidthS = 0;
                    break;
                case 2:
                    RightRemoveStatus = true;
                    Application.Current.MainWindow.Height = 625;
                    Application.Current.MainWindow.Width = 1150;

                    adminConnector = new AdminConnector();

                    WidthA = 250;
                    WidthS = 0;
                    break;

                case 3:
                    RightRemoveStatus = false;
                    Application.Current.MainWindow.Height = 625;
                    Application.Current.MainWindow.Width = 1150;
                    WidthA = 0;
                    WidthS = 250;

                    superConnector = new SuperadminConnector();
                    CompanyForComboBox = superConnector.GetMusicCompany();
                   
                    break;
            }
        }

        private void MyClient_CompanyPropertyChanged(object sender, EventArgs e)
        {
            CompanyForComboBox = myClient.CompanyList;
        }

        private void WinLogin_MainWindowPropertyChanged(object sender, EventArgs e)
        {
            OpenWindow = winLogin.OpenMainWindow;
            UserName = User_client.User_name;
        }

        private void StartConnection()
        {
            bool answear = connector.ConnectToServer();

            if (answear)
            {

                myClient = connector.GetClient;
                myClient.GenrePropertyChanged += MyClient_GenrePropertyChanged;
                myClient.ProducerPropertyChanged += MyClient_ProducerPropertyChanged;
                myClient.AllMusicPropertyChanged += MyClient_AllMusicPropertyChanged;
                myClient.AllMusicCountPropertyChanged += MyClient_AllMusicCountPropertyChanged;


                myClient.MyMusicCountPropertyChanged += MyClient_MyMusicCountPropertyChanged;
                myClient.MyMusicPropertyChanged += MyClient_MyMusicPropertyChanged;
                myClient.UpdateCountPropertyChanged += MyClient_UpdateCountPropertyChanged;
                myClient.MyAlbumPropertyChanged += MyClient_MyAlbumPropertyChanged;

                myClient.CompanyPropertyChanged += MyClient_CompanyPropertyChanged;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Try connect one more?", "Music Player", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    StartConnection();
                }
                else
                {
                    Application.Current.MainWindow.Close();
                }
            }
        }

        #endregion


        //Изменение числа прослушивания для трека
        private void MyClient_UpdateCountPropertyChanged(object sender, EventArgs e)
        {
            Song updated = myClient.UpdatedSong;
            for (int i = 0; i < AllSong.Count; i++)
            {
                if (AllSong[i].Path_to_Song == updated.Path_to_Song)
                {
                    AllSong[i] = updated;
                    OnPropertyChanged();
                    break;
                }
            }

            for (int i = 0; i < MySong.Count; i++)
            {
                if (MySong[i].Path_to_Song == updated.Path_to_Song)
                {
                    MySong[i] = updated;
                    OnPropertyChanged();
                    break;
                }
            }
        }

       

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }





        #region ФУНКЦИОНАЛ ВЗАИМОДЕЙСТВИЯ SIZE MainWindow
        //=======================================================
        private int _widtha;

        public int WidthA
        {
            get { return _widtha; }
            set
            {
                _widtha = value;
                OnPropertyChanged();
            }
        }

        private int _widths;

        public int WidthS
        {
            get { return _widths; }
            set
            {
                _widths = value;
                OnPropertyChanged();
            }
        }
        //======================================================================
        #endregion


      


        //Bindings for combobox
        private List<string> _genre;

        public List<string> GenreForComboBox
        {
            get { return _genre; }
            set
            {
                _genre = value;
                OnPropertyChanged();
            }
        }



        private List<string> _producer;

        public List<string> ProducerForComboBox
        {
            get { return _producer; }
            set
            {
                _producer = value;
                OnPropertyChanged();
            }
        }


        private List<string> _company;

        public List<string> CompanyForComboBox
        {
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged();
            }
        }


        #region Обработка комманд SAVE, LOAD, DELETE, CLEAR

        private void MyClient_MyAlbumPropertyChanged(object sender, EventArgs e)
        {
            MyAlbumComboBox = myClient.MyAlbum;
            OnPropertyChanged();
        }

        private List<string> _myAlbum;

        public List<string> MyAlbumComboBox
        {
            get { return _myAlbum; }
            set
            {
                _myAlbum = value;
                OnPropertyChanged();
            }
        }


        private string _newListName;

        public string NewAlbumName
        {
            get { return _newListName; }
            set
            {
                _newListName = value;
                OnPropertyChanged();
            }
        }

        private ICommand _saveAlbumCommand;
        public ICommand SaveAlbumCommand
        {
            get
            {
                if (_saveAlbumCommand == null)
                    _saveAlbumCommand = new RelayCommand(SaveAlbumClick/*, CanExecuteSaveClick*/);
                return _saveAlbumCommand;
            }
            set
            {
                _saveAlbumCommand = value;
            }
        }
        void SaveAlbumClick(object param)
        {
            connector.SaveMyPlayList(User_client, NewAlbumName, CentralPlayList.ToList());
        }

        //bool CanExecuteSaveClick(object param)
        //{
        //    return (!String.IsNullOrWhiteSpace(NewAlbumName) && CentralPlayList.Count!=0 && CentralList.Count!=0);
        //}

        private ICommand _selectedAlbumCommand;
        public ICommand SelectedAlbumCommand
        {
            get
            {
                if (_selectedAlbumCommand == null)
                    _selectedAlbumCommand = new RelayCommand(SelectedAlbumClick);
                return _selectedAlbumCommand;
            }
            set
            {
                _selectedAlbumCommand = value;
            }
        }
        void SelectedAlbumClick(object param)
        {

            try
            {
              CentralPlayList = new ObservableCollection<Song>(connector.GetSelectedAlbum(User_client, param.ToString()));
            }
            catch /*()*/
            {
                //Работать будет именно так в данной реализации!!!!!!
                //Иначе после отработки команды DELETE, param становится = null и сразу EXEPTION
            }


            CentralList.Clear();

            SelectedCentralIndex = 0;

            IsActive = false;

            NewAlbumName = null;

            MediaState = MediaState.Stop;

            foreach (var song in CentralPlayList)
            {
                string tmp = "";
                tmp += song.Name_Artist;
                tmp += " / ";
                tmp += song.Name_Song;
                CentralList.Add(tmp);
            }
        }


        private string _selectedAlbum;

        public string SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                _selectedAlbum = value;
                OnPropertyChanged();
            }
        }


        private ICommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                    _clearCommand = new RelayCommand(ClearCommandClick);
                return _clearCommand;
            }
            set
            {
                _clearCommand = value;
            }
        }
        void ClearCommandClick(object param)
        {
            if (MediaState == MediaState.Play)
                MediaState = MediaState.Stop;
            if (MediaState == MediaState.Pause)
                MediaState = MediaState.Stop;

            if (CentralPlayList.Count>0)
            CentralPlayList.Clear();

            if (CentralList.Count > 0)
                CentralList.Clear();

           NewAlbumName=null;
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(DeleteCommandClick, CanExecuteDeleteClick);
                return _deleteCommand;
            }
            set
            {
                _deleteCommand = value;
            }
        }
        void DeleteCommandClick(object param)
        {
            if (MediaState == MediaState.Play)
                MediaState = MediaState.Stop;
            if (MediaState == MediaState.Pause)
                MediaState = MediaState.Stop;
     
            connector.DeleteUserAlbum(User_client, SelectedAlbum);

            CentralList.Clear();
            CentralPlayList.Clear();
        }

        bool CanExecuteDeleteClick(object param)
        {
            return !String.IsNullOrEmpty(SelectedAlbum);
        }

        private ICommand _textCommand;
        public ICommand TextChangedCommand
        {
            get
            {
                if (_textCommand == null)
                    _textCommand = new RelayCommand(TextChangedCommandClick);
                return _textCommand;
            }
            set
            {
                _textCommand = value;
            }
        }
        void TextChangedCommandClick(object param)
        {
            if (CentralPlayList.Count != 0 && CentralList.Count != 0)
            {
               // SelectedAlbum = null;

                SaveAlbumCommand = new RelayCommand(SaveAlbumClick);

                IsActive = true;              
            }
        }

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }

        #endregion




        //Обработка событий
        private void MyClient_ProducerPropertyChanged(object sender, EventArgs e)
        {
            ProducerForComboBox = myClient.Producer;
        }

        private void MyClient_GenrePropertyChanged(object sender, EventArgs e)
        {
            GenreForComboBox = myClient.Genre;
        }


        //ФУНКЦИОНАЛ ВЗАИМОДЕЙСТВИЯ С MediaElement
        //=======================================================

        private Uri _selectedSong;

        public Uri SelectedSong
        {
            get { return _selectedSong; }
            set
            {
                if (_selectedSong == value)
                {
                    return;
                }
                _selectedSong = value;
                OnPropertyChanged();
            }
        }

        private MediaState _mediaState;

        public MediaState MediaState
        {
            get { return _mediaState; }
            set
            {
                if (_mediaState == value)
                {
                    return;
                }
                _mediaState = value;
                OnPropertyChanged();
            }
        }




        //ОБРАБОТКА КОМАНД
        //=======================================================

        //Выйти из приложения
        private ICommand _closeClientCommand;
        public ICommand CloseClientCommand
        {
            get
            {
                if (_closeClientCommand == null)
                    _closeClientCommand = new RelayCommand(CloseClientClick);
                return _closeClientCommand;
            }
            set
            {
                _closeClientCommand = value;
            }
        }
        void CloseClientClick(object param)
        {
            if (SelectedSong != null)
            {
                MediaState = MediaState.Stop;
            }

            try
            {
                connector.Disconnect();
                Application.Current.MainWindow.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.MainWindow.Close();
            }
            // App.Current.Shutdown(); Нельзя закрыает сервер 
        }


        //Свернуть в трей
        private ICommand _trayClientCommand;
        public ICommand TrayClientCommand
        {
            get
            {
                if (_trayClientCommand == null)
                    _trayClientCommand = new RelayCommand(TrayClientClick);
                return _trayClientCommand;
            }
            set
            {
                _trayClientCommand = value;
            }
        }
        void TrayClientClick(object param)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }


        //Включить воспроизведение

        private ICommand _playEelement;
        public ICommand PlayElement
        {
            get
            {
                if (_playEelement == null)
                    _playEelement = new RelayCommand(PlayClick);
                return _playEelement;
            }
            set
            {
                _playEelement = value;
            }
        }
        void PlayClick(object param)
        {
            CentralCanPlay = false;
            MessageBox.Show((param as Song).Path_to_Song);

            if (SelectedSong != null)
            {
               
                MediaState = MediaState.Stop;
            }
            SelectedSong = new Uri((param as Song).Path_to_Song);
            MediaState = MediaState.Play;

            connector.UpdateCountListen(User_client, param as Song);
        }




        #region ФУНКЦИОНАЛ ВЗАИМОДЕЙСТВИЯ С БЛОКОМ CENTRAL MUSIC
        //ФУНКЦИОНАЛ ВЗАИМОДЕЙСТВИЯ С БЛОКОМ CENTRAL MUSIC
        //=======================================================

        ObservableCollection<Song> _centralPlayList;//= new ObservableCollection<Song>();
        public ObservableCollection<Song> CentralPlayList
        {
            get
            {
                if (_centralPlayList == null)
                    _centralPlayList = new ObservableCollection<Song>();
                return _centralPlayList;
            }
            set
            {
                if (_centralPlayList == null)
                    _centralPlayList = new ObservableCollection<Song>();
                _centralPlayList = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<string> _centralList;
        public ObservableCollection<string> CentralList
        {
            get
            {
                if (_centralList == null)
                    _centralList = new ObservableCollection<string>();
                return _centralList;
            }
            set
            {
                if (_centralList == null)
                    _centralList = new ObservableCollection<string>();
                _centralList = value;
                OnPropertyChanged();
            }
        }

       



        private ICommand _leftAdd;
        public ICommand LeftAdd
        {
            get
            {
                if (_leftAdd == null)
                    _leftAdd = new RelayCommand(LeftAddClick);
                return _leftAdd;
            }
            set
            {
                _leftRemove = value;
            }
        }
        void LeftAddClick(object param)
        {        
            CentralPlayList.Add(param as Song);

            string tmp = "";
            tmp += (param as Song).Name_Artist;
            tmp += " / ";
            tmp += (param as Song).Name_Song;
            CentralList.Add(tmp);

        }

      

        private int _selectedCentralIndex;

        public int SelectedCentralIndex
        {
            get { return _selectedCentralIndex; }
            set
            {
                _selectedCentralIndex = value;
                OnPropertyChanged();
            }
        }

        private bool _centralCanPlay;

        public bool CentralCanPlay
        {
            get { return _centralCanPlay; }
            set
            {
                _centralCanPlay = value;
                OnPropertyChanged();
            }
        }

        private bool _isPause;

        public bool IsPause
        {
            get { return _isPause; }
            set
            {
                _isPause = value;
                OnPropertyChanged();
            }
        }



        private ICommand _playCentral;
        public ICommand PlayCentral
        {
            get
            {
                if (_playCentral == null)
                    _playCentral = new RelayCommand(PlayCentralClick);
                return _playCentral;
            }
            set
            {
                _leftRemove = value;
            }
        }
        void PlayCentralClick(object param)
        {
            CentralCanPlay = true;

            if(IsPause)
            {
                MediaState = MediaState.Play;
                IsPause = false;
            }
            else
            {
                if (SelectedSong != null)
                {
                    MediaState = MediaState.Stop;
                }

                SelectedSong = new Uri(CentralPlayList[SelectedCentralIndex].Path_to_Song);
                              
                MediaState = MediaState.Play;

                connector.UpdateCountListen(User_client, CentralPlayList[SelectedCentralIndex]);
            }

        }

        private ICommand _mediaEnded;
        public ICommand SongMediaEnded
        {
            get
            {
                if (_mediaEnded == null)
                    _mediaEnded = new RelayCommand(MediaEndedEvent);
                return _mediaEnded;
            }
            set
            {
                _mediaEnded = value;
            }
        }
        void MediaEndedEvent(object param)
        {
            if (CentralCanPlay)
            {
                SelectedCentralIndex++;
                if (SelectedCentralIndex == CentralPlayList.Count)
                {
                    SelectedCentralIndex = 0;
                }
                PlayCentralClick(param);
            }
        }


        private ICommand _stopCommand;
        public ICommand StopCommand
        {
            get
            {
                if (_stopCommand == null)
                    _stopCommand = new RelayCommand(StopCommandClick);
                return _stopCommand;
            }
            set
            {
                _stopCommand = value;
            }
        }
        void StopCommandClick(object param)
        {
            CentralCanPlay = false;
                MediaState = MediaState.Stop;
        }

        private ICommand _pauseCommand;
        public ICommand PauseCommand
        {
            get
            {
                if (_pauseCommand == null)
                    _pauseCommand = new RelayCommand(PauseCommandClick);
                return _pauseCommand;
            }
            set
            {
                _pauseCommand = value;
            }
        }
        void PauseCommandClick(object param)
        {
            if (CentralCanPlay == true)
            {
                MediaState = MediaState.Pause;
                IsPause = true;
            }
        }



        private ICommand _prevCentrlCommand;
        public ICommand PrevCentralCommand
        {
            get
            {
                if (_prevCentrlCommand == null)
                    _prevCentrlCommand = new RelayCommand(PrevCentralClick);
                return _prevCentrlCommand;
            }
            set
            {
                _prevCentrlCommand = value;
            }
        }
        void PrevCentralClick(object param)
        {  
                SelectedCentralIndex--;
                if (SelectedCentralIndex < 0)
                {
                    SelectedCentralIndex = CentralPlayList.Count-1;
                }
            IsPause = false;
                PlayCentralClick(param);          
        }


        private ICommand _nextCentrlCommand;
        public ICommand NextCentralCommand
        {
            get
            {
                if (_nextCentrlCommand == null)
                    _nextCentrlCommand = new RelayCommand(NextCentralClick);
                return _nextCentrlCommand;
            }
            set
            {
                _nextCentrlCommand = value;
            }
        }
        void NextCentralClick(object param)
        {
            SelectedCentralIndex++;
            if (SelectedCentralIndex == CentralPlayList.Count)
            {
                SelectedCentralIndex = 0;
            }
            IsPause = false;
            PlayCentralClick(param);
        }
        #endregion

        #region ФУНЦИОНАЛ SUPERADMIN PANEL
        //SUPERADMIN ACTION

        private string _nameCompany;

        public string NameCompany
        {
            get { return _nameCompany; }
            set
            {
                _nameCompany = value;
                OnPropertyChanged();
            }
        }


        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }


        private ICommand _addCompany;
        public ICommand AddCompany
        {
            get
            {
                if (_addCompany == null)
                    _addCompany = new RelayCommand(AddCompanyClick);
                return _addCompany;
            }
            set
            {
                _addCompany = value;
            }
        }
        void AddCompanyClick(object param)
        {
            if (!String.IsNullOrEmpty(NameCompany) && !String.IsNullOrEmpty(Description))
            { superConnector.AddCompany(NameCompany, Description); }
            else
            {
                MessageBox.Show("Введите данные!");
            }
        }

        private string _selectedNameCompany;

        public string SelectedNameCompany
        {
            get { return _selectedNameCompany; }
            set
            {
                _selectedNameCompany = value;
                OnPropertyChanged();
            }
        }

        private string _loginAdmin;

        public string LoginAdmin
        {
            get { return _loginAdmin; }
            set
            {
                _loginAdmin = value;
                OnPropertyChanged();
            }
        }

        private string _emailAdmin;

        public string EmailAdmin
        {
            get { return _emailAdmin; }
            set
            {
                _emailAdmin = value;
                OnPropertyChanged();
            }
        }


        private ICommand _getAdminCommand;
        public ICommand GetAdminCommand
        {
            get
            {
                if (_getAdminCommand == null)
                    _getAdminCommand = new RelayCommand(GetAdminEvent);
                return _getAdminCommand;
            }
            set
            {
                _getAdminCommand = value;
            }
        }
        void GetAdminEvent(object param)
        {
            ListAdmin = new  ObservableCollection<UserPlayer>(superConnector.GetListAdmin(SelectedNameCompany));
        }

        ObservableCollection<UserPlayer> _listAdmin;
        public ObservableCollection<UserPlayer> ListAdmin
        {
            get
            {
                if (_listAdmin == null)
                    _listAdmin = new ObservableCollection<UserPlayer>();
                return _listAdmin;
            }
            set
            {
                if (_listAdmin == null)
                    _listAdmin = new ObservableCollection<UserPlayer>();
                _listAdmin = value;
                OnPropertyChanged();
            }
        }

        private ICommand _addAdmin;
        public ICommand AddAdmin
        {
            get
            {
                if (_addAdmin == null)
                    _addAdmin = new RelayCommand(AddAdminClick);
                return _addAdmin;
            }
            set
            {
                _addAdmin = value;
            }
        }
        void AddAdminClick(object param)
        {
            if (!String.IsNullOrEmpty(SelectedNameCompany) && !String.IsNullOrEmpty(LoginAdmin) && !String.IsNullOrEmpty(EmailAdmin))
            {
                superConnector.AddNewAdmin(SelectedNameCompany, LoginAdmin, EmailAdmin);
                ListAdmin = new ObservableCollection<UserPlayer>(superConnector.GetListAdmin(SelectedNameCompany));
            }
            else
            {
                MessageBox.Show("Введите данные!");
            }
        }
        #endregion
    }
}

