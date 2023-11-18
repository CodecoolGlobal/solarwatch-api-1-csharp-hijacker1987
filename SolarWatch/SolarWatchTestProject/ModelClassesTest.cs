using SolarWatch.Model;

namespace SolarWatchTestProject;

[TestFixture]
public class CityCoordinateTests
{
    [Test]
    public void CityCoordinate_WithValidValues_SetValuesCorrectly()
    {
        const int expectedId = 1;
        const string expectedName = "Budapest";
        const double expectedLatitude = 37.7749;
        const double expectedLongitude = -122.4194;
        const string expectedState = "-";
        const string expectedCountry = "HU";

        var cityCoordinate = new City(expectedName, expectedLongitude, expectedLatitude, expectedState, expectedCountry) { Id = expectedId };
        Assert.Multiple(() =>
        {
            Assert.That(cityCoordinate.Id, Is.EqualTo(expectedId), "Id should be set to the provided value.");
            Assert.That(cityCoordinate.Name, Is.EqualTo(expectedName), "Name should be set to the provided value.");
            Assert.That(cityCoordinate.Latitude, Is.EqualTo(expectedLatitude), "Latitude should be set to the provided value.");
            Assert.That(cityCoordinate.Longitude, Is.EqualTo(expectedLongitude), "Longitude should be set to the provided value.");
            Assert.That(cityCoordinate.Latitude, Is.EqualTo(expectedLatitude), "State should be set to the provided value.");
            Assert.That(cityCoordinate.Latitude, Is.EqualTo(expectedLatitude), "Country should be set to the provided value.");
        });
        
        const string expectedName2 = "Chicago";
        const double expectedLatitude2 = 38.7749;
        const double expectedLongitude2 = -121.4194;
        const string expectedState2 = "US";
        const string expectedCountry2 = "Illinois";
        
        cityCoordinate.ChangeCityData(expectedName2, expectedLongitude2, expectedLatitude2, expectedState2, expectedCountry2);
        Assert.Multiple(() =>
        {
            Assert.That(cityCoordinate.Name, Is.EqualTo(expectedName2), "Name should be set to the provided value.");
            Assert.That(cityCoordinate.Latitude, Is.EqualTo(expectedLatitude2), "Latitude should be set to the provided value.");
            Assert.That(cityCoordinate.Longitude, Is.EqualTo(expectedLongitude2), "Longitude should be set to the provided value.");
            Assert.That(cityCoordinate.Latitude, Is.EqualTo(expectedLatitude2), "State should be set to the provided value.");
            Assert.That(cityCoordinate.Latitude, Is.EqualTo(expectedLatitude2), "Country should be set to the provided value.");
        });
    }
}

[TestFixture]
public class SolarWatchTest
{
    [Test]
    public void CityCoordinate_WithValidValues_SetValuesCorrectly()
    {
        const int expectedId = 1;
        const int expectedCityId = 1;
        const string expectedSunRiseDate = "2000-01-01";
        const string expectedSunSetDate = "2000-01-01";

        var solarWatch = new SunriseSunsetTimes(expectedSunRiseDate, expectedSunSetDate)
            { Id = expectedId, CityId = expectedCityId };
            
        Assert.Multiple(() =>
        {
            Assert.That(solarWatch.Id, Is.EqualTo(expectedId), "Id should be set to the provided value.");
            Assert.That(solarWatch.CityId, Is.EqualTo(expectedCityId), "City Id should be set to the provided value.");
            Assert.That(solarWatch.SunRiseTime, Is.EqualTo(expectedSunRiseDate), "Sun rise date should be set to the provided value.");
            Assert.That(solarWatch.SunSetTime, Is.EqualTo(expectedSunSetDate), "Sun set date should be set to the provided value.");
        });
        
        const string expectedSunRiseDate2 = "2000-01-01";
        const string expectedSunSetDate2 = "2000-01-01";
        
        solarWatch.ChangeSunTimeData(expectedSunRiseDate2, expectedSunSetDate2);
        
        Assert.Multiple(() =>
        {
            Assert.That(solarWatch.SunRiseTime, Is.EqualTo(expectedSunRiseDate2), "Sun rise date should be set to the provided value.");
            Assert.That(solarWatch.SunSetTime, Is.EqualTo(expectedSunSetDate2), "Sun set date should be set to the provided value.");
        });
    }
}