using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseUserClassDll;
using LibraryMusicContactDll;

namespace MusicPlayerOnline.Model
{
    public class ClientApp : IMusicCallBack
    {
        public ClientApp()
        {}

        private List<string> _genre;

        public List<string> Genre
        {
            get { return _genre; }
            set
            {
                _genre = value;
                OnGenrePropertyChanged();

            }
        }

        private List<string> _producer;

        public List<string> Producer
        {
            get { return _producer; }

            set
            {
                _producer = value;
                OnProducerPropertyChanged();
            }
        }

        private List<Song> _allMusic;

        public List<Song> AllMusic
        {
            get { return _allMusic; }
            set
            {
                _allMusic = value;
                OnAllMusicPropertyChanged();
            }
        }


        public void UpdateComboBoxGenre(List<string> allGenre)
        {
            if (allGenre != null)
                Genre = allGenre;
        }


        public void UpdateComboBoxProducer(List<string> allProducer)
        {
            if (allProducer != null)
                Producer = allProducer;
        }



        public event EventHandler GenrePropertyChanged;
        public void OnGenrePropertyChanged()
        {
            GenrePropertyChanged?.Invoke(this, EventArgs.Empty);

        }





        public event EventHandler ProducerPropertyChanged;
        public void OnProducerPropertyChanged()
        {
            ProducerPropertyChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler AllMusicPropertyChanged;
        public void OnAllMusicPropertyChanged()
        {
            AllMusicPropertyChanged?.Invoke(this, EventArgs.Empty);
        }




        public void UpdateAllMusicList(List<Song> allPlayList)
        {
            if (allPlayList != null)
                AllMusic = allPlayList;
        }



        private int _allMusicCount;

        public int AllMusicCount
        {
            get { return _allMusicCount; }
            set
            {
                _allMusicCount = value;
                OnAllMusicCountPropertyChanged();
            }
        }

        public event EventHandler AllMusicCountPropertyChanged;
        public void OnAllMusicCountPropertyChanged()
        {
            AllMusicCountPropertyChanged?.Invoke(this, EventArgs.Empty);
        }
        public void UpdateAllMusicCount(int countAll)
        {
            AllMusicCount = countAll;
        }




        // LEFT PANEL
        //========================================

        //count my music
        private int _myMusicCount;

        public int MyMusicCount
        {
            get { return _myMusicCount; }
            set
            {
                _myMusicCount = value;
                OnMyMusicCountPropertyChanged();
            }
        }

        public event EventHandler MyMusicCountPropertyChanged;
        public void OnMyMusicCountPropertyChanged()
        {
            MyMusicCountPropertyChanged?.Invoke(this, EventArgs.Empty);
        }


        public void UpdateMyMusicCount(int countMy)
        {
            MyMusicCount = countMy;
        }


        public void UpdateListBoxMyMusic(List<Song> myPlayList)
        {
            if (myPlayList != null)
                MyMusic = myPlayList;
        }


        private List<Song> _myMusic;

        public List<Song> MyMusic
        {
            get { return _myMusic; }
            set
            {
                _myMusic = value;
                OnMyMusicPropertyChanged();
            }
        }

        public event EventHandler MyMusicPropertyChanged;
        public void OnMyMusicPropertyChanged()
        {
            MyMusicPropertyChanged?.Invoke(this, EventArgs.Empty);
        }


        private Song _updatedSong;

        public Song UpdatedSong
        {
            get { return _updatedSong; }
            set
            {
                _updatedSong = value;
                OnUpdateCountPropertyChanged();
            }
        }

        public event EventHandler UpdateCountPropertyChanged;
        public void OnUpdateCountPropertyChanged()
        {
            UpdateCountPropertyChanged?.Invoke(this, EventArgs.Empty);
        }


        public void UpdateCount(Song song)
        {
            if (song != null)
                UpdatedSong = song;
        }

       


        private List<string> _myAlbum;

        public List<string> MyAlbum
        {
            get { return _myAlbum; }
            set
            {
                _myAlbum = value;
                OnMyAlbumPropertyChanged();
            }
        }

        public event EventHandler MyAlbumPropertyChanged;
        public void OnMyAlbumPropertyChanged()
        {
            MyAlbumPropertyChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateComboMyAlbum(List<string> albumName)
        {
            if (albumName != null)
                MyAlbum = albumName;
        }


        //Superadmin interface
        private List<string> _companyList;

        public List<string> CompanyList
        {
            get { return _companyList; }
            set
            {
                _companyList = value;
                OnCompanyPropertyChanged();
            }
        }

        public event EventHandler CompanyPropertyChanged;
        public void OnCompanyPropertyChanged()
        {
            CompanyPropertyChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateComboCompany(List<string> nameCompany)
        {
            if (nameCompany != null)
                CompanyList = nameCompany;
        }


        private List<UserPlayer> _adminList;

        public List<UserPlayer> AdminList
        {
            get { return _adminList; }
            set
            {
                _adminList = value;
                OnAdminPropertyChanged();
            }
        }

        public event EventHandler AdminPropertyChanged;
        public void OnAdminPropertyChanged()
        {
            AdminPropertyChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateComboAdmin(List<UserPlayer> adminList)
        {
            if (adminList != null)
                AdminList = adminList;
        }
    }
}
