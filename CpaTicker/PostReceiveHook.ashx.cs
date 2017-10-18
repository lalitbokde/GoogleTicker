using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;

namespace CpaTicker
{
    /// <summary>
    /// Summary description for PostReceiveHook
    /// </summary>
    public class PostReceiveHook : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string response = Deploygit();
            context.Response.Write("deployment -->" + response);
            string response1 = Deploy();

            context.Response.Write("deployment -->" + response1);


        }

        private string Deploygit()
        {
            string res = "";
            try
            {
                ExecuteCommandSync(string.Format(@"cd {0} && git reset --hard dev && git pull", ""));
                res = "git successfully" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                throw;
            }

            return res;
        }

        private string Deploy()
        {
            string res1 = "";
            try
            {
                res1 = ExecuteCommandSyncDeploy(string.Format(@"cd {0} && git reset --hard dev && git pull", ""));
                // res1 = "build success";
            }
            catch (Exception ex)
            {
                throw;
            }
            return res1;
        }

        public void ExecuteCommandSync(object command)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo psi =
            new System.Diagnostics.ProcessStartInfo(@"D:\batch\dev_autoupdate_git.bat");
                psi.RedirectStandardOutput = true;
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                psi.UseShellExecute = false;
                System.Diagnostics.Process listFiles;
                listFiles = System.Diagnostics.Process.Start(psi);
                System.IO.StreamReader myOutput = listFiles.StandardOutput;
                listFiles.WaitForExit();

                if (listFiles.HasExited == true)
                {
                    string output = myOutput.ReadToEnd();
                    string re = output;
                }
            }
            catch
            {
                throw;
            }


        }

        public void ExecuteCommandAsync(string command)
        {
            try
            {
                //Asynchronously start the Thread to process the Execute command request.
                var thread = new Thread(new ParameterizedThreadStart(ExecuteCommandSync));
                //Make the thread as background thread.
                thread.IsBackground = true;
                //Set the Priority of the thread.
                thread.Priority = ThreadPriority.AboveNormal;
                //Start the thread.
                thread.Start(command);

                // thread.Abort();
            }
            catch (ThreadStartException exp)
            {
                // Log the exception
            }
            catch (ThreadAbortException exp)
            {
                // Log the exception
            }
            catch (Exception exp)
            {
                // Log the exception
            }
        }


        public string ExecuteCommandSyncDeploy(object command)
        {
            string output = "";
            try
            {
                System.Diagnostics.ProcessStartInfo psi =
            new System.Diagnostics.ProcessStartInfo(@"D:\batch\devdeploy.bat");
                psi.RedirectStandardOutput = true;
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                psi.UseShellExecute = false;
                System.Diagnostics.Process listFiles;
                listFiles = System.Diagnostics.Process.Start(psi);
                System.IO.StreamReader myOutput = listFiles.StandardOutput;
                listFiles.WaitForExit(120000);

                //if (listFiles.HasExited == true)
                //{
                output = myOutput.ReadToEnd();
                string re = output;
                // }
            }
            catch
            {
                throw;
            }

            return output;

        }
        //public void ExecuteCommandAsyncDeploy(string command)
        //{
        //    try
        //    {
        //        //Asynchronously start the Thread to process the Execute command request.
        //        var thread = new Thread(new ParameterizedThreadStart(ExecuteCommandSyncDeploy));
        //        //Make the thread as background thread.
        //        thread.IsBackground = true;
        //        //Set the Priority of the thread.
        //        thread.Priority = ThreadPriority.Highest;
        //        //Start the thread.
        //        thread.Start(command);

        //        // thread.Abort();
        //    }
        //    catch (ThreadStartException exp)
        //    {
        //        // Log the exception
        //    }
        //    catch (ThreadAbortException exp)
        //    {
        //        // Log the exception
        //    }
        //    catch (Exception exp)
        //    {
        //        // Log the exception
        //    }
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}