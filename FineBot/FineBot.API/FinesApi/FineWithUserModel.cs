using FineBot.API.UsersApi;

namespace FineBot.API.FinesApi
{
    public class FineWithUserModel : FineModel
    {
         public UserModel User { get; set; }
    }
}