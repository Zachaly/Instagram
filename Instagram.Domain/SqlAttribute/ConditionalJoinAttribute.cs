﻿namespace Instagram.Domain.SqlAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ConditionalJoinAttribute : JoinAttribute
    {
        public string JoinedColumn { get; set; } = "";
    }
}
