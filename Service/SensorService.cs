using GlobalSolution.Data;
using GlobalSolution.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution.Service;

public class SensorService
{
    private readonly AppDbContext _context;

    public SensorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sensor>> ListarTodosAsync()
    {
        return await _context.Sensores
            .Include(s => s.Plantacao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Sensor?> BuscarPorIdAsync(long id)
    {
        return await _context.Sensores
            .Include(s => s.Plantacao)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.IdSensor == id);
    }

    public async Task<Sensor> CriarAsync(Sensor sensor)
    {
        var plantacaoExiste = await _context.Plantacoes
            .AnyAsync(p => p.IdPlantacao == sensor.IdPlantacao);

        if (!plantacaoExiste)
            throw new InvalidOperationException("Plantação informada não existe.");

        sensor.Ativo = true;
        sensor.DataInstalacao = DateTime.Now;

        _context.Sensores.Add(sensor);
        await _context.SaveChangesAsync();

        return sensor;
    }

    public async Task<Sensor?> AtualizarAsync(long id, Sensor sensorAtualizado)
    {
        var sensor = await _context.Sensores.FindAsync(id);

        if (sensor is null)
            return null;

        var plantacaoExiste = await _context.Plantacoes
            .AnyAsync(p => p.IdPlantacao == sensorAtualizado.IdPlantacao);

        if (!plantacaoExiste)
            throw new InvalidOperationException("Plantação informada não existe.");

        sensor.Nome = sensorAtualizado.Nome;
        sensor.Tipo = sensorAtualizado.Tipo;
        sensor.Ativo = sensorAtualizado.Ativo;
        sensor.IdPlantacao = sensorAtualizado.IdPlantacao;

        await _context.SaveChangesAsync();

        return sensor;
    }

    public async Task<bool> ExcluirAsync(long id)
    {
        var sensor = await _context.Sensores
            .Include(s => s.Leituras)
            .FirstOrDefaultAsync(s => s.IdSensor == id);

        if (sensor is null)
            return false;

        if (sensor.Leituras.Any())
            throw new InvalidOperationException("Não é possível excluir sensor que possui leituras registradas.");

        _context.Sensores.Remove(sensor);
        await _context.SaveChangesAsync();

        return true;
    }
}