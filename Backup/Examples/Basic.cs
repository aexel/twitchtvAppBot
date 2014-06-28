
using System;
using Sharkbite.Irc;
using keybound;
using System.Collections.Generic;
//using System.Windows.Forms;

namespace Sharkbite.Irc.Examples
{

	/// <summary>
	/// A basic example which simply echos all
	/// public messages sent to a channel.
	/// It is designed to demonstrate how to connect to an IRC
	/// server and how to register event handlers.
	/// </summary>
	public class Basic
	{

		/// <summary>
		/// The connection object is the focal point of the library.
		/// It used to retrieve references to the various library components.
		/// </summary>
	    private Connection connection;
        private WindowHook Pwindow;
        List<string> team_red = new List<string>();
        List<string> team_blue = new List<string>();
        
		/// <summary>
		/// Create a bot and register its handlers.
		/// </summary>
		public Basic() 
		{
            Pwindow = new WindowHook();
			CreateConnection();

			//OnRegister tells us that we have successfully established a connection with
			//the server. Once this is established we can join channels, check for people
			//online, or whatever.
			connection.Listener.OnRegistered += new RegisteredEventHandler( OnRegistered );
						
			//Listen for any messages sent to the channel
			connection.Listener.OnPublic += new PublicMessageEventHandler( OnPublic );

			//Listen for bot commands sent as private messages
			connection.Listener.OnPrivate += new PrivateMessageEventHandler( OnPrivate );
	
			//Listen for notification that an error has ocurred 
			connection.Listener.OnError += new ErrorMessageEventHandler( OnError );

			//Listen for notification that we are no longer connected.
			connection.Listener.OnDisconnected += new DisconnectedEventHandler( OnDisconnected );

            //Listen for notification that a user has left.
            connection.Listener.OnQuit += new QuitEventHandler( OnQuit );

            //Listen for notification that we are no longer connected.
            connection.Listener.OnPart += new PartEventHandler( OnPart );

            //Listen for notification that we are no longer connected.
            connection.Listener.OnKick += new KickEventHandler(OnKick);

            //Listen for notification that we are no longer connected.
            connection.Listener.OnKill += new KillEventHandler(OnKill);
		}

		private void CreateConnection() 
		{
				//The hostname of the IRC server
				string server = "irc.twitch.tv";
                
                //oauth for twitch
                string oauth = "";

				//The bot's nick on IRC
				string nick = "";

				//Fire up the Ident server for those IRC networks
				//silly enough to use it.
				Identd.Start( nick );

				//A ConnectionArgs contains all the info we need to establish
				//our connection with the IRC server and register our bot.
				//This line uses the simplfied contructor and the default values.
				//With this constructor the Nick, Real Name, and User name are
				//all set to the same value. It will use the default port of 6667 and no server
				//password.
				ConnectionArgs cargs = new ConnectionArgs(nick, server);
                cargs.ServerPassword = oauth;
		
				//When creating a Connection two additional protocols may be
				//enabled: CTCP and DCC. In this example we will disable them
				//both.
				connection = new Connection( cargs, false, false );			

			//NOTE
			//We could have created multiple Connections to different IRC servers
			//and each would process messages simultaneously and independently.
			//There is no fixed limit on how many Connection can be opened at one time but
			//it is important to realize that each runs in its own Thread. Also,  separate event 
			//handlers are required for each connection, i.e. the
			//same OnRegistered() handler cannot be used for different connection
			//instances.
		}

		public void start() 
		{
			//Notice that by having the actual connect call here 
			//the constructor can add the necessary listeners before 
			//the connection process begins. If listeners are added
			//after connecting they may miss certain events. the OnRegistered()
			//event will certainly be missed.

			try
			{			
				//Calling Connect() will cause the Connection object to open a 
				//socket to the IRC server and to spawn its own thread. In this
				//separate thread it will listen for messages and send them to the 
				//Listener for processing.
				connection.Connect();

				Console.WriteLine("bot connected.");
				//The main thread ends here but the Connection's thread is still alive.
				//We are now in a passive mode waiting for events to arrive.
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Error during connection process.");
				Console.WriteLine( e );
				Identd.Stop();
			}
		}
		

		public void OnRegistered() 
		{
			//We have to catch errors in our delegates because Thresher purposefully
			//does not handle them for us. Exceptions will cause the library to exit if they are not
			//caught.
			try
			{ 
				//Don't need this anymore in this example but this can be left running
				//if you want.
				Identd.Stop();

				//The connection is ready so lets join a channel.
				//We can join any number of channels simultaneously but
				//one will do for now.
				//All commands are sent to IRC using the Sender object
				//from the Connection.
                connection.Sender.Join("#twitchplaysmk");
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Error in OnRegistered(): " + e ) ;
			}
		}

		public void OnPublic( UserInfo user, string channel, string message )
		{

            int team;
			//Echo back any public messages
			//connection.Sender.PublicMessage( channel,  user.Nick + " said, " + message );
            //message;
            //Console.WriteLine(message);
            //Pwindow.sendKeystroke(message);
            //red = 1; blue = 2

            team = checkTeam(user);

            switch (message.ToLower())
            {
                case "up":
                    if (team == 1)
                    {
                        Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_W);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(user.Nick + "  UP");
                        Console.ResetColor();
                    }
                    else
                    {
                        Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_U);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(user.Nick + "  UP");
                        Console.ResetColor();
                    }
                break;

                case "down":
                if (team == 1)
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_S);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(user.Nick + "  DOWN");
                    Console.ResetColor();
                }
                else
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_J);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(user.Nick + "  DOWN");
                    Console.ResetColor();
                }
                break;

                case "left":
                if (team == 1)
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_A);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(user.Nick + "  LEFT");
                    Console.ResetColor();
                }
                else
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_H);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(user.Nick + "  LEFT");
                    Console.ResetColor();
                }
                break;

                case "right":
                if (team == 1)
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_D);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(user.Nick + "  RIGHT");
                    Console.ResetColor();
                }
                else
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_K);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(user.Nick + "  RIGHT");
                    Console.ResetColor();
                }
                break;

                case "lpunch":
                if (team == 1)
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_R);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(user.Nick + "  LPUNCH");
                    Console.ResetColor();
                }
                else
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_O);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(user.Nick + "  LPUNCH");
                    Console.ResetColor();
                }
                break;

                case "hpunch":
                if (team == 1)
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_T);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(user.Nick + "  HPUNCH");
                    Console.ResetColor();
                }
                else
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_P);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(user.Nick + "  HPUNCH");
                    Console.ResetColor();
                }
                break;

                case "lkick":
                if (team == 1)
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_F);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(user.Nick + "  LKICK");
                    Console.ResetColor();
                }
                else
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_L);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(user.Nick + "  LKICK");
                    Console.ResetColor();
                }
                break;

                case "hkick":
                if (team == 1)
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_G);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(user.Nick + "  HKICK");
                    Console.ResetColor();
                }
                else
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_OEM_1);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(user.Nick + "  HKICK");
                    Console.ResetColor();
                }
                break;

                case "block":
                if (team == 1)
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_E);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(user.Nick + "  BLOCK");
                    Console.ResetColor();
                }
                else
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_I);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(user.Nick + "  BLOCK");
                    Console.ResetColor();
                }
                break;

                case "throw":
                if (team == 1)
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_Y);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(user.Nick + "  THROW");
                    Console.ResetColor();
                }
                else
                {
                    Pwindow.sendKeystroke(keybound.WindowHook.VirtualKeyStates.VK_Q);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(user.Nick + "  THROW");
                    Console.ResetColor();
                }
                break;
                default:
                break;

            }
		}

        private void removePlayer_red(string username)
        {
            for (int i = team_red.Count - 1; i >= 0; i--)
            {
                if (team_red[i].Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    team_red.RemoveAt(i);
                }
            }
        }


        private void removePlayer_blue(string username)
        {
            for (int i = team_blue.Count - 1; i >= 0; i--)
            {
                if (team_blue[i].Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    team_blue.RemoveAt(i);
                }
            }
        }

        private int checkTeam(UserInfo user)
        {

            foreach (string nick in team_red)
            {
                if (nick.Equals(user.Nick, StringComparison.OrdinalIgnoreCase))
                {
                    return 1;
                }
            }

            foreach (string nick in team_blue)
            {
                if (nick.Equals(user.Nick, StringComparison.OrdinalIgnoreCase))
                {
                    return 2;
                }
            }

            if (team_blue.Count < team_red.Count)
            {
                team_blue.Add(user.Nick);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Player " + user.Nick + " joined blue team\n");
                Console.ResetColor();
                return 2;
            }
            else
            {
                team_red.Add(user.Nick);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Player " + user.Nick + " joined red team\n");
                Console.ResetColor();
                return 1;
            }
        }

        public void removePlayer(string username)
        {

            for (int i = team_red.Count - 1; i >= 0; i--)
            {
                if (team_red[i].Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    team_red.RemoveAt(i);
                }
            }

            for (int i = team_blue.Count - 1; i >= 0; i--)
            {
                if (team_blue[i].Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    team_blue.RemoveAt(i);
                }
            }
        }

		public void OnPrivate( UserInfo user,  string message )
		{
			//Quit IRC if someone sends us a 'die' message
			if( message == "die" ) 
			{
				connection.Disconnect("Bye");
			}
		}

		public void OnError( ReplyCode code, string message) 
		{
			//All anticipated errors have a numeric code. The custom Thresher ones start at 1000 and
			//can be found in the ErrorCodes class. All the others are determined by the IRC spec
			//and can be found in RFC2812Codes.
			Console.WriteLine("An error of type " + code + " due to " + message + " has occurred.");
		}

		public void OnDisconnected() 
		{
			//If this disconnection was involutary then you should have received an error
			//message ( from OnError() ) before this was called.
			Console.WriteLine("Connection to the server has been closed.");
		}

        public void OnQuit(UserInfo user, string reason)
        {
            //If this disconnection was involutary then you should have received an error
            //message ( from OnError() ) before this was called.
            Console.WriteLine(user.Nick + "has quit.");
            removePlayer(user.Nick);
        }

        public void OnPart(UserInfo user, string channel, string reason)
        {
            //If this disconnection was involutary then you should have received an error
            //message ( from OnError() ) before this was called.
            Console.WriteLine(user.Nick + "has parted.");
            removePlayer(user.Nick);
        }

        public void OnKill(UserInfo user, string username, string reason)
        {
            //If this disconnection was involutary then you should have received an error
            //message ( from OnError() ) before this was called.
            Console.WriteLine(user.Nick + "has parted.");
            removePlayer(user.Nick);
        }


        public void OnKick(UserInfo user, string channel, string kickee, string reason)
        {
            //If this disconnection was involutary then you should have received an error
            //message ( from OnError() ) before this was called.
            Console.WriteLine(user.Nick + "has parted.");
            removePlayer(user.Nick);
        }
	
	}
}
