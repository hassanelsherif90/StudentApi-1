using StudentApi.Data.Entities;

namespace StudentApi.Authorization
{
    //"هنا للحصول على وظيفة الفحص "

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CheckPermissionAttribute : Attribute
    {
        public Permission Permission { get; set; }

        public CheckPermissionAttribute(Permission permission)
        {
            Permission = permission;
        }

    }
}
