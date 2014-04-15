using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

namespace HipCommit
{
	class Program
	{
		/// <summary>
		/// Main program flow
		/// </summary>
		static void Main(string[] args)
		{
			string json;

			// Build a request with the Hipchat room URL

			var request = (HttpWebRequest)WebRequest.Create(
				Properties.Settings.Default.HipChatRoomUrl
			);

			request.ContentType = "application/json";
			request.Method = "POST";

			// The SVN hook passes us a revision number through
			// the program args, so we can use this to get the 
			// commit message and other details from assembla

			using (var sw = new StreamWriter(request.GetRequestStream()))
			{
				var jsonObj = new
				{
					message = GetCommitMessage(
						args[Properties.Settings.Default.SVNRevisionArg]
					),
					notify = true
				};

				json = JsonConvert.SerializeObject(jsonObj);

				sw.Write(json);
			}

			try
			{
				var response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException)
			{
				Console.Error.WriteLine(json);
				Environment.Exit(1);
			}

		}

		/// <summary>
		/// Get the commit message for the supplied revision number
		/// using the svn command line tool
		/// </summary>
		/// <param name="revision">The revision number to get the message for</param>
		/// <returns>The commit message</returns>
		static string GetCommitMessage(string revision)
		{
			// Build the svn log command
			string svnCommand = String.Format("svn log -r {0} {1}",
									revision,
									Properties.Settings.Default.AssemblaRepoUrl);

			ProcessStartInfo psi = new ProcessStartInfo("cmd", 
											String.Format("/c {0}", svnCommand));

			psi.CreateNoWindow = true;
			psi.RedirectStandardOutput = true;
			psi.UseShellExecute = false;

			Process p = new Process();
			p.StartInfo = psi;

			p.Start();

			string message = "";
			string line = p.StandardOutput.ReadLine();

			while (line != null)
			{
				message += line + "<br />";

				line = p.StandardOutput.ReadLine();
			}

			return AddTicketLink(message);
		}

		/// <summary>
		/// Extract #[id] ticket ids and add URL to assembla into
		/// the html message
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		static string AddTicketLink(string message)
		{
			// Replace all instances of #[number] with <a> tag
			return Regex.Replace(message, @"#{1}\d+", delegate(Match match) {

				string m = match.ToString();

				return String.Format("<a href=\"{0}\">{1}</a>",
								Properties.Settings.Default.AssemblaTicketsUrl + m.Substring(1),
								m);

			});
		}
	}
}
