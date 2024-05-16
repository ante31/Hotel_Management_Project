using HotelMS.Application.Models.WeatherForecast;

namespace HotelMS.Application.Services;

public interface IWeatherForecastService
{
    public Task<IEnumerable<WeatherForecastResponseModel>> GetAsync();
}
