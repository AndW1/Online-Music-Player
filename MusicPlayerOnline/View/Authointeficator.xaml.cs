using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BaseUserClassDll;
using LibraryAuthorizationDll;

using System.ServiceModel;

namespace MusicPlayerOnline.View
{
    /// <summary>
    /// Логика взаимодействия для Authointeficator.xaml
    /// </summary>
    public partial class Authointeficator : Window
    {
        public bool OpenMainWindow { get; set; }

        public event EventHandler MainWindowPropertyChanged;

        public UserPlayer UserPlayer { get; set; }

        public event EventHandler UserPropertyChanged;

        bool closed = false;

        LoginConnector loginConnector;

        public Authointeficator()
        {
            InitializeComponent();

            CloseRegisterPanel();

            textLogin.Text = "admin1";
            textPassword.Text = "123456789";

            loginConnector = new LoginConnector();
            loginConnector.ConnectToServer();
           
        }

       

        private void windowsLogin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected void OnUserPropertyChanged()
        {
            UserPropertyChanged?.Invoke(this, EventArgs.Empty);
        }

        protected void OnMainWindowPropertyChanged()
        {
            MainWindowPropertyChanged?.Invoke(this, EventArgs.Empty);
        }

        private void windowsLogin_Closed(object sender, EventArgs e)
        {
            if (!closed)
            {
                OpenMainWindow = false;
                OnMainWindowPropertyChanged();
            }
            else
            {
                OpenMainWindow = true;
                OnMainWindowPropertyChanged();
            }

            loginConnector.CloseLogin();
        }

        private void checkRegistr_Checked(object sender, RoutedEventArgs e)
        {
            if(checkRegistr.IsChecked==true)
            {
                OpenRegisterPanel();
            }       
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            textLogin.Text = "";
            textEmail.Text = "";
            textPassword.Text = "";
            textConfirm.Text = "";
        }

        private void OpenRegisterPanel()
        {
            rowEmail.Height = new GridLength(100);
            rowConfirm.Height = new GridLength(40);
            windowsLogin.Height = 395;
            checkRegistr.IsChecked = true;
        }

        private void CloseRegisterPanel()
        {
            rowEmail.Height = new GridLength(0);
            rowConfirm.Height = new GridLength(0);
            windowsLogin.Height = 255;
        }


        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            if (checkRegistr.IsChecked == false)
            {           
                StubRequest stubRequest= loginConnector.CheckUserInDB(textLogin.Text, textPassword.Text);

                if (stubRequest.Stub == false)
                {
                    MessageBoxResult result = MessageBox.Show("Сбой соединения. Попробовать соединить?", "Music Player", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (result == MessageBoxResult.Yes)
                    {
                        buttonSend_Click(sender, e);
                    }
                    else
                      this.Close();
                }
                else if(stubRequest.Origin==false)
                {
                    MessageBox.Show("Необходимо зарегистрироваться", "Music Player");
                   // checkRegistr.IsChecked = true;
                    OpenRegisterPanel();
                    return;
                }
                else
                {
                    UserPlayer userPlayer = loginConnector.GetRgisteredUser(textLogin.Text, textPassword.Text);
                    if (userPlayer == null)
                    {
                        MessageBoxResult result = MessageBox.Show("Сбой соединения 1. Попробовать соединить?", "Music Player", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                        if (result == MessageBoxResult.Yes)
                        {
                            buttonSend_Click(sender, e);
                        }
                        else
                            this.Close();
                    }
                    else
                    {                    
                        UserPlayer = userPlayer;
                        OnUserPropertyChanged();
                        closed = true;
                        this.Close();
                    }
                }

            }

            if (checkRegistr.IsChecked == true)
            {
                UserPlayer userPlayer = loginConnector.RegisterNewUser(textLogin.Text, textEmail.Text, textPassword.Text);
                if (userPlayer != null)
                {
                    UserPlayer = userPlayer;
                    OnUserPropertyChanged();
                    checkRegistr.IsChecked = false;
                    closed = true;
                    this.Close();    
                }
                else
                {                
                    MessageBoxResult result = MessageBox.Show("Сбой соединения 2. Попробовать соединить?", "Music Player", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (result == MessageBoxResult.Yes)
                    {
                        buttonSend_Click(sender, e);
                        checkRegistr.IsChecked = false;
                        return;
                    }
                    else
                        this.Close();
                }
            }
        }

        private void buttonGetPassword_Click(object sender, RoutedEventArgs e)
        {
            StubRequest requstEmail = loginConnector.CheckEmailInBuffer(textEmail.Text);

            if(requstEmail.Origin==false && requstEmail.Stub==false)
            {
                MessageBoxResult result = MessageBox.Show("Сбой соединения 3. Попробовать соединить?", "Music Player", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    buttonGetPassword_Click(sender, e);
                }
                else
                    this.Close();
            }

            else if(requstEmail.Origin == true && requstEmail.Stub == false)
            {
                MessageBox.Show(String.Format("Используйте пароль с Вашей почты от {0}", requstEmail.Date.ToShortDateString()));
            }

            else
            {
                StubRequest stubRequest = loginConnector.CheckEmail(textEmail.Text);

                if (stubRequest.Stub == false)
                {
                    MessageBoxResult result = MessageBox.Show("Сбой соединения 4.Пароль не сформирован. Попробовать соединить?", "Music Player", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (result == MessageBoxResult.Yes)
                    {
                        buttonGetPassword_Click(sender, e);
                    }
                    else
                        this.Close();
                }
                else if (stubRequest.Origin == true)
                {
                    MessageBox.Show("Войдите под своей учетной записью", "Music Player");
                    checkRegistr.IsChecked = false;
                    CloseRegisterPanel();
                }

                else
                {
                    bool OK = loginConnector.CreateNewPassword(textLogin.Text, textEmail.Text);
                    if (OK)
                        MessageBox.Show("Пароль сформирован и выслан на Ваш Email");
                }
            }
           
        }
    }
}
