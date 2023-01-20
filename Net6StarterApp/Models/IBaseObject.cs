using System;
namespace Net6StarterApp.Models
{
    public interface IBaseObject
    {

        DateTime CreateDate { get; set; }

        string CreatedBy { get; set; }

        DateTime LastModifiedDate { get; set; }

        string LastModifiedBy { get; set; }

        string ModifiedSource { get; set; }
    }
}

