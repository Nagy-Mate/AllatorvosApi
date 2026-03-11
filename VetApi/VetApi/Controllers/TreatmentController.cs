using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetApi.Core;

namespace VetApi.Controllers;

[Route("[controller]")]
[ApiController]
public class TreatmentController(VetDbContext db) : ControllerBase
{
    [HttpGet]
    public ActionResult GetTreatments()
    {

        return Ok(db.Treatment.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult GetTreatments(int id)
    {
        var res = db.Treatment.FirstOrDefault(t => t.Id == id);
        if (res == null) return NotFound();

        return Ok(res);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTreatment(int id)
    {
        var res = db.Treatment.FirstOrDefault(t => t.Id == id);
        if (res == null) return NotFound();

        db.Treatment.Remove(res);
        db.SaveChanges();
        return NoContent();
    }

    [HttpPost]
    public ActionResult CreateTreatment([FromBody] TreatmentDto treatment)
    {
        try
        {
            db.Treatment.Add(new Treatment 
            { 
                Name = treatment.Name, 
                EndDate = treatment.EndDate, 
                StartDate = treatment.StartDate, 
                Price = treatment.Price,  
                Type = treatment.Type
            });

            db.SaveChanges();
            return Created();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut]
    public ActionResult UpdateTreatment([FromBody] Treatment treatment)
    {
        var tret = db.Treatment.FirstOrDefault(t => t.Id == treatment.Id);
        if (tret == null) return NotFound();

        try
        {
            db.Treatment.Where(t => t.Id == treatment.Id).
                ExecuteUpdate(u => u
                    .SetProperty(p => p.Name, treatment.Name)
                    .SetProperty(p => p.StartDate, treatment.StartDate)
                    .SetProperty(p => p.EndDate, treatment.EndDate)
                    .SetProperty(p => p.Type, treatment.Type)
                    .SetProperty(p => p.Price, treatment.Price)
                    );
            return NoContent();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpGet("RevByAnimal/{type}")]
    public ActionResult GetRevenuByAnimal(AnimalTypeEnum type)
    {
        var res = db.Treatment.Where(t => t.Type == type).Sum(p => p.Price);
        return Ok(new { Animal =  type.ToString(), Revenue = res });
    }

    [HttpGet("TrFrequency")]
    public ActionResult GetTreatmentFrequency()
    {
        var res = db.Treatment.GroupBy(t => t.Type).Select(x => new
        {
            Type = x.Key.ToString(),
            Count = x.Count()
        }).ToList();
        return Ok(res);
    }

}
