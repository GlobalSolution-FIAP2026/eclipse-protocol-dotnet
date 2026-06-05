using GlobalSolution.Data;
using GlobalSolution.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution.Service;

public class PlantacaoService
{
    private readonly AppDbContext _context;

    public PlantacaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Plantacao>> ListarTodosAsync()
    {
        return await _context.Plantacoes
            .Include(p => p.Propriedade)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Plantacao?> BuscarPorIdAsync(long id)
    {
        return await _context.Plantacoes
            .Include(p => p.Propriedade)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdPlantacao == id);
    }

    public async Task<Plantacao> CriarAsync(Plantacao plantacao)
    {
        var propriedadeExiste = await _context.Propriedades
            .AnyAsync(p => p.IdPropriedade == plantacao.IdPropriedade);

        if (!propriedadeExiste)
            throw new InvalidOperationException("Propriedade informada não existe.");

        _context.Plantacoes.Add(plantacao);
        await _context.SaveChangesAsync();

        return plantacao;
    }

    public async Task<Plantacao?> AtualizarAsync(long id, Plantacao plantacaoAtualizada)
    {
        var plantacao = await _context.Plantacoes.FindAsync(id);

        if (plantacao is null)
            return null;

        var propriedadeExiste = await _context.Propriedades
            .AnyAsync(p => p.IdPropriedade == plantacaoAtualizada.IdPropriedade);

        if (!propriedadeExiste)
            throw new InvalidOperationException("Propriedade informada não existe.");

        plantacao.Nome = plantacaoAtualizada.Nome;
        plantacao.Cultura = plantacaoAtualizada.Cultura;
        plantacao.AreaHectares = plantacaoAtualizada.AreaHectares;
        plantacao.Status = plantacaoAtualizada.Status;
        plantacao.IdPropriedade = plantacaoAtualizada.IdPropriedade;

        await _context.SaveChangesAsync();

        return plantacao;
    }

    public async Task<bool> ExcluirAsync(long id)
    {
        var plantacao = await _context.Plantacoes
            .Include(p => p.Sensores)
            .Include(p => p.Alertas)
            .FirstOrDefaultAsync(p => p.IdPlantacao == id);

        if (plantacao is null)
            return false;

        if (plantacao.Sensores.Any())
            throw new InvalidOperationException("Não é possível excluir plantação vinculada a sensores.");

        if (plantacao.Alertas.Any())
            throw new InvalidOperationException("Não é possível excluir plantação vinculada a alertas.");

        _context.Plantacoes.Remove(plantacao);
        await _context.SaveChangesAsync();

        return true;
    }
}