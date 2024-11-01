using FitTrack.ViewModel;
using System.Windows;
using FitTrack.Model;
using FitTrack.Exceptions;
using FitTrack.Core;
using FitTrack.Dialogs;
using System;
using System.Data.SqlClient;

namespace FitTrack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    partial class App : Application
    {
        /// <summary>
        /// Initializes the application and determines the initial window to display based on the application's state.
        /// </summary>
        /// <param name="e">An instance of <see cref="StartupEventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// This method is called when the application starts up.
        /// </remarks>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            TestAppRunner.OnStartUp();

            if (LocalStorage.DeviceId is null || !SessionDevice.Find(LocalStorage.DeviceId))
                //Assign new registered SessionDevice               
                LocalStorage.SessionDevice = SessionDevice.Register();

            try
            {
                //Sync local device informations
                LocalStorage.SessionDevice.SyncWithCloud();

                if (LocalStorage.LoginToken is null )
                {
                    //Display SignIn View if doesn't have saved login
                    new SignInWindow()
                    { DataContext = new SignInWindowVM() }.Show();
                    return;
                }
                                
                try
                {
                    //LocalStorage has a login token
                    Account user = Account.SignIn(LocalStorage.LoginToken);
                    //Display Main View
                    new MainWindow()
                    { DataContext = new MainWindowVM(user) }.Show();
                }
                catch (InvalidLoginCreditentialException)
                {
                    //Display SignIn View if LoginToken is invalid
                    new SignInWindow()
                    { DataContext = new SignInWindowVM() }.Show();
                }
            }
            //Report any uncaught exception
            catch (AccessDeniedException exception) { MessageDialog.Show(exception.Message, "Access Denied"); }
            catch (InvalidSessionDeviceException exception) { MessageDialog.Show(exception.Message, "SessionDevice Error Occured"); }
            catch (InvalidEntityAccessException exception) { MessageDialog.Show(exception.Message, "EntityAccess Error Occured"); }
            catch (ObjectNotFoundInDatabaseException exception) { MessageDialog.Show(exception.Message, "Object Not Found"); }
            catch (SqlException exception) { MessageDialog.Show(exception.Message, "Sql Error"); }
            catch (Exception exception) { MessageBox.Show(exception.Message, "An Unhandled Error Occured"); }
        }

        /// <summary>
        /// Overrides the application exit event to inject test simulations.
        /// </summary>
        /// <param name="e">An instance of <see cref="ExitEventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// This method is called when the application exits.
        /// </remarks>
        protected override void OnExit(ExitEventArgs e)
        {
            TestAppRunner.OnExit();
            base.OnExit(e);
        }
    }
}