using System.Linq;
using core.domain;
using core.persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.persistence.ef
{
    public class EFCustomizedProductSerialNumberRepository : CustomizedProductSerialNumberRepository
    {
        /// <summary>
        /// Current database context.
        /// </summary>
        protected readonly MyCContext dbContext;

        /// <summary>
        /// Constructs a new instance of EFCustomizedProductSerialNumberRepository with a given database context.
        /// </summary>
        /// <param name="dbContext"></param>
        public EFCustomizedProductSerialNumberRepository(MyCContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public CustomizedProductSerialNumber findSerialNumber()
        {
            CustomizedProductSerialNumber instance = dbContext.CustomizedProductSerialNumber.SingleOrDefault();

            if (instance == null)
            {
                instance = new CustomizedProductSerialNumber();
                dbContext.CustomizedProductSerialNumber.Add(instance);
                dbContext.SaveChanges();
            }

            return instance;
        }


        public CustomizedProductSerialNumber increment()
        {
            CustomizedProductSerialNumber serialNumber = findSerialNumber();

            serialNumber.incrementSerialNumber();

            dbContext.Entry(serialNumber).State = EntityState.Modified;
            dbContext.SaveChanges();

            return serialNumber;
        }
    }
}