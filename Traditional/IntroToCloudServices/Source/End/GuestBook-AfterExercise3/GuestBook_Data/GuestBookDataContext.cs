using System.Linq;

namespace GuestBook_Data
{
  public class GuestBookDataContext
    : Microsoft.WindowsAzure.Storage.Table.DataServices.TableServiceContext
  {

    public GuestBookDataContext(Microsoft.WindowsAzure.Storage.Table.CloudTableClient client)
      : base(client)
    {
    }

    public IQueryable<GuestBookEntry> GuestBookEntry
    {
      get
      {
        return this.CreateQuery<GuestBookEntry>("GuestBookEntry");
      }
    }

  }
}
