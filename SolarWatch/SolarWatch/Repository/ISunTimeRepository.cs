using SolarWatch.Model;

namespace SolarWatch.Repository;

public interface ISunTimeRepository
{
    IEnumerable<SunriseSunsetTimes> GetAll();
    SunriseSunsetTimes? GetByName(int id);
    void Add(SunriseSunsetTimes time);
    void Delete(SunriseSunsetTimes time);
    void Update(SunriseSunsetTimes time);
}