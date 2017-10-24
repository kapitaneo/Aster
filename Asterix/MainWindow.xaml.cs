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
using System.Windows.Navigation;
using System.Windows.Shapes;
//using AsterNET.Manager;
//using AsterNET.Manager.Event;
using Asterisk.NET;
using Asterisk.NET.Manager;
using Asterisk.NET.Manager.Event;

namespace Asterix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ManagerConnection astCon = null;
        NewCallerIdEvent manager6;
        public MainWindow()
        {
            InitializeComponent();
            //astCon = new ManagerConnection("193.93.187.217", 5060, "901", "9d49c145849b09428e2e03a58477fa2f");
            astCon = new ManagerConnection("sip2sip.info", 5060, "cnetamconsoft", "fuckyou1987");
            //astCon.PingInterval = 10000;
            //astCon.PingInterval = 0;
            astCon.LogChannel += AstCon_LogChannel; 
            //astCon.FireAllEvents = true;
            //astCon.
                astCon.NewChannel += manager_NewChannel;
                astCon.NewState += AstCon_NewState;
                astCon.NewCallerId += AstCon_NewCallerId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //astCon = new ManagerConnection("193.93.187.217", 5060, "902", "861163e0f5989dba4149faf3265ea8ad");
            try
            {
                // Uncomment next 2 line comments to Disable timeout (debug mode)
                astCon.DefaultResponseTimeout = 5000;
                astCon.DefaultEventTimeout = 5000;

                astCon.Login();
            }
            catch (Asterisk.NET.Manager.TimeoutException ex)
            {
                MessageBox.Show("Error connect\n" + ex.Message);
                astCon.Logoff();
                //	this.Close();
            }
        }

        private void AstCon_LogChannel(object sender, LogChannelEvent e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Aster : {0} {1} @ {2}", e.DateReceived, e.Server, e.Channel));
        }

        private void AstCon_NewState(object sender, NewStateEvent e)
        {
            if (e.ChannelStateDesc != null)
            {
                if (
                    e.Channel.ToUpper()
                        .StartsWith("SIP"))
                {
                    // TODO: This could be the result of our Originate! If so, we don't want to do anything... or do we?
                    // this event related to me
                    switch (e.ChannelStateDesc.ToLower())
                    {
                        case "ringing":
                            { 
                                MessageBox.Show($"Incoming call from {e.CallerIdNum} ({e.Channel})");
                            }
                            break;
                    }
                }
            }
        }

        private void AstCon_NewCallerId(object sender, NewCallerIdEvent e)
        {
            MessageBox.Show(e.CallerIdName);
        }

        private String curChannel;
        void manager_NewChannel(object sender, NewChannelEvent e)
        {
            //if (e.CallerIdNum != userrow.number)
            //    return;

            curChannel = e.Channel;

        }
    }
}
