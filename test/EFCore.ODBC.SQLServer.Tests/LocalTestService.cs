using Microsoft.EntityFrameworkCore;

namespace EFCore.ODBC.SQLServer.Tests;
public class LocalTestService
{
    private readonly LocalContext _context;

    public LocalTestService(LocalContext context)
    {
        _context = context;
    }

    public async Task<ComplexEntity?> GetComplexEntityAsync(int id)
    {
        return await _context.ComplexEntities
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<ComplexEntity>> GetEntitiesWithUnionAsync(int id1, int id2)
    {
        var query1 = _context.ComplexEntities.Where(x => x.Id == id1);
        var query2 = _context.ComplexEntities.Where(x => x.Id == id2);

        return await query1.Union(query2).ToListAsync();
    }

    public async Task<List<ComplexEntity>> GetEntitiesWithMultipleUnionsAsync(int id1, int id2, int id3)
    {
        var query1 = _context.ComplexEntities.Where(x => x.Id == id1);
        var query2 = _context.ComplexEntities.Where(x => x.Id == id2);
        var query3 = _context.ComplexEntities.Where(x => x.Id == id3);

        return await query1.Union(query2).Union(query3).ToListAsync();
    }

    public async Task<List<ComplexEntity>> GetEntitiesWithUnionAndFilterAsync(int id1, int id2, string stringValue)
    {
        var query1 = _context.ComplexEntities.Where(x => x.Id == id1);
        var query2 = _context.ComplexEntities.Where(x => x.Id == id2);

        return await query1.Union(query2)
            .Where(x => x.StringValue.Contains(stringValue))
            .ToListAsync();
    }

    public async Task<List<ComplexEntity>> GetEntitiesWithUnionAndSortingAsync(int id1, int id2)
    {
        var query1 = _context.ComplexEntities.Where(x => x.Id == id1);
        var query2 = _context.ComplexEntities.Where(x => x.Id == id2);

        return await query1.Union(query2)
            .OrderBy(x => x.StringValue)
            .ThenByDescending(x => x.DateTimeValue)
            .ToListAsync();
    }

    public async Task<List<ComplexEntity>> GetEntitiesByStringValuesAsync(List<string> stringValues)
    {
        return await _context.ComplexEntities
            .Where(x => stringValues.Contains(x.StringValue))
            .ToListAsync();
    }

    public async Task<ComplexEntity?> GetComplexAndRelatedEntitiesData(string stringValue, DateTime dateTimeValue, string description)
    {
        var query = from c in _context.ComplexEntities
                    join r in _context.RelatedEntities on c.Id equals r.SharedIntValue
                    where c.StringValue == stringValue
                     && c.DateTimeValue <= dateTimeValue
                     && r.Description == description
                    select c;
        return await query.FirstOrDefaultAsync();
    }
}
