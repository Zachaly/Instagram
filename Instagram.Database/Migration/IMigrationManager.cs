using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Database.Migration
{
    public interface IMigrationManager
    {
        void CreateDatabase();
        void MigrateDatabase();
    }
}
