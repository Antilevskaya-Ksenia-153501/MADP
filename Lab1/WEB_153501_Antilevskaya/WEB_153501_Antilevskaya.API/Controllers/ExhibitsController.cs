using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WEB_153501_Antilevskaya.API.Data;
using WEB_153501_Antilevskaya.API.Services.ExhibitService;
using WEB_153501_Antilevskaya.Domain.Models;
using WEB_153501_Antilevskaya.Domain.Entities;

namespace WEB_153501_Antilevskaya.API.Controllers;

[Route("api/exhibits")]
[ApiController]
public class ExhibitsController : Controller
{
    private readonly IExhibitService _exhibitService;

    public ExhibitsController(IExhibitService exhibitService)
    {
        _exhibitService = exhibitService;
    }

    [HttpGet("{category?}/{pageNo?}")]
    public async Task<ActionResult<ResponseData<List<Exhibit>>>> GetExhibits(string? category = null ,int pageNo = 1,int pageSize = 3)
    {
        if (int.TryParse(category, out int parsedPageNo))
        {
            pageNo = parsedPageNo;
            category = null;
        }
        return Ok(await _exhibitService.GetExhibitListAsync(category, pageNo, pageSize));
    }

    [HttpGet("get/{id:int}")]
    public async Task<ActionResult<ResponseData<Exhibit>>> GetExhibitById(int id)
    {
        return Ok(await _exhibitService.GetExhibitByIdAsync(id));
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<ActionResult<ResponseData<Exhibit>>> DeleteExhibitById(int id)
    {
        try
        {
            await _exhibitService.TaskDeleteExhibitAsync(id);
        }
        catch (Exception ex)
        {
            return NotFound(new ResponseData<Exhibit>()
            {
                Data = null,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
        return NoContent();
    }

    [HttpPut("update/{id:int}")]
    public async Task<ActionResult<ResponseData<Exhibit>>> UpdateExhibit(int id, Exhibit newExhibit)
    {
        try
        {
            _exhibitService.UpdateExhibitAsync(id, newExhibit);
        }
        catch(Exception ex)
        {
            return new ResponseData<Exhibit>()
            {
                Success = false,
                ErrorMessage = ex.Message,
                Data = null
            };
        }
        return Ok(new ResponseData<Exhibit>()
        {
            Data = newExhibit
        });
    }

    [HttpPost("create/")]
    public async Task<ActionResult<ResponseData<Exhibit>>> CreateExhibit(Exhibit exhibit)
    {
        if (exhibit is null)
        {
            return BadRequest(new ResponseData<Exhibit>()
            {
                Success = false,
                ErrorMessage = "Exhibit is null",
                Data = null
            });
        }
        var response = await _exhibitService.CreateExhibitAsync(exhibit);
        if (!response.Success)
        {
            return BadRequest(response.ErrorMessage);
        }

        return CreatedAtAction("GetExhibitById", new { id = exhibit.Id }, new ResponseData<Exhibit>()
        {
            Data = exhibit
        });
    }

    [HttpPost("image/{id:int}")]
    public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
    {
        var response = await _exhibitService.SaveImageAsync(id, formFile);
        if (response.Success)
        {
            return Ok(response);
        }
        return NotFound(response);
    }
}






//// GET: Exhibits
//public async Task<IActionResult> Index()
//{
//      return _context.Exhibit != null ? 
//                  View(await _context.Exhibit.ToListAsync()) :
//                  Problem("Entity set 'AppDbContext.Exhibit'  is null.");
//}

//// GET: Exhibits/Details/5
//public async Task<IActionResult> Details(int? id)
//{
//    if (id == null || _context.Exhibit == null)
//    {
//        return NotFound();
//    }

//    var exhibit = await _context.Exhibit
//        .FirstOrDefaultAsync(m => m.Id == id);
//    if (exhibit == null)
//    {
//        return NotFound();
//    }

//    return View(exhibit);
//}

//// GET: Exhibits/Create
//public IActionResult Create()
//{
//    return View();
//}

//// POST: Exhibits/Create
//// To protect from overposting attacks, enable the specific properties you want to bind to.
//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> Create([Bind("Id,Title,Description,Price,Image")] Exhibit exhibit)
//{
//    if (ModelState.IsValid)
//    {
//        _context.Add(exhibit);
//        await _context.SaveChangesAsync();
//        return RedirectToAction(nameof(Index));
//    }
//    return View(exhibit);
//}

//// GET: Exhibits/Edit/5
//public async Task<IActionResult> Edit(int? id)
//{
//    if (id == null || _context.Exhibit == null)
//    {
//        return NotFound();
//    }

//    var exhibit = await _context.Exhibit.FindAsync(id);
//    if (exhibit == null)
//    {
//        return NotFound();
//    }
//    return View(exhibit);
//}

//// POST: Exhibits/Edit/5
//// To protect from overposting attacks, enable the specific properties you want to bind to.
//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,Image")] Exhibit exhibit)
//{
//    if (id != exhibit.Id)
//    {
//        return NotFound();
//    }

//    if (ModelState.IsValid)
//    {
//        try
//        {
//            _context.Update(exhibit);
//            await _context.SaveChangesAsync();
//        }
//        catch (DbUpdateConcurrencyException)
//        {
//            if (!ExhibitExists(exhibit.Id))
//            {
//                return NotFound();
//            }
//            else
//            {
//                throw;
//            }
//        }
//        return RedirectToAction(nameof(Index));
//    }
//    return View(exhibit);
//}

//// GET: Exhibits/Delete/5
//public async Task<IActionResult> Delete(int? id)
//{
//    if (id == null || _context.Exhibit == null)
//    {
//        return NotFound();
//    }

//    var exhibit = await _context.Exhibit
//        .FirstOrDefaultAsync(m => m.Id == id);
//    if (exhibit == null)
//    {
//        return NotFound();
//    }

//    return View(exhibit);
//}

//// POST: Exhibits/Delete/5
//[HttpPost, ActionName("Delete")]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> DeleteConfirmed(int id)
//{
//    if (_context.Exhibit == null)
//    {
//        return Problem("Entity set 'AppDbContext.Exhibit'  is null.");
//    }
//    var exhibit = await _context.Exhibit.FindAsync(id);
//    if (exhibit != null)
//    {
//        _context.Exhibit.Remove(exhibit);
//    }

//    await _context.SaveChangesAsync();
//    return RedirectToAction(nameof(Index));
//}

//private bool ExhibitExists(int id)
//{
//  return (_context.Exhibit?.Any(e => e.Id == id)).GetValueOrDefault();
//}

