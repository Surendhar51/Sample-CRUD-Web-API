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
    public class CustomerRepository : Repository<Customer> , ICustomerRepository
    {
        public CustomerRepository(Context context) : base(context)
        {

        }
    }
}
