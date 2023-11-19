using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Repository;

public class SunTimeRepository : ISunTimeRepository
{
    
    private readonly UsersContext _context;

    public SunTimeRepository(UsersContext context)
    {
        _context = context;
    }
    
    public IEnumerable<SunriseSunsetTimes> GetAll()
    {
        return _context.Times.ToList();
    }

    public SunriseSunsetTimes? GetByName(int id)
    {
        return _context.Times.FirstOrDefault(time => time.Id == id);
    }

    public void Add(SunriseSunsetTimes time)
    {
        _context.Times.Add(time);
    }

    public void Delete(SunriseSunsetTimes time)
    {
        _context.Times.Remove(time);
    }

    public void Update(SunriseSunsetTimes time)
    {
        _context.Times.Update(time);
        _context.SaveChanges();
    }
}