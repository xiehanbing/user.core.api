using System;

namespace Project.Domain.Exceptions
{
    public class ProjectDomainException:Exception
    {
        public ProjectDomainException():base()
        {

        }

        public ProjectDomainException(string message) : base(message)
        {

        }
    }
}