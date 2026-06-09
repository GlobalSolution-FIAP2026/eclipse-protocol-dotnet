using GlobalSolution.Data;
using GlobalSolution.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution.Service;

public class LeituraService
{
    private readonly AppDbContext _context;

    public LeituraService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Leitura>> ListarTodosAsync()
    {
        return await _context.Leituras
            .Include(l => l.Sensor)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Leitura?> BuscarPorIdAsync(long id)
    {
        return await _context.Leituras
            .Include(l => l.Sensor)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.IdLeitura == id);
    }

    public async Task<Leitura> CriarAsync(Leitura leitura)
    {
        var sensorExiste = await _context.Sensores
            .CountAsync(s => s.IdSensor == leitura.IdSensor) > 0;

        if (!sensorExiste)
            throw new InvalidOperationException("Sensor informado não existe.");

        leitura.DataLeitura = DateTime.Now;

        _context.Leituras.Add(leitura);
        await _context.SaveChangesAsync();

        return leitura;
    }

    public async Task<Leitura?> AtualizarAsync(long id, Leitura leituraAtualizada)
    {
        var leitura = await _context.Leituras.FindAsync(id);

        if (leitura is null)
            return null;

        var sensorExiste = await _context.Sensores
            .CountAsync(s => s.IdSensor == leituraAtualizada.IdSensor) > 0;

        if (!sensorExiste)
            throw new InvalidOperationException("Sensor informado não existe.");

        leitura.Temperatura = leituraAtualizada.Temperatura;
        leitura.Umidade = leituraAtualizada.Umidade;
        leitura.Precipitacao = leituraAtualizada.Precipitacao;
        leitura.Ndvi = leituraAtualizada.Ndvi;
        leitura.IdSensor = leituraAtualizada.IdSensor;

        await _context.SaveChangesAsync();

        return leitura;
    }

    public async Task<bool> ExcluirAsync(long id)
    {
        var leitura = await _context.Leituras
            .Include(l => l.Alertas)
            .FirstOrDefaultAsync(l => l.IdLeitura == id);

        if (leitura is null)
            return false;

        if (leitura.Alertas.Any())
            throw new InvalidOperationException("Não é possível excluir leitura vinculada a alertas.");

        _context.Leituras.Remove(leitura);
        await _context.SaveChangesAsync();

        return true;
    }
}