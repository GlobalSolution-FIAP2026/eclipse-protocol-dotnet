using GlobalSolution.Data;
using GlobalSolution.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution.Services;

public class AlertaService
{
    private readonly AppDbContext _context;

    public AlertaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Alerta>> ListarTodosAsync()
    {
        return await _context.Alertas
            .Include(a => a.Leitura)
            .Include(a => a.Plantacao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Alerta?> BuscarPorIdAsync(long id)
    {
        return await _context.Alertas
            .Include(a => a.Leitura)
            .Include(a => a.Plantacao)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdAlerta == id);
    }

    public async Task<Alerta> CriarAsync(Alerta alerta)
    {
        var leituraExiste = await _context.Leituras
            .AnyAsync(l => l.IdLeitura == alerta.IdLeitura);

        if (!leituraExiste)
            throw new InvalidOperationException("Leitura informada não existe.");

        var plantacaoExiste = await _context.Plantacoes
            .AnyAsync(p => p.IdPlantacao == alerta.IdPlantacao);

        if (!plantacaoExiste)
            throw new InvalidOperationException("Plantação informada não existe.");

        alerta.Status = string.IsNullOrWhiteSpace(alerta.Status) ? "ABERTO" : alerta.Status;
        alerta.DataCriacao = DateTime.Now;

        _context.Alertas.Add(alerta);
        await _context.SaveChangesAsync();

        return alerta;
    }

    public async Task<Alerta?> AtualizarAsync(long id, Alerta alertaAtualizado)
    {
        var alerta = await _context.Alertas.FindAsync(id);

        if (alerta is null)
            return null;

        var leituraExiste = await _context.Leituras
            .AnyAsync(l => l.IdLeitura == alertaAtualizado.IdLeitura);

        if (!leituraExiste)
            throw new InvalidOperationException("Leitura informada não existe.");

        var plantacaoExiste = await _context.Plantacoes
            .AnyAsync(p => p.IdPlantacao == alertaAtualizado.IdPlantacao);

        if (!plantacaoExiste)
            throw new InvalidOperationException("Plantação informada não existe.");

        alerta.TipoAlerta = alertaAtualizado.TipoAlerta;
        alerta.Severidade = alertaAtualizado.Severidade;
        alerta.Mensagem = alertaAtualizado.Mensagem;
        alerta.Status = alertaAtualizado.Status;
        alerta.IdLeitura = alertaAtualizado.IdLeitura;
        alerta.IdPlantacao = alertaAtualizado.IdPlantacao;

        await _context.SaveChangesAsync();

        return alerta;
    }

    public async Task<bool> ExcluirAsync(long id)
    {
        var alerta = await _context.Alertas.FindAsync(id);

        if (alerta is null)
            return false;

        _context.Alertas.Remove(alerta);
        await _context.SaveChangesAsync();

        return true;
    }
}