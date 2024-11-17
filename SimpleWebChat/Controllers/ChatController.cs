using Microsoft.AspNetCore.Mvc;
using QRCoder;
using SimpleWebChat.Models;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using static System.Net.WebRequestMethods;

using Microsoft.AspNetCore.SignalR;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
        services.AddSignalR();  // Add SignalR services
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // Map the /chatHub URL to the ChatHub class
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapHub<ChatHub>("/chatHub");  // Map SignalR Hub
        });
    }
}

public class ChatHub : Hub
    {
        // Send message to all clients connected to the Hub
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }


public class ChatController : Controller
{
    private static List<ChatMessage> Messages = new List<ChatMessage>();

    public IActionResult Index()
    {
        // Generate the QR code for the current URL
        string url = $"{Request.Scheme}://{Request.Host}{Request.Path}";
        ViewBag.QRCodeImage = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data="+ url;

        return View(Messages);
    }

    [HttpPost]
    public IActionResult SendMessage(string userName, string message)
    {
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(message))
        {
            Messages.Add(new ChatMessage
            {
                UserName = userName,
                Message = message,
                Timestamp = DateTime.Now
            });
        }
        return RedirectToAction("Index");
    }

}