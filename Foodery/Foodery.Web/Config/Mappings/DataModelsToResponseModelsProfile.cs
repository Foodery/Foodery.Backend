using AutoMapper;
using Foodery.Data.Models;
using Foodery.Web.Models.Auth.Register;

namespace Foodery.Web.Config.Mappings
{
    public class DataModelsToResponseModelsProfile : Profile
    {
        public DataModelsToResponseModelsProfile()
        {
            this.CreateMap<User, RegisterResponse>();
        }
    }
}
