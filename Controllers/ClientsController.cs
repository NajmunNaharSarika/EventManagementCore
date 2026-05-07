using EventManagementCore.Data;
using EventManagementCore.Models;
using EventManagementCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventManagementCore.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _he;
        public ClientsController(ApplicationDbContext _context, IWebHostEnvironment _he)
        {
            this._context = _context;
            this._he = _he;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Client.Include(x=>x.EventServices).ThenInclude(y=>y.Event).ToListAsync());
        }
        public IActionResult AddNewEvent(int? id)
        {
            ViewBag.events = new SelectList(_context.Events, "EventId", "EventName", id.ToString() ?? "");
            return PartialView("_addNewEvent");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        

        [HttpPost]
        public async Task<IActionResult> Create(ClientVM clientVM, int[] eventId)   
        {
            if (ModelState.IsValid)
            {
                Client client = new Client()
                {
                    ClientName = clientVM.ClientName,
                    BirthDate = clientVM.BirthDate,
                    Age = clientVM.Age,
                    MaritalStatus = clientVM.MaritalStatus,
                };

                //For Image
                var file = clientVM.ImageFile;
                string webroot = _he.WebRootPath;
                string folder = "Images";
                string ext = Path.GetExtension(clientVM.ImageFile.FileName);
                string imgFileName = Path.GetRandomFileName() + ext;
                string fileSave = Path.Combine(webroot, folder, imgFileName);

                if (file != null)
                {
                    using (var stream = new FileStream(fileSave, FileMode.Create))
                    {
                        await clientVM.ImageFile.CopyToAsync(stream);
                        clientVM.Image = "/" + folder + "/" + imgFileName;
                        client.Image = clientVM.Image;
                    }
                }
                _context.Add(client);

                //For event
                foreach (var item in eventId)
                {
                    EventService eventService = new EventService()
                    {
                        Client = client,
                        ClientId = client.ClientId,
                        EventId = item
                    };
                    _context.EventServices.Add(eventService);
                }
                await _context.SaveChangesAsync();
                return PartialView("_success");
            }
            return PartialView("_error");
        }
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            var client = await _context.Client.FirstOrDefaultAsync(x => x.ClientId == id);
            ClientVM clientVM = new ClientVM()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                BirthDate = client.BirthDate,
                Age = client.Age,
                Image = client.Image,
                MaritalStatus = client.MaritalStatus
            };
            //Event
            var existingEvent = _context.EventServices.Where(x => x.ClientId == id).ToList();
            foreach (var item in existingEvent)
            {
                clientVM.EventList.Add(item.EventId);
            }
            return View(clientVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientVM clientVM, int[] eventId)
        {
   
            eventId = eventId.Where(x => x != 0).ToArray();

            if (ModelState.IsValid)
            {
           
                var client = await _context.Client.FirstOrDefaultAsync(x => x.ClientId == clientVM.ClientId);

                if (client == null) return PartialView("_error");

                
                client.ClientName = clientVM.ClientName;
                client.BirthDate = clientVM.BirthDate;
                client.Age = clientVM.Age;
                client.MaritalStatus = clientVM.MaritalStatus;

                //  Image
                var file = clientVM.ImageFile;
                if (file != null)
                {
                    string webroot = _he.WebRootPath;
                    string folder = "Images";
                    string ext = Path.GetExtension(file.FileName);
                    string imgFileName = Path.GetRandomFileName() + ext;
                    string fileSave = Path.Combine(webroot, folder, imgFileName);

                    using (var stream = new FileStream(fileSave, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        client.Image = "/" + folder + "/" + imgFileName;
                    }
                }
               

                var existEvent = _context.EventServices
                                    .Where(x => x.ClientId == client.ClientId).ToList();
                foreach (var item in existEvent)
                {
                    _context.EventServices.Remove(item);
                }

           
                foreach (var item in eventId)
                {
                    _context.EventServices.Add(new EventService()
                    {
                        ClientId = client.ClientId,
                        EventId = item
                    });
                }

                await _context.SaveChangesAsync();
                return PartialView("_success");
            }
            return PartialView("_error");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            var client = await _context.Client.FirstOrDefaultAsync(x => x.ClientId == id);
            ClientVM clientVM = new ClientVM()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                BirthDate = client.BirthDate,
                Age = client.Age,
                Image = client.Image,
                MaritalStatus = client.MaritalStatus
            };
            //Event
            var existingEvent = _context.EventServices.Where(x => x.ClientId == id).ToList();
            foreach (var item in existingEvent)
            {
                clientVM.EventList.Add(item.EventId);
            }
            return View(clientVM);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ActionName(name: "Delete")]
        public async Task<IActionResult> Deleted(int? id)
        {
            var evt= await _context.Client.FirstOrDefaultAsync(x => x.ClientId == id);
            var exitEvent = _context.EventServices.Where(x => x.ClientId == id).ToList();
            foreach(var item in exitEvent)
            {
                _context.EventServices.Remove(item);
            }
            _context.Remove(evt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
