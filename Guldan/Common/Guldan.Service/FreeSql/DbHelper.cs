using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.Aop;
using Guldan.Common.Extension;
using Guldan.Service.FreeSql.Base;

namespace Guldan.Service.FreeSql
{
    public class DbHelper
    {
        public static void AuditValue(AuditValueEventArgs e, TimeSpan timeOffset, IUserContext user)
        {

            if (user == null || user.Id.IsNull())
            {
                return;
            }

            if (e.AuditValueType == AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case "Create_By":
                        if (e.Value == null || ((string)e.Value).IsNull())
                        {
                            e.Value = user.Name;
                        }
                        break;
                    case "Create_Time":
                        if (e.Value == null || ((string)e.Value).IsNull())
                        {
                            e.Value = DateTime.Now;
                        }
                        break;

                }
            }
            else if (e.AuditValueType == AuditValueType.Update)
            {
                switch (e.Property.Name)
                {
                    case "Modify_By":
                        e.Value = user.Name;
                        break;
                    case "Modify_Time":
                        e.Value = DateTime.Now;
                        break;
                }
            }
        }




    }
}
