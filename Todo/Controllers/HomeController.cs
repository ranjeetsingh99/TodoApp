using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext taskDb;

    public HomeController(ILogger<HomeController> logger, AppDbContext taskDb)
    {

        _logger = logger;
        this.taskDb = taskDb;
    }

    public async Task<IActionResult> Index()
    {
        var vm = new HomeIndexViewModel();
        vm.Tasks =await taskDb.Tasks.ToListAsync();
        //var tasks = await taskDb.Tasks.ToListAsync();
        return View(vm);
    }

    public async Task<IActionResult> UpdateStatus(int id)
    {
        var task = await taskDb.Tasks.FindAsync(id);
        if (task == null)
            return NotFound();

        if (task.IsCompleted)
        {
            task.IsCompleted = false;
        }
        else
        {
            task.IsCompleted = true;
        }
        task.UpdatedAt = DateTime.Now;
        task.taskPrint();
        await taskDb.SaveChangesAsync();

        return RedirectToAction("Index"); 
    }
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> CreateTask(HomeIndexViewModel task)
    {
        Console.WriteLine("Enter in createtask>..........................................");

        if (ModelState.IsValid)
        {
            await taskDb.AddAsync(task.NewTask);
            await taskDb.SaveChangesAsync();
        }
        return RedirectToAction("Index");
        
       
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        //Console.WriteLine("task id ............................."+id);
         var task =await taskDb.Tasks.FindAsync(id);
        
        //task.taskPrint();
        if (task == null)
        {
            //Console.WriteLine("task is null>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            return NotFound();
        }
        
        return View(task);
    }
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Edit(int taskId, TaskItem task)
    {
        //Console.WriteLine("Enter edit post --------------------------------"+taskId+ModelState.IsValid);
        task.taskPrint();
        if(taskId != task.TaskId)
        {
            return RedirectToAction("Index");
        }
        TaskItem? finalEdit = await taskDb.Tasks.FindAsync(task.TaskId);


        if (ModelState.IsValid) {
            finalEdit!.UpdatedAt = DateTime.Now;
            finalEdit!.Description = task.Description;
            //task.taskPrint();
            taskDb.Tasks.Update(finalEdit);
            await taskDb.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(task);
    }
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> DeleteTask(int taskId)
    {

        Console.WriteLine("Enter delete post...............");
        var taskToBeDeleted = await taskDb.Tasks.FindAsync(taskId);

        if(taskToBeDeleted == null)
        {
            Console.WriteLine("task to be deleted id ");
            return RedirectToAction("Index");
        }


        taskToBeDeleted!.taskPrint();
        taskDb.Tasks.Remove(taskToBeDeleted);
        await taskDb.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
