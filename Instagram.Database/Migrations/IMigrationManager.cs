using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Database.Migrations
{
    public interface IMigrationManager
    {
        void CreateDatabase();
        void MigrateDatabase();
    }
}
