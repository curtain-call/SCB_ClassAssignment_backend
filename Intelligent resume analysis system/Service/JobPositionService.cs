using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class JobPositionService : IJobPositionService
{
    private readonly ApplicationDbContext _context;

    public JobPositionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<JobPosition> GetAll()
    {
        return _context.JobPositions.ToList();
    }

    public JobPosition GetById(int id)
    {
        return _context.JobPositions.Find(id);
    }

    public void Create(JobPosition jobPosition)
    {
        _context.JobPositions.Add(jobPosition);
        _context.SaveChanges();
    }

    public void Update(JobPosition jobPosition)
    {
        _context.Entry(jobPosition).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var jobPosition = _context.JobPositions.Find(id);
        if (jobPosition != null)
        {
            _context.JobPositions.Remove(jobPosition);
            _context.SaveChanges();
        }
    }
}
