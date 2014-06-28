
using System;
using Sharkbite.Irc;


namespace Sharkbite.Irc.Examples
{
	public class Launcher
	{

		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				if( args.Length < 1 ) 
				{
					Console.WriteLine("Please select one of the following numbers:");
					Console.WriteLine("\t1. Botul nostru");
					Console.WriteLine("\t2. Ceva ce nu este b");
					Console.WriteLine("\t3. Advanced example with a custom parser");
					Console.WriteLine("\t4. DCC Chat example");
					Console.WriteLine("\t5. Simple DCC file server (needs directory as second arg)");
					Console.WriteLine("\t6. DCC file downloader (needs directory as second arg)");
					#if SSL
					Console.WriteLine("\t7. Secure connection example");
					#endif
					return;
				}
				int choice = int.Parse( args[0] ); 
				
				switch( choice ) 
				{
					case 1:
						Basic basic = new Basic();
						basic.start();
						break;
					case 2:
						Intermediate intermediate = new Intermediate();
						intermediate.start();
						break;
					case 3:
						Advanced  advanced = new Advanced();
						advanced.start();
						break;
					case 4:
						ChatBot chatBot = new ChatBot();
						chatBot.start();
						break;
					case 5:
						FileServer fileServer = new FileServer( args[1] );
						fileServer.start();
						break;
					case 6:
						FileClient fileClient = new FileClient( args[1] );
						fileClient.start();
						break;
					#if SSL
					case 7:
						Secure secure = new Secure();
						secure.start();
						break;
					#endif
					default:
					#if SSL
							Console.WriteLine("Please choose an example form 1 to 7.");
					#else
							Console.WriteLine("Please choose an example form 1 to 6.");
					#endif
					break;
				}
			}
			catch( FormatException fe ) 
			{
				Console.WriteLine("The first argument must be a number.");
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Unanticipated exception " + e );
			}
		}

	}
}
