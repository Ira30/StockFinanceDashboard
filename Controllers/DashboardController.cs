using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

public class DashboardController : Controller
{
    private readonly IConfiguration _configuration;

    public DashboardController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult ExportDashboard()
    {
        string folderPath = Directory.GetCurrentDirectory();
        string exportDir = Path.Combine(folderPath, "exports");
        string zipFilePath = Path.Combine(exportDir, "dashboardExport.zip");

        if (!Directory.Exists(exportDir))
            Directory.CreateDirectory(exportDir);

        if (System.IO.File.Exists(zipFilePath))
        {
            try
            {
                System.IO.File.Delete(zipFilePath);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File is in use: {ex.Message}");
                return StatusCode(500, "Could not delete existing zip file because it is in use.");
            }
        }

        CreateZipFile(folderPath, zipFilePath);

        byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);
        return base.File(fileBytes, "application/zip", "dashboardExport.zip"); // ✅ FIXED HERE
    }

    public void CreateZipFile(string folderPath, string zipFilePath)
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        foreach (var dirPath in Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories))
        {
            if (dirPath.Contains("exports")) continue;
            Directory.CreateDirectory(dirPath.Replace(folderPath, tempDir));
        }

        foreach (var newPath in Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories))
        {
            if (newPath.Contains("exports")) continue;
            System.IO.File.Copy(newPath, newPath.Replace(folderPath, tempDir), true);
        }

        ZipFile.CreateFromDirectory(tempDir, zipFilePath);
        Directory.Delete(tempDir, true);
    }

    public void SendEmailWithAttachment(string zipFilePath, string recipientEmail)
    {
        var smtpServer = _configuration["EmailSettings:SmtpServer"];
        var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
        var senderEmail = _configuration["EmailSettings:SenderEmail"];
        var senderPassword = _configuration["EmailSettings:SenderPassword"];

        if (string.IsNullOrWhiteSpace(senderEmail))
        {
            Console.WriteLine("Sender email is not configured.");
            return;
        }

        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail),
            Subject = "Dashboard Export",
            Body = "Please find the dashboard export attached."
        };

        mailMessage.To.Add(recipientEmail);
        mailMessage.Attachments.Add(new Attachment(zipFilePath));

        var smtp = new SmtpClient(smtpServer)
        {
            Port = smtpPort,
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            EnableSsl = true
        };

        try
        {
            smtp.Send(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
}
