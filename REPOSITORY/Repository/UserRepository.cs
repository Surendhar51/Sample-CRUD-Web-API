using DATA.DbModel;
using DATA.Model;
using REPOSITORY.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOSITORY.Repository
{
    public class UserRepository : Repository<Users> , IUserRepository
    {
        public UserRepository(Context context) : base(context) { }
    }
}
