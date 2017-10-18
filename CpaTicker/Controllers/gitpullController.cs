using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.IO;

namespace CpaTicker.Controllers
{
    public class gitpullController : Controller
    {
        //
        // GET: /gitpull/

        public ActionResult Index()
        {
            string password = "2Joyluck1";
            System.Security.SecureString _password = new System.Security.SecureString();
            foreach (char c in password)
            {
                _password.AppendChar(c);
            }
            string path = "D:\\deploycpaticker.bat";
            ProcessStartInfo process = new ProcessStartInfo("cmd.exe");
            process.LoadUserProfile = true;
            process.UserName = "jeffcheung";
            process.Password = _password;
            process.UseShellExecute = false;
            process.CreateNoWindow = true;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.RedirectStandardError = true;
            process.WorkingDirectory = "D:\\";
            Process batchProcess = Process.Start(process);
            StreamReader stream = System.IO.File.OpenText(path);
            StreamReader outstream = batchProcess.StandardOutput;
            StreamWriter instream = batchProcess.StandardInput;
            while (stream.Peek() != -1)
            {
                instream.WriteLine(stream.ReadLine());
            }
            stream.Close();
            //instream.WriteLine(String.Format("# {0} ran successfully.  Exiting now.", path));
            instream.WriteLine("Exit");
            batchProcess.Close();
            string output = outstream.ReadToEnd().Trim();
            Response.Write("Output: " + output);
            instream.Close();
            outstream.Close();
            string parameter = Request.QueryString["payload"];
            string log = "D:\\cpaticker.com\\logs\\" + DateTime.Today.Year + Right("0" + DateTime.Today.Month, 2) + Right("0" + DateTime.Today.Day, 2) + Right("0" + DateTime.Now.Hour, 2) + Right("0" + DateTime.Now.Minute, 2) + Right("0" + DateTime.Now.Second, 2) + Right("00" + DateTime.Now.Millisecond, 3) + ".txt";
            StreamWriter logwriter = System.IO.File.CreateText(log);
            logwriter.WriteLine(parameter);
            logwriter.Close();
            return Content(output.Replace(System.Environment.NewLine, "<br>"));
        }

        public static string Right(string original, int numberCharacters)
        {
            return original.Substring(original.Length - numberCharacters);
        }

    }
}
