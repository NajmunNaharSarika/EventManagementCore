using EventManagementCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EventManagementCore.ViewComponents
{
    public class RunningEventsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public RunningEventsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var activeCount = await _context.Events.CountAsync(p => p.IsActive);

            return View(activeCount);
        }
    }
}




