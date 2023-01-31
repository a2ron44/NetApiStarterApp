using System;
namespace NetApiStarterLibrary.Models
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

